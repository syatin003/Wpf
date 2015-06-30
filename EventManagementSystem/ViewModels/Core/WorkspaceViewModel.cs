using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using EventManagementSystem.Views;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp;
using Microsoft.Practices.Unity;
using Tile = EventManagementSystem.CommonObjects.Tile;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders;

namespace EventManagementSystem.ViewModels.Core
{
    public class WorkspaceViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Tile> _tiles;
        private Timer _timer;
        private readonly int _logoutTime;
        private readonly IWorkspaceDataUnit _workspaceDataUnit;

        private DispatcherTimer _updateTimer;
        private DispatcherTimer _updateTimerEvents;
        private readonly ICrmDataUnit _crmDataUnit;
        private readonly IEventDataUnit _eventDataUnit;

        #endregion

        #region Properties

        public ObservableCollection<Tile> Tiles
        {
            get { return _tiles; }
            private set
            {
                if (_tiles == value) return;
                _tiles = value;
                RaisePropertyChanged(() => Tiles);
            }
        }

        #endregion

        #region Constructor

        public WorkspaceViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _workspaceDataUnit = dataUnitLocator.ResolveDataUnit<IWorkspaceDataUnit>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            var time = (int?)ApplicationSettings.Read("LogoutTime");
            _logoutTime = (time.HasValue && time.Value > 0) ? time.Value : 30; // 30 minutes - default logout time

            EventManager.RegisterClassHandler(typeof(Window), UIElement.KeyDownEvent, new RoutedEventHandler(Window_KeyDown));
            EventManager.RegisterClassHandler(typeof(Window), UIElement.MouseDownEvent, new RoutedEventHandler(Window_MouseDown));
            EventManager.RegisterClassHandler(typeof(Window), UIElement.MouseMoveEvent, new RoutedEventHandler(Window_MouseMove));
            EventManager.RegisterClassHandler(typeof(Window), UIElement.MouseWheelEvent, new RoutedEventHandler(Window_MouseWheel));

            _timer = new Timer(LogoutByInactivity, null, 1000 * 60 * _logoutTime, Timeout.Infinite);

            _updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
            _updateTimer.Tick += UpdateTimer_Tick;

            _updateTimer.Start();

            _updateTimerEvents = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
            _updateTimerEvents.Tick += _updateTimerEvents_Tick;

