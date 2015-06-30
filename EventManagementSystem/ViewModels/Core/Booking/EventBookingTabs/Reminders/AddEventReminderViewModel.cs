using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.Views.CRM;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ObjectBuilder2;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Properties;
using EventManagementSystem.Views.Core.Booking;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Reminders
{
    public class AddEventReminderViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private readonly EventModel _event;
        private bool _isBusy;
        private EventReminderModel _eventReminder;
        private bool _isEditMode;
        private List<EventModel> _events;
        private EventModel _selectedEvent;
        private ObservableCollection<User> _users;
        private EventModel _originalEvent;

        private User _createdByUser;
        private User _assignedToUser;
        #endregion

        #region Properties

        public bool ActivityChanged, FoolowUpsChanged;

        public bool CanEditEveryoneEventReminders { get; private set; }
        public bool CanEditOwnEventReminders { get; private set; }

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

        public EventReminderModel EventReminder
        {
            get { return _eventReminder; }
            set
            {
                if (_eventReminder == value) return;
                _eventReminder = value;
                RaisePropertyChanged(() => EventReminder);
            }
        }
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (_isEditMode == value) return;
                _isEditMode = value;
                RaisePropertyChanged(() => IsEditMode);
            }
        }
        public List<EventModel> Events
        {
            get { return _events; }
            set
            {
                if (_events == value) return;
                _events = value;
                RaisePropertyChanged(() => Events);
            }
        }

        public EventModel SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (_selectedEvent == value) return;
                _selectedEvent = value;
                RaisePropertyChanged(() => SelectedEvent);

                SubmitCommand.RaiseCanExecuteChanged();
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
        public User CreatedByUser
        {
            get { return _createdByUser; }
            set
            {
                if (_createdByUser == value) return;
                _createdByUser = value;
                RaisePropertyChanged(() => CreatedByUser);
            }
        }
        public User AssignedToUser
        {
            get
            {
                return _assignedToUser;
            }
            set
            {
                _assignedToUser = value;
                RaisePropertyChanged(() => AssignedToUser);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool AreEventsVisible
        {
            get { return Events != null; }
        }
        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand OpenEventCommand { get; private set; }

        #endregion

        #region Constructor

        public AddEventReminderViewModel(EventReminderModel eventReminderModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            if (eventReminderModel != null)
            {
                ProcessEventReminder(eventReminderModel);
            }
            else
            {
                EventReminder = GetEventReminderWithoutEvent();
                EventReminder.PropertyChanged += EventReminderOnPropertyChanged;
            }
        }
        public AddEventReminderViewModel(EventModel eventModel, EventReminderModel eventReminderModel)
        {
            _event = eventModel;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            CanEditEveryoneEventReminders = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_FOLLOWUP_ALLOWED);
            CanEditOwnEventReminders = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_FOLLOWUP_ALLOWED);

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessEventReminder(eventReminderModel);
        }
        public AddEventReminderViewModel(IEnumerable<EventModel> events, EventReminderModel eventReminderModel)
        {
            var Today = DateTime.Now;
            Events = events.Where(x => x.Date.Date > Today.Date).OrderBy(x => x.Date).ThenBy(x => x.Name).ToList();

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            CanEditEveryoneEventReminders = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_FOLLOWUP_ALLOWED);
            CanEditOwnEventReminders = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_FOLLOWUP_ALLOWED);

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            OpenEventCommand = new RelayCommand(OpenEventCommandExecute, OpenEventCommandCanExecute);
            if (eventReminderModel != null)
            {
                ProcessEventReminder(eventReminderModel);
                SelectedEvent = Events.FirstOrDefault(x => x.Event == eventReminderModel.EventReminder.Event);
                _originalEvent = SelectedEvent.Clone();
            }
            else
            {
                EventReminder = GetEventReminderWithoutEvent();
                EventReminder.PropertyChanged += EventReminderOnPropertyChanged;
            }

        }
        #endregion

        #region Methods

        private void ProcessEventReminder(EventReminderModel eventReminderModel)
        {
            IsEditMode = (eventReminderModel != null);
            if (IsEditMode)
            {
                CreatedByUser = eventReminderModel.CreatedByUser;
                AssignedToUser = eventReminderModel.AssignedToUser;
            }
            EventReminder = (IsEditMode) ? eventReminderModel : GetEventReminder();
            EventReminder.PropertyChanged += EventReminderOnPropertyChanged;
        }

        private EventReminderModel GetEventReminder()
        {
            var eventReminderModel = new EventReminderModel(new Data.Model.EventReminder()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                DateDue = DateTime.Now,
                CreatedByUserID = AccessService.Current.User.ID,
                Status = Convert.ToBoolean(Convert.ToInt32(EventManagementSystem.Enums.Events.ReminderStatus.Active))
            });
            return eventReminderModel;
        }
        private EventReminderModel GetEventReminderWithoutEvent()
        {
            var eventReminderModel = new EventReminderModel(new Data.Model.EventReminder()
            {
                ID = Guid.NewGuid(),
                DateDue = DateTime.Now,
                CreatedByUserID = AccessService.Current.User.ID,
                Status = Convert.ToBoolean(Convert.ToInt32(EventManagementSystem.Enums.Events.ReminderStatus.Active))
            });
            return eventReminderModel;
        }

        private void EventReminderOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var users = await _eventDataUnit.UsersRepository.GetUsersAsync();
            Users = new ObservableCollection<User>(users);

            OnLoadCurrentUser();

            if (IsEditMode)
            {
                var desiredEventReminder = await _eventDataUnit.EventRemindersRepository.GetUpdatedEventReminder(_eventReminder.EventReminder.ID);
                // Check if we have new changes
                if (desiredEventReminder != null && desiredEventReminder.LastEditDate != null && _eventReminder.LoadedTime < desiredEventReminder.LastEditDate)
                {
                    EventReminder = new EventReminderModel(desiredEventReminder);
                    AssignedToUser = desiredEventReminder.User;
                }
            }
            IsBusy = false;
        }

        private void OnLoadCurrentUser()
        {
            if (IsEditMode) return;

            var user = Users.FirstOrDefault(x => x.ID == AccessService.Current.User.ID);
            CreatedByUser = user;
        }

        #endregion

        #region Commands

        private void OpenEventCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new BookingView(BookingViews.Event, new EventModel(EventReminder.EventReminder.Event));
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private bool OpenEventCommandCanExecute()
        {
            return IsEditMode;
        }

        private async void SubmitCommandExecuted()
        {
            if (!IsEditMode)
            {
                if (AreEventsVisible)
                {
                    EventReminder.EventReminder.EventID = SelectedEvent.Event.ID;
                    EventReminder.EventReminder.Event = SelectedEvent.Event;
                }
                else
                    _event.EventReminders.Add(EventReminder);
                _eventDataUnit.EventRemindersRepository.Add(EventReminder.EventReminder);

                var primaryContact = EventReminder.EventReminder.Event != null ? EventReminder.EventReminder.Event.Contact == null ? String.Empty : "Primary Contact: " + EventReminder.EventReminder.Event.Contact.FirstName + " "
                    + EventReminder.EventReminder.Event.Contact.LastName : String.Empty;

                var msg = "Event-Reminder" + "\n" + "Created by " + EventReminder.CreatedByUser.FirstName + " " +
                          EventReminder.CreatedByUser.LastName + " at " + DateTime.Now + "\n" +
                          "Event Name: " + EventReminder.EventName + "\n" + primaryContact + "\n" + EventReminder.WhatToDo;
                var email = new CorrespondenceModel(new Corresponcence()
                {
                    ID = Guid.NewGuid(),
                    Date = DateTime.Now,
                    FromAddress = EventReminder.CreatedByUser.EmailAddress,
                    ToAddress = EventReminder.AssignedToUser.EmailAddress,
                    Subject = "Event-Reminder",
                    Message = msg,
                });

                await EmailService.SendEmail(email);
                if (AreEventsVisible)
                {
                    _originalEvent = SelectedEvent.Clone();
                    SelectedEvent.EventReminders.Add(EventReminder);
                    var eventUpdates = LoggingService.FindDifference(_originalEvent, SelectedEvent);
                    if (!SelectedEvent.EventUpdates.Any())
                    {
                        var updates = await _eventDataUnit.EventUpdatesRepository.GetAllAsync(x => x.EventID == SelectedEvent.Event.ID);
                        SelectedEvent.EventUpdates = new ObservableCollection<EventUpdate>(updates.OrderByDescending(x => x.Date));
                    }
                    ProcessUpdates(SelectedEvent, eventUpdates);

                    await _eventDataUnit.SaveChanges();
                }
            }
            else
            {
                _eventReminder.AssignedToUser = AssignedToUser;
                _eventReminder.EventReminder.AssignedToUserID = AssignedToUser.ID;
                EventReminder.EventReminder.LastEditDate = DateTime.Now;
                if (AreEventsVisible)
                {
                    if (_originalEvent.Event.ID != _selectedEvent.Event.ID)
                    {
                        _originalEvent = _selectedEvent.Clone();
                    }

                    EventReminder.EventReminder.Event = SelectedEvent.Event;
                    SelectedEvent.EventReminders.Where(x => x.EventReminder == _eventReminder.EventReminder).FirstOrDefault().AssignedToUser = AssignedToUser;
                    var eventUpdates = LoggingService.FindDifference(_originalEvent, SelectedEvent);
                    if (!SelectedEvent.EventUpdates.Any())
                    {
                        var updates = await _eventDataUnit.EventUpdatesRepository.GetAllAsync(x => x.EventID == SelectedEvent.Event.ID);
                        SelectedEvent.EventUpdates = new ObservableCollection<EventUpdate>(updates.OrderByDescending(x => x.Date));
                    }
                    ProcessUpdates(_selectedEvent, eventUpdates);

                    await _eventDataUnit.SaveChanges();
                }
                else
                    _eventDataUnit.EventRemindersRepository.SetEntityModified(_eventReminder.EventReminder);
                EventReminder.Refresh();
            }

        }

        private void ProcessUpdates(EventModel model, List<EventUpdate> eventUpdates)
        {
            eventUpdates.ForEach(update =>
                {
                    model.EventUpdates.Insert(0, update);
                    _eventDataUnit.EventUpdatesRepository.Add(update);
                });

            model.EventUpdates = new ObservableCollection<EventUpdate>(model.EventUpdates.OrderByDescending(x => x.Date));
        }

        private bool SubmitCommandCanExecute()
        {
            return !EventReminder.HasErrors && (!AreEventsVisible || SelectedEvent != null);
        }

        private void CancelCommandExecuted()
        {
            _eventDataUnit.RevertChanges();
            if (_isEditMode)
            {
                EventReminder.Refresh();
            }
            else
            {
                EventReminder.AssignedToUser = null;
                EventReminder = null;
            }
        }
        #endregion
    }
}
