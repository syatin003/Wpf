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
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.Correspondence
{
    public class SendEnquiryMailViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private CorrespondenceModel _correspondence;
        private readonly EnquiryModel _enquiry;
        private readonly ICrmDataUnit _crmDataUnit;
        private ObservableCollection<MailTemplateModel> _mailTemplates;
        private MailTemplateModel _mainEmailTemplate;
        private MailTemplateModel _selectedMailTemplate;
        private bool _isResend;
        private List<CorresponcenceType> _corresponcenceTypes;
        private ObservableCollection<Document> _documents;
        private List<EventNote> _enquiryNotes;
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

        public SendEnquiryMailViewModel(EnquiryModel enquiryModel, CorrespondenceModel mail)
        {
            _enquiry = enquiryModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

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
            this.PropertyChanged += SendEnquiryMailViewModel_PropertyChanged;
        }

        private void SendEnquiryMailViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        private void MailOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        private CorrespondenceModel GetCorrespondence()
        {
            var model = new CorrespondenceModel(new Corresponcence()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                OwnerID = _enquiry.Enquiry.ID,
                OwnerType = "Enquiry",
                EmailType = "Email",
                Message = "",
                FromAddress = AccessService.Current.User.EmailAddress,
                UserID = AccessService.Current.User.ID
            });

            if (_enquiry.PrimaryContact != null)
            {
                model.ToAddress = _enquiry.PrimaryContact.Contact.Email;
                model.ContactTo = _enquiry.PrimaryContact;
                model.Correspondence.ContactToID = _enquiry.PrimaryContact.Contact.ID;
            }

            if (_isResend)
                model.Documents = new ObservableCollection<Document>(Correspondence.Documents);

            return model;
        }

        private CorrespondenceModel CopyCorrespondence(CorrespondenceModel correspondence)
        {
            var model = new CorrespondenceModel(new Corresponcence()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                OwnerID = _enquiry.Enquiry.ID,
                OwnerType = "Enquiry",
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

            _crmDataUnit.MailTemplatesRepository.Refresh();
            var templates = await _crmDataUnit.MailTemplatesRepository.GetAllAsync();
            MainEmailTemplate = new MailTemplateModel(templates.Where(x => x.MailTemplateCategory.Name == "CRM" && x.MailTemplateType.Name == "MainEmailTemplate").FirstOrDefault());
            MailTemplates = new ObservableCollection<MailTemplateModel>(templates.Where(x => x.MailTemplateCategory.Name == "CRM" && x.IsEnabled && x.MailTemplateType.Name != "MainEmailTemplate").OrderBy(x => x.Name).Select(x => new MailTemplateModel(x)));

            _crmDataUnit.EmailHeadersRepository.Refresh();
            var headers = await _crmDataUnit.EmailHeadersRepository.GetAllAsync();
            _allEmailHeaders = new List<EmailHeader>(headers);
            EmailHeaders = new ObservableCollection<EmailHeader>(_allEmailHeaders.Where(x => x.IsEnabled));

            var types = await _crmDataUnit.CorresponcenceTypesRepository.GetAllAsync();
            _corresponcenceTypes = new List<CorresponcenceType>(types);

            var appPath = (string)ApplicationSettings.Read("DocumentsPath");

            var documets = await _crmDataUnit.DocumentsRepository.GetAllAsync(x => x.IsEnabled && x.IsCommon);
            Documents = new ObservableCollection<Document>(documets.Where(x => File.Exists(string.Concat(appPath, x.Path))));

            OnLoadContacts();

            IsBusy = false;
        }

        private async void OnLoadContacts()
        {
            var contacts = await _crmDataUnit.EventContactsRepository.GetAllAsync(x => x.EventID == _enquiry.Enquiry.ID);

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
            var contacts = await _crmDataUnit.EventContactsRepository.GetAllAsync(x => x.EventID == _enquiry.Enquiry.ID);

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
                    _crmDataUnit.CCContactsCorrespondenceRepository.Add(new CCContactsCorrespondence()
                    {
                        ID = Guid.NewGuid(),
                        ContactID = eventContact.ContactID,
                        CorrespondenceID = Correspondence.Correspondence.ID
                    });
                }
            }
        }

        private async void ParseMailMessage()
        {
            var inputText = _selectedMailTemplate.MailTemplate.Template;

            string outputText;
            outputText = inputText
                    .Replace("{#EventName}", _enquiry.Name)
                    .Replace("{#CurrentDate}", DateTime.Now.ToString("d"))
                    .Replace("{#NumofPeople}", _enquiry.Places.ToString())
                    .Replace("{#Type}", _enquiry.EventType.Name)
                    .Replace("{#Status}", _enquiry.EventStatus.Name)
                    .Replace("{#EventDate}", _enquiry.Date == null ? String.Empty : Convert.ToDateTime(_enquiry.Date).ToString("d"));

            outputText = outputText.Replace("{#EmailSignature}", AccessService.Current.User.EmailSignature);

            if (_enquiry.PrimaryContact != null)
            {
                if (_enquiry.PrimaryContact.Title != null)
                    outputText = outputText
                        .Replace("{#ContactTitle}", _enquiry.PrimaryContact.Title.Title);

                outputText = outputText
                    .Replace("{#ContactFirstName}", _enquiry.PrimaryContact.FirstName)
                    .Replace("{#ContactLastName}", _enquiry.PrimaryContact.LastName)
                    .Replace("{#ContactFullName}", _enquiry.PrimaryContact.ContactName)
                    .Replace("{#ContactAddress1}", _enquiry.PrimaryContact.Contact.Address1)
                    .Replace("{#ContactAddress2}", _enquiry.PrimaryContact.Contact.Address2)
                    .Replace("{#ContactAddress3}", _enquiry.PrimaryContact.Contact.Address3)
                    .Replace("{#ContactTown}", _enquiry.PrimaryContact.Contact.City)
                    .Replace("{#ContactCounty}", _enquiry.PrimaryContact.Contact.Country)
                    .Replace("{#ContactPostCode}", _enquiry.PrimaryContact.Contact.PostCode)
                    .Replace("{#ContactAddressBlock}", _enquiry.PrimaryContact.FullAddressSepareted)
                    .Replace("{#ContactCompanyName}", _enquiry.PrimaryContact.Contact.CompanyName)
                    .Replace("{#ContactPhone1}", _enquiry.PrimaryContact.Contact.Phone1)
                    .Replace("{#ContactPhone2}", _enquiry.PrimaryContact.Contact.Phone2)
                    .Replace("{#ContactEmail}", _enquiry.PrimaryContact.Contact.Email);
            }

            // Notes
            if (outputText.Contains("#InternalNotes"))
            {
                if (_enquiryNotes == null)
                    _enquiryNotes = await _crmDataUnit.EventNotesRepository.GetAllAsync(x => x.EventID == _enquiry.Enquiry.ID);

                if (outputText.Contains("#InternalNotes") && _enquiryNotes.Any(x => x.EventNoteType.Type == "Internal"))
                {
                    var notes = _enquiryNotes.Where(x => x.EventNoteType.Type == "Internal");

                    var builder = new StringBuilder();
                    notes.ForEach(x => builder.AppendLine(string.Format("[{0}] {1}", x.Date.ToString("d"), x.Note)));

                    outputText = outputText
                        .Replace("{#InternalNotes}", builder.ToString());
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

            if (_isResend)
            {
                var mail = GetCorrespondence();
                mail.CCAddress = Correspondence.CCAddress;
                mail.Date = DateTime.Now;
                mail.FromAddress = Correspondence.FromAddress;
                mail.ToAddress = Correspondence.ToAddress;
                mail.Message = Correspondence.Message;
                mail.SendMailToCcAddress = Correspondence.SendMailToCcAddress;
                mail.Subject = Correspondence.Subject;
                mail.EmailType = Correspondence.EmailType;
                mail.Correspondence.CorresponcenceTypeID = Correspondence.Correspondence.CorresponcenceTypeID;
                mail.ContactTo = Correspondence.ContactTo;

                Correspondence = mail;
            }
            else
            {
                var type = _corresponcenceTypes.FirstOrDefault(x => x.Type == "Event");
                Correspondence.CorresponcenceType = type;
                Correspondence.Message = Correspondence.Message;
            }

            var onCompletedAction = new Action(() =>
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

                    _crmDataUnit.CorrespondenceDocumentsRepository.Add(correspondenceDocument);
                }

                // add entry into update log
                var update = new EnquiryUpdate()
                {
                    ID = Guid.NewGuid(),
                    EnquiryID = _enquiry.Enquiry.ID,
                    Date = DateTime.Now,
                    Message = string.Format("Email was sent to {0}", _correspondence.ToAddress),
                    UserID = AccessService.Current.User.ID
                };

                _enquiry.EnquiryUpdates.Insert(0, update);
                _crmDataUnit.EnquiryUpdatesRepository.Add(update);

                _enquiry.Correspondences.Insert(0, Correspondence);
                _crmDataUnit.CorresponcencesRepository.Add(Correspondence.Correspondence);
            });

            bool success = await EmailService.SendEmail(Correspondence, onCompletedAction, MainEmailTemplate.MailTemplate.Template);

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
            _crmDataUnit.RevertChanges();
        }

        #endregion
    }
}
