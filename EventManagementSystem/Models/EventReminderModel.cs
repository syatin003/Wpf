using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventReminderModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private EventReminder _eventReminder;

        #endregion

        #region Properties

        [DataMember]
        public EventReminder EventReminder
        {
            get { return _eventReminder; }
            set
            {
                _eventReminder = value;
            }
        }

        public DateTime LoadedTime { get; set; }

        [DataMember]
        public User AssignedToUser
        {
            get { return _eventReminder.User; }
            set
            {
                if (_eventReminder.User == value) return;
                _eventReminder.User = value;
                RaisePropertyChanged(() => AssignedToUser);
            }
        }

        [DataMember]
        public User CreatedByUser
        {
            get { return _eventReminder.User1; }
            set
            {
                if (_eventReminder.User1 == value) return;
                _eventReminder.User1 = value;
                RaisePropertyChanged(() => CreatedByUser);
            }
        }


        [DataMember]
        public string WhatToDo
        {
            get { return _eventReminder.WhatToDo; }
            set
            {
                if (_eventReminder.WhatToDo == value) return;
                _eventReminder.WhatToDo = value;
                RaisePropertyChanged(() => WhatToDo);
            }
        }

        [DataMember]
        public DateTime DateDue
        {
            get { return _eventReminder.DateDue; }
            set
            {
                if (_eventReminder.DateDue == value) return;
                _eventReminder.DateDue = value;
                RaisePropertyChanged(() => DateDue);
            }
        }

        [DataMember]
        public string DateOnly
        {
            get { return _eventReminder.DateDue.Date.ToString("dd/MM/yy"); }
        }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public EventManagementSystem.Enums.Events.ReminderStatus ReminderStatus
        {
            get
            {
                if (_eventReminder.Status == Convert.ToBoolean(Convert.ToInt32(EventManagementSystem.Enums.Events.ReminderStatus.Complete)))
                    return EventManagementSystem.Enums.Events.ReminderStatus.Complete;
                return EventManagementSystem.Enums.Events.ReminderStatus.Active;
            }

        }

        public DateTime NewDateDue
        {
            get { return _eventReminder.Event.Date.AddDays(_eventReminder.EventTypeTODO.NumberOfDays); }
        }


        public string EventName
        {
            get
            {
                if (EventReminder.Event == null)
                    return String.Empty;
                return EventReminder.Event.Name;
            }
        }
        #endregion

        #region Constructor

        public EventReminderModel(EventReminder eventReminder)
        {
            _eventReminder = eventReminder;

            LoadedTime = DateTime.Now;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => WhatToDo);
            RaisePropertyChanged(() => DateDue);
            RaisePropertyChanged(() => CreatedByUser);
            RaisePropertyChanged(() => AssignedToUser);
            RaisePropertyChanged(() => EventName);
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
                if (columnName == "AssignedToUser")
                    if (AssignedToUser == null)
                        Error = "AssignedToUser can't be empty!";
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
