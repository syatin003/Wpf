using System;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Reminders
{
    public class SnoozeViewModel : ViewModelBase
    {
        #region Fields

        private string _snoozeTime;
        private EventReminderModel _eventReminder;

        #endregion

        #region Properties

        public string SnoozeTime
        {
            get { return _snoozeTime; }
            set
            {
                if (_snoozeTime == value) return;
                _snoozeTime = value;
                RaisePropertyChanged(() => SnoozeTime);
                OKCommand.RaiseCanExecuteChanged();
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

        public RelayCommand OKCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public SnoozeViewModel(EventReminderModel eventReminderModel)
        {
            EventReminder = eventReminderModel;

            OKCommand = new RelayCommand(OKCommandExecute, OKCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecute);
        }

        #endregion

        #region Methods

        #endregion

        #region Commands

        private void OKCommandExecute()
        {

        }

        private bool OKCommandCanExecute()
        {
            return !String.IsNullOrEmpty(SnoozeTime);
        }

        private void CancelCommandExecute()
        {

        }

        #endregion
    }
}
