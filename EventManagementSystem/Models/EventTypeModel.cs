using System.Collections.ObjectModel;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using System.Linq;
using System;

namespace EventManagementSystem.Models
{
    public class EventTypeModel : ModelBase, IEquatable<EventTypeModel>
    {
        #region Fields

        private readonly EventType _eventType;
        private ObservableCollection<EventOptionModel> _options;
        private bool _isChecked;
        private ObservableCollection<EventTypeToDoModel> _eventTypeToDos;

        #endregion

        #region Properties

        public EventType EventType
        {
            get { return _eventType; }
        }

        public string Name
        {
            get
            {
                return _eventType.Name;
            }
            set
            {
                if (_eventType.Name == value) return;
                _eventType.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public bool AllowEnquiry
        {
            get
            {
                return _eventType.AllowEnquiry;
            }
            set
            {
                if (_eventType.AllowEnquiry == value) return;
                _eventType.AllowEnquiry = value;
                RaisePropertyChanged(() => AllowEnquiry);
            }
        }

        public bool AllowToBeBooked
        {
            get
            {
                return _eventType.AllowToBeBooked;
            }
            set
            {
                if (_eventType.AllowToBeBooked == value) return;
                _eventType.AllowToBeBooked = value;
                RaisePropertyChanged(() => AllowToBeBooked);
            }
        }

        public string PreferredName
        {
            get
            {
                return _eventType.PreferredName;
            }
            set
            {
                if (_eventType.PreferredName == value) return;
                _eventType.PreferredName = value;
                RaisePropertyChanged(() => PreferredName);
            }
        }

        public string Description
        {
            get
            {
                return _eventType.Description;
            }
            set
            {
                if (_eventType.Description == value) return;
                _eventType.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public string Colour
        {
            get
            {
                return _eventType.Colour;
            }
            set
            {
                if (_eventType.Colour == value) return;
                _eventType.Colour = value;
                RaisePropertyChanged(() => Colour);
            }
        }

        public string Abbreviation
        {
            get
            {
                return _eventType.Abbreviation;
            }
            set
            {
                if (_eventType.Abbreviation == value) return;
                _eventType.Abbreviation = value;
                RaisePropertyChanged(() => Abbreviation);
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _eventType.IsEnabled;
            }
            set
            {
                if (_eventType.IsEnabled == value) return;
                _eventType.IsEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        public User User
        {
            get
            {
                if (!AllowEnquiry)
                    return null;
                return _eventType.User;
            }
            set
            {
                if (_eventType.User == value) return;
                _eventType.User = value;
                RaisePropertyChanged(() => User);
            }
        }

        public ObservableCollection<EventOptionModel> Options
        {
            get { return _options; }
            set
            {
                if (_options == value) return;
                _options = value;
                RaisePropertyChanged(() => Options);
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        public string CorrespondenceTokens
        {
            get { return string.Concat(_eventType.Token1, _eventType.Token2, _eventType.Token3, _eventType.Token4, _eventType.Token5); }
        }

        public ObservableCollection<EventTypeToDoModel> EventTypeToDos
        {
            get { return _eventTypeToDos; }
            set
            {
                if (_eventTypeToDos == value) return;
                _eventTypeToDos = value;
                RaisePropertyChanged(() => EventTypeToDos);
            }
        }
        #endregion

        #region Constructors

        public EventTypeModel(EventType type)
        {
            _eventType = type;
            if (type.EventTypeTODOs != null)
                EventTypeToDos = new ObservableCollection<EventTypeToDoModel>(type.EventTypeTODOs.Select(eventTypeToDo => new EventTypeToDoModel(eventTypeToDo)));
        }

        #endregion

        public bool Equals(EventTypeModel other)
        {
            return this.EventType.ID == other.EventType.ID;
        }
    }
}
