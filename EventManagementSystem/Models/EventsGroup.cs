using System;
using EventManagementSystem.Core.ViewModels;
using System.Collections.ObjectModel;


namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventsGroup : ModelBase
    {
        private ObservableCollection<EventModel> _events;

        public ObservableCollection<EventModel> Events
        {
            get { return _events; }
            set
            {
                if (_events == value) return;
                _events = value;
                RaisePropertyChanged(() => Events);
            }
        }

        private DateTime _eventDate;

        public DateTime EventDate
        {
            get { return _eventDate; }
            set
            {
                if (_eventDate == value) return;
                _eventDate = value;
                RaisePropertyChanged(() => EventDate);
            }
        }

        public EventsGroup(DateTime date, ObservableCollection<EventModel> events)
        {
            Events = events;
            EventDate = date;
        }

    }

}
