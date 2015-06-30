using System.Collections.Generic;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Services;
using EventManagementSystem.Properties;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders;
using System.Collections.ObjectModel;
using System;
using Telerik.Windows.Controls;

namespace EventManagementSystem.ViewModels.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventRemindersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private bool _isBusy;
        private EventModel _event;
        private EventReminderModel _originalEventReminder;
        private String _noReminderMessage;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public EventModel Event
        {
            get { return _event; }
            set
            {
                if (_event == value) return;
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }
        public String NoReminderMessage
        {
            get { return _noReminderMessage; }
            set
            {
                if (_noReminderMessage == value) return;
                _noReminderMessage = value;
                RaisePropertyChanged(() => NoReminderMessage);
            }
        }

        public bool CanDeleteEventReminder { get; private set; }
        public bool CanEditEveryoneEventReminders { get; private set; }
        public bool CanEditOwnEventReminder { get; private set; }

        public RelayCommand AddEventReminderCommand { get; private set; }
        public RelayCommand<EventReminderModel> DeleteEventReminderCommand { get; private set; }
        public RelayCommand<EventReminderModel> EditEventReminderCommand { get; private set; }

        public RelayCommand AddDefaultEventReminderCommand { get; private set; }

        public static List<FollowUpStatus> FollowUpStatuses { get; set; }

        #endregion

        #region Constructor

        public EventRemindersViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            CanDeleteEventReminder = AccessService.Current.UserHasPermissions(Resources.PERMISSION_DELETE_FOLLOWUP_ALLOWED);
            CanEditEveryoneEventReminders = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_FOLLOWUP_ALLOWED);
            CanEditOwnEventReminder = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_FOLLOWUP_ALLOWED);


            AddEventReminderCommand = new RelayCommand(AddEventReminderCommandExecuted);
            DeleteEventReminderCommand = new RelayCommand<EventReminderModel>(DeleteEventReminderCommandExecuted);
            EditEventReminderCommand = new RelayCommand<EventReminderModel>(EditEventReminderCommandExecuted);

            AddDefaultEventReminderCommand = new RelayCommand(AddDefaultEventReminderCommandExecuted, AddDefaultEventReminderCommandCanExecute);

        }

        #endregion

        #region Methods

        public async void LoadEventData()
        {
            IsBusy = true;

            if (!Event.EventReminders.Any())
            {
                var reminders = await _eventDataUnit.EventRemindersRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                Event.EventReminders = new ObservableCollection<EventReminderModel>(reminders.Select(x => new EventReminderModel(x)).OrderBy(x => x.EventReminder.Status).ThenByDescending(x => x.DateDue));
            }
            Event.PropertyChanged += Event_PropertyChanged;
            NoReminderMessage = Properties.Resources.MESSAGE_NO_REMINDER_AVAILABLE;
            IsBusy = false;
        }

        private void Event_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AddDefaultEventReminderCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void AddEventReminderCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddEventReminderView(Event);
            window.ShowDialog();
            RaisePropertyChanged("EnableParentWindow");
        }

        private void EditEventReminderCommandExecuted(EventReminderModel eventReminderModel)
        {
            RaisePropertyChanged("DisableParentWindow");
            _originalEventReminder = eventReminderModel.Clone();
            var window = new AddEventReminderView(Event, eventReminderModel);
            window.ShowDialog();
            if (window.DialogResult != null && !window.DialogResult.Value)
            {
                eventReminderModel.EventReminder.DateDue = _originalEventReminder.EventReminder.DateDue;
                eventReminderModel.EventReminder.WhatToDo = _originalEventReminder.EventReminder.WhatToDo;
                eventReminderModel.EventReminder.AssignedToUserID = _originalEventReminder.EventReminder.AssignedToUserID;
                eventReminderModel.Refresh();
            }
            RaisePropertyChanged("EnableParentWindow");
        }

        private void DeleteEventReminderCommandExecuted(EventReminderModel eventReminderModel)
        {

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(new DialogParameters()
            {
                Content = confirmText,
                Closed = (sender, args) => { dialogResult = args.DialogResult; }
            });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            Event.EventReminders.Remove(eventReminderModel);
            _eventDataUnit.EventRemindersRepository.Delete(eventReminderModel.EventReminder);
        }
        private bool AddDefaultEventReminderCommandCanExecute()
        {
            if (Event == null)
                return false;
            return Event.Date != default(DateTime) && Event.EventType != null;
        }

        private void AddDefaultEventReminderCommandExecuted()
        {
            if (Event.EventType.EventTypeTODOs.Any())
            {
                Event.EventType.EventTypeTODOs.ToList().ForEach(async eventTypeTODO =>
                    {
                        var eventReminder = new EventReminderModel(new Data.Model.EventReminder()
                        {
                            ID = Guid.NewGuid(),
                            EventID = Event.Event.ID,
                            DateDue = eventTypeTODO.RelatedDateType == Convert.ToInt32(EventManagementSystem.Enums.Admin.RelatedDateType.EventDate) ? Event.Date.AddDays(eventTypeTODO.NumberOfDays) : Event.Event.CreationDate.AddDays(eventTypeTODO.NumberOfDays),
                            CreatedByUserID = AccessService.Current.User.ID,
                            WhatToDo = eventTypeTODO.WhatToDo,
                            Status = Convert.ToBoolean(Convert.ToInt32(EventManagementSystem.Enums.Events.ReminderStatus.Active)),
                            AssignedToUserID = eventTypeTODO.AssignedToUserID,
                            User = eventTypeTODO.User1,
                            EventTypeToDoID = eventTypeTODO.ID
                        });

                        Event.EventReminders.Add(eventReminder);
                        _eventDataUnit.EventRemindersRepository.Add(eventReminder.EventReminder);

                        var primaryContact = eventReminder.EventReminder.Event != null ? eventReminder.EventReminder.Event.Contact == null ? String.Empty : "Primary Contact: " + eventReminder.EventReminder.Event.Contact.FirstName + " "
                  + eventReminder.EventReminder.Event.Contact.LastName : String.Empty;

                        var msg = "Default Event Reminder" + "\n" + "Created by " + eventReminder.CreatedByUser.FirstName + " " +
                                  eventReminder.CreatedByUser.LastName + " at " + DateTime.Now + "\n" +
                                  "Event Name: " + eventReminder.EventName + "\n" + primaryContact + "\n" + eventReminder.WhatToDo;
                        var email = new CorrespondenceModel(new Corresponcence()
                        {
                            ID = Guid.NewGuid(),
                            Date = DateTime.Now,
                            FromAddress = eventReminder.CreatedByUser.EmailAddress,
                            ToAddress = eventReminder.AssignedToUser.EmailAddress,
                            Subject = "Default Event Reminder",
                            Message = msg,
                        });

                        await EmailService.SendEmail(email);
                    });
            }
        }

        #endregion
    }
}
