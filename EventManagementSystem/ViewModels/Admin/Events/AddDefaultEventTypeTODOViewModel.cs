using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EventManagementSystem.Data.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Core.Contacts;
using EventManagementSystem.Core.Serialization;

namespace EventManagementSystem.ViewModels.Admin.Events
{
    public class AddDefaultEventTypeTODOViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private EventTypeToDoModel _eventTypeToDo;
        private EventTypeModel _eventType;
        private ObservableCollection<User> _users;
        private bool _isEditMode;
        private bool _eventDateType;
        private bool _bookingDatetype;

        #endregion Fields

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


        public bool EventDateType
        {
            get
            {
                return (EventTypeToDo.RelatedDateType == Convert.ToInt32(Enums.Admin.RelatedDateType.EventDate));
            }
            set
            {
                if (_eventDateType == value) return;
                _eventDateType = value;
                _bookingDatetype = !_eventDateType;
                if (_eventDateType)
                    EventTypeToDo.RelatedDateType = Convert.ToInt32(Enums.Admin.RelatedDateType.EventDate);
                else
                    EventTypeToDo.RelatedDateType = Convert.ToInt32(Enums.Admin.RelatedDateType.BookingDate);
                RaisePropertyChanged(() => EventDateType);
                RaisePropertyChanged(() => BookingDateType);
            }
        }

        public bool BookingDateType
        {
            get
            {
                return (EventTypeToDo.RelatedDateType == Convert.ToInt32(Enums.Admin.RelatedDateType.BookingDate));
            }
            set
            {
                if (_bookingDatetype == value) return;
                _bookingDatetype = value;
                _eventDateType = !_bookingDatetype;
                if (_bookingDatetype)
                    EventTypeToDo.RelatedDateType = Convert.ToInt32(Enums.Admin.RelatedDateType.BookingDate);
                else
                    EventTypeToDo.RelatedDateType = Convert.ToInt32(Enums.Admin.RelatedDateType.EventDate);
                RaisePropertyChanged(() => BookingDateType);
                RaisePropertyChanged(() => EventDateType);
            }
        }

        public EventTypeToDoModel EventTypeToDo
        {
            get { return _eventTypeToDo; }
            set
            {
                if (_eventTypeToDo == value) return;
                _eventTypeToDo = value;
                RaisePropertyChanged(() => EventTypeToDo);
            }
        }

        public EventTypeModel EventType
        {
            get { return _eventType; }
            set
            {
                if (_eventType == value) return;
                _eventType = value;
                RaisePropertyChanged(() => EventType);
            }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                if (_users == value) return;
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }


        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }


        #endregion

        #region Constructors

        public AddDefaultEventTypeTODOViewModel(EventTypeModel eventTypeModel, EventTypeToDoModel eventTypeToDoModel)
        {
            EventType = eventTypeModel;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            ProcessEventTypeToDo(eventTypeToDoModel);
        }


        #endregion

        #region Methods

        private void ProcessEventTypeToDo(EventTypeToDoModel eventTypeToDoModel)
        {

            _isEditMode = (eventTypeToDoModel != null);

            EventTypeToDo = eventTypeToDoModel ?? GetNewEventTypeToDo();
            EventTypeToDo.PropertyChanged += OnEventTypeTODOPropertyChanged;
        }

        private EventTypeToDoModel GetNewEventTypeToDo()
        {
            var eventTypeToDoModel = new EventTypeToDoModel(new EventTypeTODO()
             {
                 ID = Guid.NewGuid(),
                 EventTypeID = EventType.EventType.ID,
                 AddedByUserID = AccessService.Current.User.ID,
                 CreatedDate = DateTime.Now,
                 RelatedDateType = 0
             });
            return eventTypeToDoModel;
        }

        public async void LoadData()
        {
            var users = await _adminDataUnit.UsersRepository.GetUsersAsync();
            Users = new ObservableCollection<User>(users);
        }
        private void OnEventTypeTODOPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            if (_isEditMode)
            {
                EventTypeToDo.EventTypeTODO.LastEditDate = DateTime.Now;
            }
            else
            {
                _adminDataUnit.EventTypeTODOsRepository.Add(EventTypeToDo.EventTypeTODO);
            }
            _adminDataUnit.SaveChanges();
        }

        private bool SaveCommandCanExecute()
        {
            return !EventTypeToDo.HasErrors;
        }


        #endregion
    }
}
