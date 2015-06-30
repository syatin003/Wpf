using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EmailHeaderModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly EmailHeader _emailHeader;
        private string _headerImageUrl;


        #endregion

        #region Properties

        [DataMember]
        public EmailHeader EmailHeader
        {
            get { return _emailHeader; }
        }

        [DataMember]
        public String Name
        {
            get { return _emailHeader.Name; }
            set
            {
                if (_emailHeader.Name == value) return;
                _emailHeader.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }


        public string HeaderImageUrl
        {
            get { return _headerImageUrl; }
            set
            {
                if (_headerImageUrl == value) return;
                _headerImageUrl = value;
                RaisePropertyChanged(() => HeaderImageUrl);
                RaisePropertyChanged(() => IsImageUploaded);
            }
        }
        public string ImageName
        {
            get { return _emailHeader.ImageName; }
            set
            {
                if (_emailHeader.ImageName == value) return;
                _emailHeader.ImageName = value;
                RaisePropertyChanged(() => ImageName);
                RaisePropertyChanged(() => ImageUrl);
            }
        }
        public String ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageName))
                {
                    var appPath = (string)ApplicationSettings.Read("ImagesPath");
                    if (!string.IsNullOrWhiteSpace(appPath))
                        return string.Concat(appPath, _emailHeader.ImageName);
                }
                return null;
            }
        }

        public bool IsImageUploaded
        {
            get { return !string.IsNullOrEmpty(_headerImageUrl); }
        }

        #endregion

        #region Constructor

        public EmailHeaderModel(EmailHeader EmailHeader)
        {
            _emailHeader = EmailHeader;
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(EmailHeaderModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        Error = "Name can't be empty!";
                }
                if (columnName == "HeaderImageUrl")
                {
                    if (string.IsNullOrEmpty(HeaderImageUrl))
                        Error = "Header Image is required!";
                }
                return Error;
            }
        }

        public string Error { get; protected set; }

        #endregion IDataErrorInfo
    }
}
