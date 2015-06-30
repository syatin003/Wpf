using System;
using System.Collections.ObjectModel;
using System.Windows;
using EventManagementSystem.CommonObjects;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Core;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System.Linq;
using EventManagementSystem.Views.Core.Booking;
using EventManagementSystem.Views.Events;
using EventManagementSystem.ViewModels.Events;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Reminders
{
    public class ReminderPopUpViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private bool _isBusy;
        private EventReminderModel _eventReminder;
        private ObservableCollection<User> _users;

        #endregion

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


        public EventModel SelectedEvent
        {
            get { return new EventModel(EventReminder.EventReminder.Event); }
        }

        public bool AreEnquiriesVisible
        {
            get { return EventReminder.EventReminder.Event != null; }
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

        //  public CorresponcenceType CorresponcenceType { get; set; }

        public RelayCommand SnoozeCommand { get; private set; }
        public RelayCommand CompleteEventReminderCommand { get; private set; }
        public RelayCommand DeleteEventReminderCommand { get; private set; }

        public RelayCommand OpenEventCommand { get; private set; }

        #endregion

        #region Constructor

        public ReminderPopUpViewModel(EventReminderModel eventReminderModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            EventReminder = eventReminderModel;

            SnoozeCommand = new RelayCommand(SnoozeCommandExecute);
            OpenEventCommand = new RelayCommand(OpenEventCommandExecuted, OpenEventCommandCanExecute);
            CompleteEventReminderCommand = new RelayCommand(CompleteEventReminderCommandExecuted);
            DeleteEventReminderCommand = new RelayCommand(DeleteEventReminderCommandExecute);
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            IsBusy = true;

            RaisePropertyChanged(() => EventReminder);

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void OpenEventCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new BookingView(BookingViews.Event, new EventModel(EventReminder.EventReminder.Event));
            window.ShowDialog();

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                var IsCurrentEventReminderDeleted = true;
                var eventBookingView = window.ViewModel.Content as EventBookingView;
                var eventBookingViewModel = eventBookingView.ViewModel as EventBookingViewModel;
                if (eventBookingViewModel.Event.EventReminders.Where(eventReminder => eventReminder.EventReminder.ID == _eventReminder.EventReminder.ID).Count() > 0)
                    IsCurrentEventReminderDeleted = false;

                if (IsCurrentEventReminderDeleted)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //var currentPopUp = Application.Current.MainWindow;
                        //var viewModel = currentPopUp.DataContext as MainWindowModel;
                        //var workspaceView = viewModel.WindowContent as WorkspaceView;
                        //var tile = workspaceView.RootTileView.MaximizedItem as Tile;
                        //if (tile.Name == "CRM")
                        //{
                        //    var crmview = tile.Content as CRMView;
                        //    var crmvm = crmview.DataContext as CRMViewModel;

                        //    if (isToDo)
                        //        crmvm.ReloadFollowUps();
                        //    else
                        //    {
                        //        crmvm.ReloadFollowUpsAndEnquiries();
                        //    }
                        //}
                    }));
                    RaisePropertyChanged("CloseParentWindow");
                }
                else
                {
                    RaisePropertyChanged("EnableParentWindow");
                }
            }
            else
            {
                RaisePropertyChanged("EnableParentWindow");
            }
        }

        private bool OpenEventCommandCanExecute()
        {
            return true;
        }

        private void SnoozeCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");
            var eventID = EventReminder.EventReminder.EventID;
            var snoozeWindow = new SnoozeView(EventReminder);
            snoozeWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            snoozeWindow.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (snoozeWindow.DialogResult == null || snoozeWindow.DialogResult != true) return;

            var viewModel = snoozeWindow.DataContext as SnoozeViewModel;
            switch (viewModel.SnoozeTime)
            {
                case "5 mins":
                    EventReminder.DateDue = DateTime.Now + TimeSpan.FromMinutes(5);
                    break;
                case "10 mins":
                    EventReminder.DateDue = DateTime.Now + TimeSpan.FromMinutes(10);
                    break;
                case "30 mins":
                    EventReminder.DateDue = DateTime.Now + TimeSpan.FromMinutes(30);
                    break;
                case "1 Hour":
                    EventReminder.DateDue = DateTime.Now + TimeSpan.FromHours(1);
                    break;
                case "2 Hours":
                    EventReminder.DateDue = DateTime.Now + TimeSpan.FromHours(2);
                    break;
                case "1 Day":
                    EventReminder.DateDue = DateTime.Now + TimeSpan.FromDays(1);
                    break;
                case "7 Days":
                    EventReminder.DateDue = DateTime.Now + TimeSpan.FromDays(7);
                    break;
            }

            _eventDataUnit.SaveChanges();

            var window = Application.Current.MainWindow;
            var mainViewModel = window.DataContext as MainWindowModel;
            var workspaceView = mainViewModel.WindowContent as WorkspaceView;
            var tile = workspaceView.RootTileView.MaximizedItem as Tile;
            if (tile.Name == "Events")
            {
                var eventsView = tile.Content as EventsView;
                if (eventsView != null)
                {
                    var eventsViewModel = eventsView.DataContext as EventsViewModel;
                    eventsViewModel.RefreshReminders(EventReminder, eventID);
                    eventsView.RefreshReminder(EventReminder);
                }

            }
        }

        private async void DeleteEventReminderCommandExecute()
        {
            var eventID = _eventReminder.EventReminder.EventID;
            _eventDataUnit.EventRemindersRepository.Delete(_eventReminder.EventReminder);
            var result = await _eventDataUnit.SaveChanges().ConfigureAwait(false);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var window = Application.Current.MainWindow;
                var viewModel = window.DataContext as MainWindowModel;
                var workspaceView = viewModel.WindowContent as WorkspaceView;
                var tile = workspaceView.RootTileView.MaximizedItem as Tile;
                if (tile.Name == "Events")
                {
                    var eventsView = tile.Content as EventsView;
                    if (eventsView != null)
                    {
                        var eventsViewModel = eventsView.DataContext as EventsViewModel;
                        eventsViewModel.RemoveReminders(EventReminder, eventID);
                        eventsView.RemoveReminder(EventReminder);
                    }

                }
            }));
        }

        private void CompleteEventReminderCommandExecuted()
        {
            var eventID = EventReminder.EventReminder.EventID;
            EventReminder.EventReminder.Status = Convert.ToBoolean(Convert.ToInt32(EventManagementSystem.Enums.Events.ReminderStatus.Complete));
            _eventDataUnit.SaveChanges();
            var window = Application.Current.MainWindow;
            var mainViewModel = window.DataContext as MainWindowModel;
            var workspaceView = mainViewModel.WindowContent as WorkspaceView;
            var tile = workspaceView.RootTileView.MaximizedItem as Tile;
            if (tile.Name == "Events")
            {
                var eventsView = tile.Content as EventsView;
                if (eventsView != null)
                {
                    var eventsViewModel = eventsView.DataContext as EventsViewModel;
                    eventsViewModel.RefreshReminders(EventReminder, eventID);
                    eventsView.RefreshReminder(EventReminder);
                }

            }
            RaisePropertyChanged("CloseParentWindow");
        }

        #endregion
    }
}