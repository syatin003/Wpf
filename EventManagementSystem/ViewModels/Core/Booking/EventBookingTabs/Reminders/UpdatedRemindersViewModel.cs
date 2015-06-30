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

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Reminders
{
    public class UpdatedRemindersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private bool _isBusy;
        private ObservableCollection<EventReminderModel> _eventRemindersToBeUpdated;

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

        public ObservableCollection<EventReminderModel> EventRemindersToBeUpdated
        {
            get { return _eventRemindersToBeUpdated; }
            set
            {
                if (_eventRemindersToBeUpdated == value) return;
                _eventRemindersToBeUpdated = value;
                RaisePropertyChanged(() => EventRemindersToBeUpdated);
            }
        }

        public RelayCommand UpdateRemindersCommand { get; private set; }

        #endregion

        #region Constructor

        public UpdatedRemindersViewModel(ObservableCollection<EventReminderModel> eventRemindersToBeUpdated)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
            EventRemindersToBeUpdated = eventRemindersToBeUpdated;
            UpdateRemindersCommand = new RelayCommand(UpdateRemindersCommandExecuted);

        }

        #endregion


        #region Commands

        private void UpdateRemindersCommandExecuted()
        {
            EventRemindersToBeUpdated.ToList().ForEach(eventReminder =>
                                {
                                    eventReminder.DateDue = eventReminder.NewDateDue;
                                });
        }


        #endregion
    }
}
