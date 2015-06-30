using System;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class MailTemplateModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly MailTemplate _mailTemplate;

        #endregion

        #region Properties

        public MailTemplate MailTemplate
        {
            get { return _mailTemplate; }
        }

        public DateTime LoadedTime { get; set; }

        public string Name
        {
            get { return _mailTemplate.Name; }
            set
            {
                if (_mailTemplate.Name == value) return;
                _mailTemplate.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string Template
        {
            get { return _mailTemplate.Template; }
            set
            {
                if (_mailTemplate.Template == value) return;
                _mailTemplate.Template = value;
                RaisePropertyChanged(() => Template);
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _mailTemplate.IsEnabled;
            }
            set
            {
                if (_mailTemplate.IsEnabled == value) return;
                _mailTemplate.IsEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        public MailTemplateType MailTemplateType
        {
            get { return _mailTemplate.MailTemplateType; }
            set
            {
                if (_mailTemplate.MailTemplateType == value) return;
                _mailTemplate.MailTemplateType = value;
                RaisePropertyChanged(() => MailTemplateType);
            }
        }

        public MailTemplateCategory MailTemplateCategory
        {
            get { return _mailTemplate.MailTemplateCategory; }
            set
            {
                if (_mailTemplate.MailTemplateCategory == value) return;
                _mailTemplate.MailTemplateCategory = value;
                RaisePropertyChanged(() => MailTemplateCategory);
            }
        }

        public DateTime LastUpdatedDate
        {
            get { return _mailTemplate.LastUpdatedDate; }
            set
            {
                if (_mailTemplate.LastUpdatedDate == value) return;
                _mailTemplate.LastUpdatedDate = value;
                RaisePropertyChanged(() => LastUpdatedDate);
            }
        }

        public User WhoBy
        {
            get { return _mailTemplate.User; }
            set
            {
                if (_mailTemplate.User == value) return;
                _mailTemplate.User = value;
                RaisePropertyChanged(() => WhoBy);
            }
        }

        public EmailHeader EmailHeader
        {
            get { return _mailTemplate.EmailHeader; }
            set
            {
                if (_mailTemplate.EmailHeader == value) return;
                _mailTemplate.EmailHeader = value;
                RaisePropertyChanged(() => EmailHeader);
            }
        }

        #endregion

        #region Constructor

        public MailTemplateModel(MailTemplate template)
        {
            _mailTemplate = template;

            LoadedTime = DateTime.Now;
        }

        #endregion

        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(EventModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Name")
                    if (string.IsNullOrWhiteSpace(Name))
                        Error = "Name can't be empty!";
                if (columnName == "MailTemplateType")
                    if (MailTemplateType == null)
                        Error = "Template Type can't be empty!";
                if (columnName == "MailTemplateCategory")
                    if (MailTemplateCategory == null)
                        Error = "Template Category can't be empty!";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
