using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Correspondence
{
    public class SendEventMailViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private CorrespondenceModel _correspondence;
        private readonly EventModel _event;
        private readonly IEventDataUnit _eventDataUnit;
        private ObservableCollection<MailTemplateModel> _mailTemplates;
        private MailTemplateModel _mainEmailTemplate;
        private MailTemplateModel _selectedMailTemplate;
        private bool _isResend;
        private List<CorresponcenceType> _corresponcenceTypes;
        private ObservableCollection<Document> _documents;
        private ObservableCollection<EmailHeader> _emailHeaders;
        private List<EmailHeader> _allEmailHeaders;

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

        public CorrespondenceModel Correspondence
        {
            get { return _correspondence; }
            set
            {
                if (_correspondence == value) return;
                _correspondence = value;
                RaisePropertyChanged(() => Correspondence);
            }
        }

        public ObservableCollection<MailTemplateModel> MailTemplates
        {
            get { return _mailTemplates; }
            set
            {
                if (_mailTemplates == value) return;
                _mailTemplates = value;
                RaisePropertyChanged(() => MailTemplates);
            }
        }

        public MailTemplateModel SelectedMailTemplate
        {
            get { return _selectedMailTemplate; }
            set
            {
                if (_selectedMailTemplate == value) return;
                _selectedMailTemplate = value;
                RaisePropertyChanged(() => SelectedMailTemplate);
                
                ParseMailMessage();
            }
        }
        public MailTemplateModel MainEmailTemplate
        {
            get { return _mainEmailTemplate; }
            set
            {
                if (_mainEmailTemplate == value) return;
                _mainEmailTemplate = value;
            }
        }

        public ObservableCollection<Document> Documents
        {
            get { return _documents; }
            set
            {
                if (_documents == value) return;
                _documents = value;
                RaisePropertyChanged(() => Documents);
            }
        }

        public ObservableCollection<EmailHeader> EmailHeaders
        {
            get { return _emailHeaders; }
            set
            {
                if (_emailHeaders == value) return;
                _emailHeaders = value;
                RaisePropertyChanged(() => EmailHeaders);
            }
        }


        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public SendEventMailViewModel(EventModel eventModel, CorrespondenceModel mail)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessMail(mail);
        }

        #endregion

        #region Methods

        private void ProcessMail(CorrespondenceModel mail)
        {
            _isResend = (mail != null);

            Correspondence = (_isResend) ? CopyCorrespondence(mail) : GetCorrespondence();
            Correspondence.PropertyChanged += MailOnPropertyChanged;
            this.PropertyChanged += SendEventMailViewModel_PropertyChanged;
        }

        private void SendEventMailViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedMailTemplate")
            {
                if (_selectedMailTemplate.EmailHeader != null)
                {
                    _correspondence.EmailHeader = _selectedMailTemplate.EmailHeader;
                    EmailHeaders = new ObservableCollection<EmailHeader>(_allEmailHeaders.Where(x => x.IsEnabled || x.ID == _selectedMailTemplate.EmailHeader.ID));
                }
                else
                    EmailHeaders = new ObservableCollection<EmailHeader>(_allEmailHeaders.Where(x => x.IsEnabled));
            }
        }

        private void MailOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        private CorrespondenceModel GetCorrespondence()
        {
            var model = new CorrespondenceModel(new Corresponcence()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                OwnerID = _event.Event.ID,
                OwnerType = "Event", // TODO: Remove
                EmailType = "Email",
                Message = "",
                FromAddress = AccessService.Current.User.EmailAddress,
                UserID = AccessService.Current.User.ID
            });

            if (_event.PrimaryContact != null)
            {
                model.ToAddress = _event.PrimaryContact.Contact.Email;
                model.ContactTo = _event.PrimaryContact;
                model.Correspondence.ContactToID = _event.PrimaryContact.Contact.ID;
            }

            return model;
        }

        private CorrespondenceModel CopyCorrespondence(CorrespondenceModel correspondence)
        {
            var model = new CorrespondenceModel(new Corresponcence()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                OwnerID = _event.Event.ID,
                OwnerType = "Event", // TODO: Remove
                EmailType = "Email",
                UserID = AccessService.Current.User.ID
            })
            {
                CCAddress = correspondence.CCAddress,
                FromAddress = correspondence.FromAddress,
                ToAddress = correspondence.ToAddress,
                Message = correspondence.Message,
                SendMailToCcAddress = correspondence.SendMailToCcAddress,
                Subject = correspondence.Subject,
                EmailType = correspondence.EmailType,
                CorresponcenceType = correspondence.CorresponcenceType,
                ContactTo = correspondence.ContactTo
            };

            return model;
        }

        public async void LoadData()
        {
            IsBusy = true;

            _eventDataUnit.MailTemplatesRepository.Refresh();
            var templates = await _eventDataUnit.MailTemplatesRepository.GetAllAsync();
            MainEmailTemplate = new MailTemplateModel(templates.Where(x => x.MailTemplateCategory.Name == "Events" && x.MailTemplateType.Name == "MainEmailTemplate").FirstOrDefault());
            MailTemplates = new ObservableCollection<MailTemplateModel>(
                templates.Where(x => x.MailTemplateCategory.Name == "Events" && x.IsEnabled && x.MailTemplateType.Name != "MainEmailTemplate").OrderBy(x => x.Name).Select(x => new MailTemplateModel(x)));

            _eventDataUnit.EmailHeadersRepository.Refresh();
            var headers = await _eventDataUnit.EmailHeadersRepository.GetAllAsync();
            _allEmailHeaders = new List<EmailHeader>(headers);
            EmailHeaders = new ObservableCollection<EmailHeader>(_allEmailHeaders.Where(x => x.IsEnabled));

            var types = await _eventDataUnit.CorresponcenceTypesRepository.GetAllAsync();
            _corresponcenceTypes = new List<CorresponcenceType>(types);

            var appPath = (string)ApplicationSettings.Read("DocumentsPath");

            if (!_event.Reports.Any())
            {
                var reports = await _eventDataUnit.ReportsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                _event.Reports = new ObservableCollection<ReportModel>(reports.OrderByDescending(x => x.Date).Select(x => new ReportModel(x)));
                _event.RefreshReports();
            }

            List<ReportModel> latestReports = _event.GetLatestReports();

            //foreach (var report in latestReports)
            //{
            //    if (!_event.Documents.Select(x => x.Path).Contains(report.Report.Path))
            //    {
            //        var document = new Document()
            //        {
            //            ID = Guid.NewGuid(),
            //            EventID = _event.Event.ID,
            //            Path = report.Report.Path,
            //            Name = Path.GetFileNameWithoutExtension(report.Report.Path),
            //            IsEnabled = true,
            //            IsCommon = false
            //        };

            //        _event.Documents.Add(document);
            //    }
            //}


            var commonDocumets = await _eventDataUnit.DocumentsRepository.GetAllAsync(x => x.IsEnabled && x.IsCommon);
            Documents = new ObservableCollection<Document>(commonDocumets.Where(x => File.Exists(string.Concat(appPath, x.Path))));

            foreach (var report in latestReports)
            {

                var eventDocument = _event.Documents.Where(doc => doc.Path == report.Report.Path).FirstOrDefault();
                if (eventDocument == null)
                {
                    eventDocument = new Document()
                                        {
                                            ID = report.Report.ID,
                                            EventID = _event.Event.ID,
                                            Path = report.Report.Path,
                                            Name = Path.GetFileNameWithoutExtension(report.Report.Path),
                                            IsEnabled = true,
                                            IsCommon = false
                                        };
                }
                Documents.Add(eventDocument);

            }

            OnLoadContacts();

            IsBusy = false;
        }

        private async void OnLoadContacts()
        {
            var contacts = await _eventDataUnit.EventContactsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);

            if (contacts.Any())
            {
                string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                 + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                 + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                var ccContacts = contacts
                    .Where(x => !string.IsNullOrWhiteSpace(x.Contact.Email) && regex.IsMatch(x.Contact.Email))
                    .Select(x => x.Contact.Email).ToList();

                Correspondence.CCAddress = string.Join(",", ccContacts);
            }
        }

        private async void AddCCContacts()
        {
            var contacts = await _eventDataUnit.EventContactsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);

            if (contacts.Any())
            {
                string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                 + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                 + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                var ccContacts = contacts
                    .Where(x => !string.IsNullOrWhiteSpace(x.Contact.Email) && regex.IsMatch(x.Contact.Email))
                    .ToList();

                foreach (var eventContact in ccContacts)
                {
                    _eventDataUnit.CCContactsCorrespondenceRepository.Add(new CCContactsCorrespondence()
                    {
                        ID = Guid.NewGuid(),
                        ContactID = eventContact.ContactID,
                        CorrespondenceID = Correspondence.Correspondence.ID
                    });
                }
            }
        }

        private void ParseMailMessage()
        {
            var inputText = _selectedMailTemplate.MailTemplate.Template;

            string outputText;

            outputText = inputText
                .Replace("{#EventName}", _event.Name)
                .Replace("{#CurrentDate}", DateTime.Now.ToString("d"))
                .Replace("{#EventDate}", _event.Event.Date.ToString("d"))
                .Replace("{#CreationDate}", _event.Event.CreationDate.ToString("d"))
                .Replace("{#NumofPeople}", _event.Places.ToString())
                .Replace("{#Type}", _event.EventType.Name)
                .Replace("{#Status}", _event.EventStatus.Name)
                .Replace("{#Value}", _event.EventItems.Where(eventItem => eventItem.IncludeInCorrespondence == true).Sum(eventItem => eventItem.TotalPrice).ToString("C"))
                .Replace("{#ValueEach}", _event.EventPricePerPerson.ToString("C"));

            outputText = outputText.Replace("{#EmailSignature}", AccessService.Current.User.EmailSignature);

            if (_event.PrimaryContact != null)
            {
                if (_event.PrimaryContact.Title != null)
                    outputText = outputText
                        .Replace("{#ContactTitle}", _event.PrimaryContact.Title.Title);

                outputText = outputText
                    .Replace("{#ContactFirstName}", _event.PrimaryContact.FirstName)
                    .Replace("{#ContactLastName}", _event.PrimaryContact.LastName)
                    .Replace("{#ContactFullName}", _event.PrimaryContact.ContactName)
                    .Replace("{#ContactAddress1}", _event.PrimaryContact.Contact.Address1)
                    .Replace("{#ContactAddress2}", _event.PrimaryContact.Contact.Address2)
                    .Replace("{#ContactAddress3}", _event.PrimaryContact.Contact.Address3)
                    .Replace("{#ContactTown}", _event.PrimaryContact.Contact.City)
                    .Replace("{#ContactCounty}", _event.PrimaryContact.Contact.Country)
                    .Replace("{#ContactPostCode}", _event.PrimaryContact.Contact.PostCode)
                    .Replace("{#ContactAddressBlock}", _event.PrimaryContact.FullAddressSepareted)
                    .Replace("{#ContactCompanyName}", _event.PrimaryContact.Contact.CompanyName)
                    .Replace("{#ContactPhone1}", _event.PrimaryContact.Contact.Phone1)
                    .Replace("{#ContactPhone2}", _event.PrimaryContact.Contact.Phone2)
                    .Replace("{#ContactEmail}", _event.PrimaryContact.Contact.Email);
            }

            // Payments
            if ((new[] 
            {
                "#DepositAmount", 
                "#DepositDate", 
                "#DepositPaymentMethod",
                "#PaymentAmount",
                "#PaymentDate",
                "#PaymentMethod"
            }).Any(w => outputText.Contains(w)))
            {
                if (_event.EventPayments.Any(x => x.IsDeposit))
                {
                    var depositPayment = _event.EventPayments.Last(x => x.IsDeposit);

                    outputText = outputText
                        .Replace("{#DepositAmount}", depositPayment.Amount.ToString("F"))
                        .Replace("{#DepositDate}", depositPayment.Date.ToString("d"))
                        .Replace("{#DepositPaymentMethod}", depositPayment.PaymentMethod.Method);
                }

                if (_event.EventPayments.Any(x => !x.IsDeposit))
                {
                    var recentPayment = _event.EventPayments.Last(x => !x.IsDeposit);

                    outputText = outputText
                        .Replace("{#PaymentAmount}", recentPayment.Amount.ToString("F"))
                        .Replace("{#PaymentDate}", recentPayment.Date.ToString("d"))
                        .Replace("{#PaymentMethod}", recentPayment.PaymentMethod.Method);
                }
            }

            // Event Contacts
            if ((new[] 
            {
                "#SecondaryContactTitle", 
                "#SecondaryContactFirstName", 
                "#SecondaryContactLastName",
                "#SecondaryContactPhone1",
                "#SecondaryContactPhone2",
                "#SecondaryContactEmail"
            }).Any(w => outputText.Contains(w)))
            {
                var firstContact = _event.EventContacts.First();

                if (firstContact.Contact.ContactTitle != null)
                    outputText = outputText
                        .Replace("{#SecondaryContactTitle}", firstContact.Contact.ContactTitle.Title);

                outputText = outputText
                    .Replace("{#SecondaryContactFirstName}", firstContact.Contact.FirstName)
                    .Replace("{#SecondaryContactLastName}", firstContact.Contact.LastName)
                    .Replace("{#SecondaryContactPhone1}", firstContact.Contact.Phone1)
                    .Replace("{#SecondaryContactPhone2}", firstContact.Contact.Phone2)
                    .Replace("{#SecondaryContactEmail}", firstContact.Contact.Email);
            }


            // Notes
            if ((new[] 
            {
                "#ExternalNotes", 
                "#InternalNotes", 
            }).Any(w => outputText.Contains(w)))
            {
                if (outputText.Contains("#ExternalNotes") && _event.EventNotes.Any(x => x.NoteType.Type == "External"))
                {
                    var builder = new StringBuilder();
                    _event.EventNotes.Where(x => x.NoteType.Type == "External")
                        .ForEach(x => builder.AppendLine(string.Format("[{0}] {1} - by {2}", x.EventNote.Date.ToString("d"), x.Note, x.EventNote.User.FirstName)));

                    outputText = outputText
                        .Replace("{#ExternalNotes}", builder.ToString());
                }

                if (outputText.Contains("#InternalNotes") && _event.EventNotes.Any(x => x.NoteType.Type == "Internal"))
                {
                    var builder = new StringBuilder();
                    _event.EventNotes.Where(x => x.NoteType.Type == "Internal")
                        .ForEach(x => builder.AppendLine(string.Format("[{0}] {1}", x.EventNote.Date.ToString("d"), x.Note)));

                    outputText = outputText
                        .Replace("{#InternalNotes}", builder.ToString());
                }
            }

            if (outputText.Contains("#ItemsBlock"))
            {
                if (_event.EventItems.Any())
                {
                    var builder = new StringBuilder();

                    foreach (var itemModel in _event.EventItems)
                    {
                        if (itemModel.IncludeInCorrespondence)
                        {
                            foreach (var product in itemModel.Products)
                            {
                                var text = string.Format("{0} {1} * {2} {1} x {3} = {4} #break#",
                                    itemModel.Time.HasValue ? itemModel.Time.Value.ToString("t") : "",
                                    product.Quantity,
                                    product.Product.Name,
                                    product.Price.ToString("F"),
                                    product.TotalPrice.ToString("F"));

                                builder.Append(text);
                            }
                        }
                    }

                    outputText = outputText
                        .Replace("{#ItemsBlock}", builder.ToString());
                }
            }

            Correspondence.Message = outputText;
            Correspondence.Subject = _selectedMailTemplate.Name;
        }

        #endregion

        #region Commands

        private async void SubmitCommandExecuted()
        {
            IsBusy = true;

            if (!_isResend)
            {
                var type = _corresponcenceTypes.FirstOrDefault(x => x.Type == "Event");
                Correspondence.CorresponcenceType = type;
            }

            var onCompleteAction = new Action(() =>
            {
                if (Correspondence.SendMailToCcAddress)
                    AddCCContacts();

                foreach (var document in Correspondence.Documents)
                {
                    var correspondenceDocument = new CorrespondenceDocument()
                    {
                        ID = Guid.NewGuid(),
                        CorrespondenceID = Correspondence.Correspondence.ID,
                        DocumentID = document.ID
                    };

                    _eventDataUnit.CorrespondenceDocumentsRepository.Add(correspondenceDocument);
                }

                // add entry into update log
                var update = new EventUpdate()
                {
                    ID = Guid.NewGuid(),
                    EventID = _event.Event.ID,
                    Date = DateTime.Now,
                    Message = string.Format("Email was sent to {0}", _correspondence.ToAddress),
                    UserID = AccessService.Current.User.ID,
                    OldValue = null,
                    NewValue = _correspondence.ToAddress,
                    ItemId = _correspondence.Correspondence.ID,
                    ItemType = "Correspondence",
                    Field = "Email",
                    Action = UpdateAction.Added
                };

                _event.EventUpdates.Insert(0, update);
                _eventDataUnit.EventUpdatesRepository.Add(update);

                _event.Correspondences.Insert(0, Correspondence);
                _eventDataUnit.CorresponcencesRepository.Add(Correspondence.Correspondence);
            });

            bool success = await EmailService.SendEmail(Correspondence, onCompleteAction, MainEmailTemplate.MailTemplate.Template);

            IsBusy = false;

            if (success)
                RaisePropertyChanged("CloseDialog");
        }

        private bool SubmitCommandCanExecute()
        {
            return !Correspondence.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            _eventDataUnit.RevertChanges();
        }

        #endregion

    }
}