            _updateTimerEvents.Start();
        }



        #endregion

        #region Methods

        private async void _updateTimerEvents_Tick(object sender, EventArgs e)
        {

            if (_updateTimerEvents != null)
                _updateTimerEvents.Stop();

            try
            {
                _eventDataUnit.EventRemindersRepository.Refresh();
                var activeStatus = Convert.ToBoolean(EventManagementSystem.Enums.Events.ReminderStatus.Active);
                var reminders = (await _eventDataUnit.EventRemindersRepository.GetAllAsync(
                            x => x.AssignedToUserID == AccessService.Current.User.ID && x.Status == activeStatus)).OrderBy(x => x.DateDue);

                var lastReminders =
                    reminders.Where(
                        x => x.DateDue < DateTime.Now && x.DateDue.ToString("g") != DateTime.Now.ToString("g"));

                foreach (var lastReminder in lastReminders)
                {
                    RaisePropertyChanged("DisableParentWindow");
                    var popup = new ReminderPopUpView(new EventReminderModel(lastReminder));
                    popup.ShowDialog();
                    RaisePropertyChanged("EnableParentWindow");
                }

                var currentReminders = reminders.Where(x => x.DateDue.ToString("g") == DateTime.Now.ToString("g"));

                foreach (var currentReminder in currentReminders)
                {
                    RaisePropertyChanged("DisableParentWindow");
                    var popup = new ReminderPopUpView(new EventReminderModel(currentReminder));
                    popup.ShowDialog();
                    RaisePropertyChanged("EnableParentWindow");

                }

            }
            catch (TimeoutException)
            {

            }
            catch (EntityCommandExecutionException)
            {

            }
            catch (Exception)
            {

            }
            if (_updateTimerEvents != null)
                _updateTimerEvents.Start();
        }


        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (_updateTimer != null)
                _updateTimer.Stop();

            try
            {
                _crmDataUnit.FollowUpsRepository.Refresh();

                var followUps = (await _crmDataUnit.FollowUpsRepository.GetAllAsync(
                            x => x.UserDueToDoID == AccessService.Current.User.ID)).OrderBy(x => x.DateDue);

                var lastFollowUps =
                    followUps.Where(
                        x => x.DateDue < DateTime.Now && x.DateDue.ToString("g") != DateTime.Now.ToString("g"));

                foreach (var lastFollowUp in lastFollowUps)
                {
                    RaisePropertyChanged("DisableParentWindow");
                    var popup = new FollowUpPopUp(new FollowUpModel(lastFollowUp));
                    popup.ShowDialog();
                    RaisePropertyChanged("EnableParentWindow");
                }

                var currentFollowUps = followUps.Where(x => x.DateDue.ToString("g") == DateTime.Now.ToString("g"));

                foreach (var currentFollowUp in currentFollowUps)
                {
                    RaisePropertyChanged("DisableParentWindow");
                    var popup = new FollowUpPopUp(new FollowUpModel(currentFollowUp));
                    popup.ShowDialog();
                    RaisePropertyChanged("EnableParentWindow");

                }

            }
            catch (TimeoutException)
            {

            }
            catch (EntityCommandExecutionException)
            {

            }
            catch (Exception)
            {

            }
            if (_updateTimer != null)
                _updateTimer.Start();
        }

        private void Window_KeyDown(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_timer != null)
            {
                // postpone auto-logout by 30 minutes
                _timer.Change(1000 * 60 * _logoutTime, Timeout.Infinite);
            }
        }

        private void Window_MouseDown(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_timer != null)
            {
                // postpone auto-logout by 30 minutes
                _timer.Change(1000 * 60 * _logoutTime, Timeout.Infinite);
            }
        }

        private void Window_MouseMove(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_timer != null)
            {
                // postpone auto-logout by 30 minutes
                _timer.Change(1000 * 60 * _logoutTime, Timeout.Infinite);
            }
        }

        private void Window_MouseWheel(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_timer != null)
            {
                // postpone auto-logout by 30 minutes
                _timer.Change(1000 * 60 * _logoutTime, Timeout.Infinite);
            }
        }

        private async void LogoutByInactivity(object state)
        {
            _timer = null;
            _updateTimer = null;

            // Close all event editing session
            var openedEvents = await _workspaceDataUnit.EventsRepository.GetLightEventsAsync(x => x.LockedUserID == AccessService.Current.User.ID);
            openedEvents.ForEach(x => x.LockedUserID = null);
            await _workspaceDataUnit.SaveChanges();

            // Close all opened windows
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var allWindows = Application.Current.Windows.OfType<Window>().ToList();

                foreach (var window in allWindows)
                {
                    if (window != Application.Current.MainWindow)
                        window.Close();
                }

                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ViewModel.Logout();
            }));
        }

        public void OnLoadData()
        {
            var resources = (ResourceDictionary)Application.LoadComponent(new Uri("Resources/Images.xaml", UriKind.Relative));

            Tiles = new ObservableCollection<Tile>();

            if (AccessService.Current.UserHasPermissions(Resources.PERMISSION_EVENTS_ALLOWED))
                Tiles.Add(new Tile()
                {
                    Name = "Events",
                    Content = new Views.Events.EventsView(),
                    Image = (ImageSource)resources["ImageEvents48"]
                });

            if (AccessService.Current.UserHasPermissions(Resources.PERMISSION_MAIN_ALLOWED))
                Tiles.Add(new Tile()
                {
                    Name = "Admin",
                    Content = new Views.Admin.MainView(),
                    Image = (ImageSource)resources["ImageAdmins48"]
                });

            if (AccessService.Current.UserHasPermissions(Resources.PERMISSION_REPORTS_ALLOWED))
                Tiles.Add(new Tile()
                {
                    Name = "Reports",
                    Content = new Views.Reports.ReportsView(),
                    Image = (ImageSource)resources["ImageReports48"]
                });

            if (AccessService.Current.UserHasPermissions(Resources.PERMISSION_CONTACTS_ALLOWED))
                Tiles.Add(new Tile()
                {
                    Name = "Contacts",
                    Content = new Views.ContactManager.ContactManagerView(),
                    Image = (ImageSource)resources["ImageContacts48"]
                });

            if (AccessService.Current.UserHasPermissions(Resources.PERMISSION_CRM_ALLOWED))
                Tiles.Add(new Tile()
                {
                    Name = "CRM",
                    Content = new Views.CRM.CRMView(),
                    Image = (ImageSource)resources["ImageCrm48"]
                });

            if (AccessService.Current.UserHasPermissions(Resources.PERMISSION__MEMBERSHIP_ALLOWED))
                Tiles.Add(new Tile()
                {
                    Name = "Membership",
                    Content = new Views.Membership.MembershipView(),
                    Image = (ImageSource)resources["ImageMembers48"]
                });
        }

        #endregion
    }
}
