using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class FollowUpModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private  FollowUp _followUp;

        #endregion

        #region Properties

        [DataMember]
        public FollowUp FollowUp
        {
            get { return _followUp; }
            set
            {
                _followUp = value;
            }
        }

        public DateTime LoadedTime { get; set; }

        [DataMember]
        public User TakenByUser
        {
            get { return _followUp.User; }
            set
            {
                if (_followUp.User == value) return;
                _followUp.User = value;
                RaisePropertyChanged(() => TakenByUser);
            }
        }

        [DataMember]
        public User AssignedToUser
        {
            get { return _followUp.User1; }
            set
            {
                if (_followUp.User1 == value) return;
                _followUp.User1 = value;
                RaisePropertyChanged(() => AssignedToUser);
            }
        }

        [DataMember]
        public string WhatToDo
        {
            get { return _followUp.WhatToDo; }
            set
            {
                if (_followUp.WhatToDo == value) return;
                _followUp.WhatToDo = value;
                RaisePropertyChanged(() => WhatToDo);
            }
        }

        [DataMember]
        public DateTime DateDue
        {
            get { return _followUp.DateDue; }
            set
            {
                if (_followUp.DateDue == value) return;
                _followUp.DateDue = value;
                RaisePropertyChanged(() => DateDue);
            }
        }

        [DataMember]
        public string DateOnly
        {
            get { return _followUp.DateDue.Date.ToString("dd/MM/yy"); }
        }

        [DataMember]
        public int Priority { get; set; }

        public string EnquiryName
        {
            get
            {
                if (FollowUp.Enquiry == null)
                    return String.Empty;
                if (FollowUp.Enquiry.Contact != null)
                    return FollowUp.Enquiry.EventType.Name + " [" + FollowUp.Enquiry.Contact.FirstName + " " + FollowUp.Enquiry.Contact.LastName + "] " + FollowUp.Enquiry.Name;
                else
                    return FollowUp.Enquiry.EventType.Name + " " + FollowUp.Enquiry.Name;
            }
        }

        public bool IsToDo
        {
            get
            {
                return FollowUp.EnquiryID == null;
            }
        }

        #endregion

        #region Constructor

        public FollowUpModel(FollowUp followUp)
        {
            _followUp = followUp;

            LoadedTime = DateTime.Now;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => WhatToDo);
            RaisePropertyChanged(() => DateDue);
            RaisePropertyChanged(() => TakenByUser);
            RaisePropertyChanged(() => AssignedToUser);
            RaisePropertyChanged(() => EnquiryName);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(FollowUpModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "WhatToDo")
                    if (string.IsNullOrWhiteSpace(WhatToDo))
                        Error = "WhatToDo text can't be empty.";

                //if (columnName == "AssignedToUser")
                //    if (AssignedToUser == null)
                //        Error = "AssignedToUser can't be empty!";

                if (columnName == "DateDue")
                    if (DateDue < new DateTime(1900, 1, 1))
                        Error = "Date can't be empty or less by 1900!";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
