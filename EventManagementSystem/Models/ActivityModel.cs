using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class ActivityModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly Activity _activity;
        private FollowUpModel _followUp;

        #endregion

        #region Properties

        [DataMember]
        public Activity Activity
        {
            get { return _activity; }
        }

        public DateTime LoadedTime { get; set; }

        [DataMember]
        public EventType EnquiryType
        {
            get { return _activity.Enquiry.EventType; }
        }

        [DataMember]
        public ActivityType ActivityType
        {
            get { return _activity.ActivityType; }
            set
            {
                if (_activity.ActivityType == value) return;
                _activity.ActivityType = value;
                RaisePropertyChanged(() => ActivityType);
            }
        }

        [DataMember]
        public string Direction
        {
            get { return _activity.Direction; }
            set
            {
                if (_activity.Direction == value) return;
                _activity.Direction = value;
                RaisePropertyChanged(() => Direction);
            }
        }

        [DataMember]
        public string Details
        {
            get { return _activity.Details; }
            set
            {
                if (_activity.Details == value) return;
                _activity.Details = value;
                RaisePropertyChanged(() => Details);
            }
        }

        [DataMember]
        public string Length
        {
            get { return _activity.Length; }
            set
            {
                if (_activity.Length == value) return;
                _activity.Length = value;
                RaisePropertyChanged(() => Length);
            }
        }

        public FollowUpModel FollowUp
        {
            get { return _followUp; }
            set
            {
                if (_followUp == value) return;
                _followUp = value;
                RaisePropertyChanged(() => FollowUp);
            }
        }

        public bool HasFollowUp
        {
            get
            {
                return FollowUp != null;
            }
        }

        [DataMember]
        public DateTime Date
        {
            get
            {
                return _activity.Date;
            }
            set
            {
                if (_activity.Date == value) return;
                _activity.Date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        [DataMember]
        public string DateOnly
        {
            get { return _activity.Date.Date.ToString("dd/MM/yy"); }
        }

        [DataMember]
        public User Assignee
        {
            get
            {
                return _activity.User;
            }
            set
            {
                if (_activity.User == value) return;
                _activity.User = value;
                RaisePropertyChanged(() => Assignee);
            }
        }

        public string EnquiryName
        {
            get
            {
                if (Activity.Enquiry.Contact != null)
                    return Activity.Enquiry.EventType.Name + " [" + Activity.Enquiry.Contact.FirstName + " " + Activity.Enquiry.Contact.LastName + "] " + Activity.Enquiry.Name;
                else
                    return Activity.Enquiry.EventType.Name + " " + Activity.Enquiry.Name;
            }
        }

        #endregion

        #region Constructor

        public ActivityModel(Activity activity)
        {
            _activity = activity;

            LoadedTime = DateTime.Now;

            if (_activity.FollowUp != null)
                _followUp = new FollowUpModel(_activity.FollowUp);
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => Direction);
            RaisePropertyChanged(() => ActivityType);
            RaisePropertyChanged(() => Details);
            RaisePropertyChanged(() => Length);
            RaisePropertyChanged(() => HasFollowUp);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(ActivityModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Details")
                    if (string.IsNullOrWhiteSpace(Details))
                        Error = "Details text can't be empty.";

                //if (columnName == "ActivityType")
                //    if (ActivityType == null)
                //        Error = "ActivityType can't be empty!";

                if (columnName == "Length")
                    if (string.IsNullOrWhiteSpace(Length))
                        Error = "Length can't be empty!";

                if (columnName == "Direction")
                    if (string.IsNullOrWhiteSpace(Direction))
                        Error = "Direction text can't be empty.";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
