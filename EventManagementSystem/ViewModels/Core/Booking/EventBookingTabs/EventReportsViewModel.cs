using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using EventManagementSystem.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using System.Collections.Generic;


namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventReportsViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;
        private readonly IEventDataUnit _eventDataUnit;
        private bool _isBusy;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public EventModel Event
        {
            get { return _event; }
            set
            {
                if (_event == value) return;
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }


        public RelayCommand ShowFunctionSheetCommand { get; private set; }
        public RelayCommand ShowQuoteCommand { get; private set; }
        public RelayCommand ShowConfirmationCommand { get; private set; }
        public RelayCommand<ReportModel> ShowDocumentCommand { get; private set; }

        public RelayCommand AttachDocumentCommand { get; private set; }
        //public RelayCommand<ReportModel> DeleteDocumentCommand { get; private set; }

        #endregion

        #region Constructor

        public EventReportsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            ShowFunctionSheetCommand = new RelayCommand(ShowFunctionSheetCommandExecuted, () => true);
            ShowDocumentCommand = new RelayCommand<ReportModel>(ShowDocumentCommandExecuted, report => true);
            ShowQuoteCommand = new RelayCommand(ShowQuoteCommandExecuted, () => true);
            ShowConfirmationCommand = new RelayCommand(ShowConfirmationCommandExecuted, () => true);
            AttachDocumentCommand = new RelayCommand(AttachDocumentCommandExecuted);
            //DeleteDocumentCommand = new RelayCommand<ReportModel>(DeleteDocumentCommandExecuted);
        }

        #endregion

        #region Methods

        private InvoiceModel GetInvoice()
        {
            var invoice = new InvoiceModel(new Invoice()
            {
                ID = Guid.NewGuid(),
                PaymentDue = DateTime.Now,
                InvoiceDate = DateTime.Now,
                InvoiceNumber = _event.Event.InvoiceNumber ?? 0,
                InvoiceAddress = string.Empty,
                CameFrom = "Event"
            })
            {
                EventModel = _event
            };

            if (!string.IsNullOrWhiteSpace(_event.Event.InvoiceAddress))
                invoice.InvoiceAddress = _event.Event.InvoiceAddress;
            else if (_event.PrimaryContact != null && !string.IsNullOrWhiteSpace(_event.PrimaryContact.FullAddress))
                invoice.InvoiceAddress = _event.PrimaryContact.FullAddress;

            return invoice;
        }

        public async void LoadEventData()
        {
            IsBusy = true;

            if (!_event.Reports.Any())
            {
                var reports = await _eventDataUnit.ReportsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                _event.Reports = new ObservableCollection<ReportModel>(reports.OrderByDescending(x => x.Date).Select(x => new ReportModel(x)));
                _event.RefreshReports();
            }
            else
            {
                //var desiredEvent = await _eventDataUnit.EventsRepository.GetUpdatedEvent(_event.Event.ID);

                //if (desiredEvent != null && desiredEvent.LastEditDate != null && _event.LoadedTime < desiredEvent.LastEditDate)
                //{
                //    _eventDataUnit.ReportsRepository.Refresh();
                //    var reports = await _eventDataUnit.ReportsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                //    _event.Reports = new ObservableCollection<ReportModel>(reports.OrderByDescending(x => x.Date).Select(x => new ReportModel(x)));
                //}
            }

            IsBusy = false;
        }

        private void AddFileFromHdd(string path)
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");
            var filePath = Path.GetFileName(path);
            var fullPath = string.Concat(appPath, filePath);

            try
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                File.Copy(path, fullPath);

                var report = new ReportModel(new Report()
                {
                    ID = Guid.NewGuid(),
                    EventID = _event.Event.ID,
                    Date = DateTime.Now,
                    Name = Path.GetFileNameWithoutExtension(path),
                    Path = filePath
                });

                _event.Reports.Insert(0, report);
                _eventDataUnit.ReportsRepository.Add(report.Report);
                _event.RefreshReports();

                // update Updates table
                var update = new EventUpdate()
                {
                    ID = Guid.NewGuid(),
                    EventID = _event.Event.ID,
                    Date = DateTime.Now,
                    UserID = AccessService.Current.User.ID,
                    Message = Resources.MESSAGE_FUNCTION_SHEET_WAS_CREATED,
                    OldValue = null,
                    NewValue = report.Name,
                    ItemId = report.Report.ID,
                    ItemType = "EventReport",
                    Field = "DocumentUpload",
                    Action = UpdateAction.Added
                };

                _eventDataUnit.EventUpdatesRepository.Add(update);

                var document = new Document()
                {
                    ID = report.Report.ID,
                    EventID = _event.Event.ID,
                    Path = filePath,
                    Name = Path.GetFileNameWithoutExtension(path),
                    IsEnabled = true,
                    IsCommon = false
                };

                _event.Documents.Add(document);
                _eventDataUnit.DocumentsRepository.Add(document);

            }
            catch (Exception ex)
            {
                PopupService.ShowMessage(ex.Message, MessageType.Failed);
            }
        }

        #endregion

        #region Commands

        private void ShowFunctionSheetCommandExecuted()
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");

            var eventWasChanged = _event.Event.LastFunctionSheetPrint <= _event.Event.LastEditDate;
            var lastReport = _event.Reports.Where(x => x.Name == "Function Sheet").OrderByDescending(x => x.Report.Date).FirstOrDefault();

            if (!eventWasChanged && lastReport != null)
            {
                var oldExportPath = string.Concat(appPath, lastReport.Report.Path);

                if (File.Exists(oldExportPath))
                {
                    Process.Start(oldExportPath);
                    return;
                }
            }

            var fileName = string.Format("{0}_{1}_{2}", _event.Name, "Function Sheet", DateTime.Now.ToString("g").Replace(":", "").Replace(@"/", ""));
            var reportPath = string.Concat(fileName, ".pdf");
            var exportPath = string.Concat(appPath, reportPath);

            // create a pdf file
            ReportingService.CreateFunctionSheetReport(_event, exportPath);

            // store report in the database
            var report = new Report()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                Name = string.Format("Function Sheet"),
                Path = reportPath
            };

            _event.Event.LastFunctionSheetPrint = DateTime.Now;

            _event.Reports.Insert(0, new ReportModel(report));
            _eventDataUnit.ReportsRepository.Add(report);
            _event.RefreshReports();

            // update Updates table
            var update = new EventUpdate()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                UserID = AccessService.Current.User.ID,
                Message = Resources.MESSAGE_FUNCTION_SHEET_WAS_CREATED,
                OldValue = null,
                NewValue = report.Name,
                ItemId = report.ID,
                ItemType = "EventReport",
                Field = "FunctionSheetReport",
                Action = UpdateAction.Added
            };

            _event.EventUpdates.Insert(0, update);
            _eventDataUnit.EventUpdatesRepository.Add(update);

            var document = new Document()
            {
                ID = report.ID,
                EventID = _event.Event.ID,
                Path = reportPath,
                Name = Path.GetFileNameWithoutExtension(reportPath),
                IsEnabled = true,
                IsCommon = false
            };

            _event.Documents.Add(document);
            _eventDataUnit.DocumentsRepository.Add(document);
        }

        private void ShowQuoteCommandExecuted()
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");
            var fileName = string.Format("{0}_{1}_{2}", _event.Name, "Quote", DateTime.Now.ToString("g").Replace(":", "").Replace(@"/", ""));
            var reportPath = string.Concat(fileName, ".pdf");
            var exportPath = string.Concat(appPath, reportPath);

            var invoice = GetInvoice();

            // create a pdf file
            ReportingService.CreateQuoteReport(invoice, exportPath);

            // store report in the database
            var report = new Report()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                Name = string.Format("Quote"),
                Path = reportPath
            };

            _event.Reports.Add(new ReportModel(report));
            _eventDataUnit.ReportsRepository.Add(report);
            _event.RefreshReports();

            // update Updates table
            var update = new EventUpdate()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                UserID = AccessService.Current.User.ID,
                Message = "Event quote was created",
                OldValue = null,
                NewValue = report.Name,
                ItemId = report.ID,
                ItemType = "EventReport",
                Field = "QuoteReport",
                Action = UpdateAction.Added
            };

            _event.EventUpdates.Insert(0, update);
            _eventDataUnit.EventUpdatesRepository.Add(update);

            var document = new Document()
            {
                ID = report.ID,
                EventID = _event.Event.ID,
                Path = reportPath,
                Name = Path.GetFileNameWithoutExtension(reportPath),
                IsEnabled = true,
                IsCommon = false
            };

            _event.Documents.Add(document);
            _eventDataUnit.DocumentsRepository.Add(document);
        }

        private void ShowConfirmationCommandExecuted()
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");
            var fileName = string.Format("{0}_{1}_{2}", _event.Name, "Confirmation", DateTime.Now.ToString("g").Replace(":", "").Replace(@"/", ""));
            var reportPath = string.Concat(fileName, ".pdf");
            var exportPath = string.Concat(appPath, reportPath);

            var invoice = GetInvoice();

            // create a pdf file
            ReportingService.CreateConfirmationReport(invoice, exportPath);

            // store report in the database
            var report = new Report()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                Name = string.Format("Confirmation"),
                Path = reportPath
            };

            _event.Reports.Add(new ReportModel(report));
            _eventDataUnit.ReportsRepository.Add(report);
            _event.RefreshReports();

            // update Updates table
            var update = new EventUpdate()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                UserID = AccessService.Current.User.ID,
                Message = "Event confirmation was created",
                OldValue = null,
                NewValue = report.Name,
                ItemId = report.ID,
                ItemType = "EventReport",
                Field = "ConfirmationReport",
                Action = UpdateAction.Added
            };

            _event.EventUpdates.Insert(0, update);
            _eventDataUnit.EventUpdatesRepository.Add(update);

            var document = new Document()
            {
                ID = report.ID,
                EventID = _event.Event.ID,
                Path = reportPath,
                Name = Path.GetFileNameWithoutExtension(reportPath),
                IsEnabled = true,
                IsCommon = false
            };

            _event.Documents.Add(document);
            _eventDataUnit.DocumentsRepository.Add(document);
        }

        private void ShowDocumentCommandExecuted(ReportModel obj)
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");
            var exportPath = string.Concat(appPath, obj.Report.Path);

            if (File.Exists(exportPath))
            {
                Process.Start(exportPath);
            }
            else
            {
                PopupService.ShowMessage("File not found", MessageType.Failed);
            }
        }

        private void AttachDocumentCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var dialog = new OpenFileDialog() { Multiselect = true };
            var result = dialog.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (result == true)
                dialog.FileNames.ForEach(AddFileFromHdd);
        }

        //private async void DeleteDocumentCommandExecuted(ReportModel obj)
        //{
        //    // Don't allow to execute SaveChanges() if event has errors.
        //    if (_event.HasErrors)
        //    {
        //        RaisePropertyChanged("DisableParentWindow");

        //        RadWindow.Alert("Event has errors. Please fill all required fields to delete any report.");

        //        RaisePropertyChanged("EnableParentWindow");
        //        return;
        //    }

        //    bool? dialogResult = null;
        //    string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

        //    RaisePropertyChanged("DisableParentWindow");

        //    RadWindow.Confirm(new DialogParameters()
        //    {
        //        Owner = Application.Current.MainWindow,
        //        Content = confirmText,
        //        Closed = (sender, args) => { dialogResult = args.DialogResult; }
        //    });

        //    RaisePropertyChanged("EnableParentWindow");

        //    if (dialogResult != true) return;

        //    try
        //    {
        //        var appPath = (string)ApplicationSettings.Read("DocumentsPath");
        //        var path = string.Concat(appPath, obj.Report.Path);

        //        if (File.Exists(path))
        //            File.Delete(path);

        //        _event.Reports.Remove(obj);
        //        _eventDataUnit.ReportsRepository.Delete(obj.Report);

        //        var update = new EventUpdate()
        //        {
        //            ID = Guid.NewGuid(),
        //            Date = DateTime.Now,
        //            Event = _event.Event,
        //            UserID = AccessService.Current.User.ID,
        //            Message = string.Format("File {0} was deleted", obj.Report.Name)
        //        };

        //        _event.EventUpdates.Insert(0, update);
        //        _eventDataUnit.EventUpdatesRepository.Add(update);

        //        var document = _event.Documents.FirstOrDefault(x => x.Path == obj.Report.Path);
        //        _event.Documents.Remove(document);
        //        _eventDataUnit.DocumentsRepository.Delete(document);

        //        await _eventDataUnit.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        PopupService.ShowMessage(ex.Message, MessageType.Failed);
        //    }
        //}

        #endregion
    }
}