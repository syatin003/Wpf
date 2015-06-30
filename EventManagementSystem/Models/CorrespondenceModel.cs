using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class CorrespondenceModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly Corresponcence _correspondence;
        private bool _sendMailToCcAddress;
        private string _eventName;
        private ObservableCollection<Document> _documents;
        private ContactModel _contactTo;
        private CorresponcenceType _corresponcenceType;
        private bool _saveOnClientsRecord;

        private const string emailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        #endregion

        #region Properties

        [DataMember]
        public Corresponcence Correspondence
        {
            get { return _correspondence; }
        }

        [DataMember]
        public string FromAddress
        {
            get { return _correspondence.FromAddress; }
            set
            {
                if (_correspondence.FromAddress == value) return;
                _correspondence.FromAddress = value;
                RaisePropertyChanged(() => FromAddress);
            }
        }

        [DataMember]
        public string ToAddress
        {
            get { return _correspondence.ToAddress; }
            set
            {
                if (_correspondence.ToAddress == value) return;
                _correspondence.ToAddress = value;
                RaisePropertyChanged(() => ToAddress);
            }
        }

        public ContactModel ContactTo
        {
            get { return _contactTo; }
            set
            {
                if (_contactTo == value) return;
                _contactTo = value;
                RaisePropertyChanged(() => ContactTo);
            }
        }

        [DataMember]
        public string Message
        {
            get { return _correspondence.Message; }
            set
            {
                if (_correspondence.Message == value) return;
                _correspondence.Message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        [DataMember]
        public string EmailType
        {
            get { return _correspondence.EmailType; }
            set
            {
                if (_correspondence.EmailType == value) return;
                _correspondence.EmailType = value;
                RaisePropertyChanged(() => EmailType);
            }
        }

        [DataMember]
        public CorresponcenceType CorresponcenceType
        {
            get { return _corresponcenceType; }
            set
            {
                if (_corresponcenceType == value) return;
                _corresponcenceType = value;
                _correspondence.CorresponcenceTypeID = _corresponcenceType.ID;

                RaisePropertyChanged(() => CorresponcenceType);
            }
        }

        [DataMember]
        public string Subject
        {
            get { return _correspondence.Subject; }
            set
            {
                if (_correspondence.Subject == value) return;
                _correspondence.Subject = value;
                RaisePropertyChanged(() => Subject);
            }
        }

        [DataMember]
        public DateTime Date
        {
            get { return _correspondence.Date; }
            set
            {
                if (_correspondence.Date == value) return;
                _correspondence.Date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        [DataMember]
        public string CCAddress
        {
            get { return _correspondence.CCAddress; }
            set
            {
                if (_correspondence.CCAddress == value) return;
                _correspondence.CCAddress = value;
                RaisePropertyChanged(() => CCAddress);
            }
        }

        [IgnoreDataMember]
        public bool SendMailToCcAddress
        {
            get { return _sendMailToCcAddress; }
            set
            {
                if (_sendMailToCcAddress == value) return;
                _sendMailToCcAddress = value;
                RaisePropertyChanged(() => SendMailToCcAddress);
            }
        }

        public string EventName
        {
            get { return _eventName; }
            set
            {
                if (_eventName == value) return;
                _eventName = value;
                RaisePropertyChanged(() => EventName);
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

        [DataMember]
        public EmailHeader EmailHeader
        {
            get { return _correspondence.EmailHeader; }
            set
            {
                if (_correspondence.EmailHeader == value) return;
                _correspondence.EmailHeader = value;
                RaisePropertyChanged(() => EmailHeader);
            }
        }

        [IgnoreDataMember]
        public bool SaveOnClientsRecord
        {
            get { return _saveOnClientsRecord; }
            set
            {
                if (_saveOnClientsRecord == value) return;
                _saveOnClientsRecord = value;
                RaisePropertyChanged(() => SaveOnClientsRecord);
            }
        }

        #endregion

        #region Constructor

        public CorrespondenceModel(Corresponcence correspondence)
        {
            _correspondence = correspondence;

            if (correspondence.CorresponcenceType != null)
                _corresponcenceType = _correspondence.CorresponcenceType;
            if (correspondence.Contact != null)
                _contactTo = new ContactModel(correspondence.Contact);

            Documents = new ObservableCollection<Document>();
        }

        #endregion

        #region IDataErrorInfo implementation

        public virtual bool HasErrors
        {
            get { return typeof(CorrespondenceModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "FromAddress")
                {
                    var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

                    if (string.IsNullOrEmpty(FromAddress))
                        Error = "From email address code can't be empty!";
                    else if (!regex.IsMatch(FromAddress))
                        Error = "From email address is not valid";
                }
                if (Correspondence.OwnerType != "Membership" && Correspondence.OwnerType != "Contact")
                {
                    if (columnName == "ToAddress")
                    {
                        var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

                        if (string.IsNullOrEmpty(ToAddress))
                            Error = "To email address code can't be empty!";
                        else if (!regex.IsMatch(ToAddress))
                            Error = "To email address is not valid";
                    }
                }

                if (columnName == "Subject")
                    if (string.IsNullOrEmpty(Subject))
                        Error = "Subject can't be empty";

                if (columnName == "Type")
                    if (string.IsNullOrEmpty(EmailType))
                        Error = "Correspondence type can't be empty";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}