using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.Models
{
   class CustomerModel : ModelBase
    {
        #region Fields

        private string _eventName;
        private ContactModel _primaryContact;
        private string _eventType;
        private string _pastEvents;
        private string _future;
        private double _totalCharged;
        private string _eventDate;
        private string _lastVisit;

        #endregion

        #region Properties

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

        public ContactModel PrimaryContact
        {
            get { return _primaryContact; }
            set
            {
                if (_primaryContact == value) return;
                _primaryContact = value;
                RaisePropertyChanged(() => PrimaryContact);
            }
        }

        public string EventType
        {
            get { return _eventType; }
            set
            {
                if (_eventType == value) return;
                _eventType = value;
                RaisePropertyChanged(() => EventType);
            }
        }

        public string PastEvents
        {
            get { return _pastEvents; }
            set
            {
                if (_pastEvents == value) return;
                _pastEvents = value;
                RaisePropertyChanged(() => PastEvents);
            }
        }

        public string Future
        {
            get { return _future; }
            set
            {
                if (_future == value) return;
                _future = value;
                RaisePropertyChanged(() => Future);
            }
        }

        public double TotalCharged
        {
            get { return _totalCharged; }
            set
            {
                if (_totalCharged == value) return;
                _totalCharged = value;
                RaisePropertyChanged(() => TotalCharged);
            }
        }

        public string EventDate
        {
            get { return _eventDate; }
            set
            {
                if (_eventDate == value) return;
                _eventDate = value;
                RaisePropertyChanged(() => EventDate);
            }
        }

        public string LastVisit
        {
            get { return _lastVisit; }
            set
            {
                if (_lastVisit == value) return;
                _lastVisit = value;
                RaisePropertyChanged(() => LastVisit);
            }
        }

        #endregion
    }
}
