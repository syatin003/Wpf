using System;
using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Charges
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        #region Fields

        private readonly EventModel _event;
        private readonly IEventDataUnit _eventDataUnit;
        private InvoiceModel _invoice;


        #endregion

        #region Properties

        public InvoiceModel Invoice
        {
            get { return _invoice; }
            set
            {
                if (_invoice == value) return;
                _invoice = value;
                RaisePropertyChanged(() => Invoice);
            }
        }
        public RelayCommand GetInvoiceNumberCommand { get; private set; }
        public RelayCommand UndoGetInvoiceNumberCommand { get; private set; }

        public RelayCommand ShowInvoiceReportCommand { get; private set; }

        #endregion

        #region Constructors

        public AddInvoiceViewModel(EventModel eventModel)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            GetInvoiceNumberCommand = new RelayCommand(GetInvoiceNumberCommandExecuted, GetInvoiceNumberCommandCanExecute);
            UndoGetInvoiceNumberCommand = new RelayCommand(UndoGetInvoiceNumberCommandExecuted, UndoGetInvoiceNumberCommandCanExecute);
            ShowInvoiceReportCommand = new RelayCommand(ShowInvoiceReportCommandExecuted, ShowInvoiceReportCommandCanExecute);

            Invoice = GetInvoice();
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
            else if (_event.PrimaryContact != null &&
                     !string.IsNullOrWhiteSpace(_event.PrimaryContact.FullAddressSepareted))
                invoice.InvoiceAddress = _event.PrimaryContact.FullAddressSepareted;

            invoice.PropertyChanged += InvoiceModelOnPropertyChanged;

            return invoice;
        }

        private void InvoiceModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            ShowInvoiceReportCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void GetInvoiceNumberCommandExecuted()
        {
            var invoiceNumber = _eventDataUnit.EventsRepository.GetNextInvoiceNumber();

            _event.Event.InvoiceNumber = invoiceNumber;
            Invoice.InvoiceNumber = invoiceNumber;

            GetInvoiceNumberCommand.RaiseCanExecuteChanged();
            UndoGetInvoiceNumberCommand.RaiseCanExecuteChanged();
        }

        private bool GetInvoiceNumberCommandCanExecute()
        {
            return Invoice.InvoiceNumber == 0;
        }

        private void UndoGetInvoiceNumberCommandExecuted()
        {
            _event.Event.InvoiceNumber = null;
            Invoice.InvoiceNumber = 0;

            GetInvoiceNumberCommand.RaiseCanExecuteChanged();
            UndoGetInvoiceNumberCommand.RaiseCanExecuteChanged();
        }

        private bool UndoGetInvoiceNumberCommandCanExecute()
        {
            return Invoice.InvoiceNumber > 0;
        }

        private void ShowInvoiceReportCommandExecuted()
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");
            var fileName = string.Format("{0}_{1}_{2}", _event.Name, "Invoice", DateTime.Now.ToString("g").Replace(":", "").Replace(@"/", ""));
            var reportPath = string.Concat(fileName, ".pdf");
            var exportPath = string.Concat(appPath, reportPath);

            // create a pdf file
            ReportingService.CreateInvoiceReport(Invoice, exportPath);

            // store report in the database
            var report = new Report()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                Name = string.Format("Invoice"),
                Path = reportPath
            };

            _eventDataUnit.InvoicesRepository.Add(_invoice.InnerInvoice);

            _eventDataUnit.ReportsRepository.Add(report);
            _event.Reports.Add(new ReportModel(report));
            _event.RefreshReports();

            // update Updates table
            var update = new EventUpdate()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                UserID = AccessService.Current.User.ID,
                Message = "Event invoice was created",
                OldValue = null,
                NewValue = _invoice.InvoiceNumber.ToString(),
                ItemId = _invoice.InnerInvoice.ID,
                ItemType = "EventInvoice",
                Field = "Invoice",
                Action = UpdateAction.Added
            };

            // add document
            var document = new Document()
            {
                ID = report.ID,
                Path = reportPath,
                Name = string.Format("Invoice for {0}", _event.Name),
                IsEnabled = true,
                IsCommon = false
            };

            _eventDataUnit.DocumentsRepository.Add(document);
            _event.Documents.Add(document);

            _eventDataUnit.EventUpdatesRepository.Add(update);

        }

        private bool ShowInvoiceReportCommandCanExecute()
        {
            return !_invoice.HasErrors;
        }

        #endregion
    }
}
