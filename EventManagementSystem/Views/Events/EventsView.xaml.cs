using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.CommonObjects.Appointments;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Events;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Linq;

namespace EventManagementSystem.Views.Events
{
    /// <summary>
    /// Interaction logic for EventsView.xaml
    /// </summary>
    public partial class EventsView : UserControl
    {
        private readonly EventsViewModel _viewModel;

        public EventsView()
        {
            InitializeComponent();

            DataContext = _viewModel = new EventsViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            GridView.MouseDoubleClick += GridViewOnMouseDoubleClick;

            Loaded += OnEventsViewLoaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var tile = this.ParentOfType<RadTileView>();

            if (args.PropertyName == "EnableParentWindow")
            {
                tile.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                tile.IsEnabled = false;
            }
            else if (args.PropertyName == "AddNewReminder")
            {
                RefreshRemindersAfterAdding(_viewModel.CurrentlyAddedReminder);
            }
        }

        private void OnEventsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_viewModel.SelectedTab == 0)
                _viewModel.LoadData();
        }

        private void EditEvent_OnClick(object sender, RoutedEventArgs e)
        {
            var eventModel = ((RadButton)sender).Tag as EventModel;
            _viewModel.EditEventCommand.Execute(eventModel);
        }

        private void GridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    var eventModel = row.Item as EventModel;
                    if (eventModel != null && eventModel.IsActualEvent)
                        _viewModel.EditEventCommand.Execute(row.Item as EventModel);
                }
            }
        }

        private void CreateEvent_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["SelectedEventStart"] = RadScheduleView.SelectedSlot.Start;
            _viewModel.AddEventCommand.Execute(null);

            ((RadButton)sender).ParentOfType<SchedulerWindow>().Close();
        }

        private void ExpandCollapseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var expandCollapseButton = (RadButton)sender;

            var selectedRow = expandCollapseButton.ParentOfType<GridViewRow>();

            if (null != expandCollapseButton && "+" == expandCollapseButton.Content.ToString())
            {
                selectedRow.DetailsVisibility = Visibility.Visible;
                expandCollapseButton.Content = "-";
            }

            else
            {
                selectedRow.DetailsVisibility = Visibility.Collapsed;
                if (expandCollapseButton != null) expandCollapseButton.Content = "+";
            }
        }

        private void GridView_OnRowDetailsVisibilityChanged(object sender, GridViewRowDetailsEventArgs e)
        {
            var eventModel = (EventModel)e.DetailsElement.DataContext;
            _viewModel.LoadLightEventDetails(eventModel);
        }

        private async void RadScheduleView_OnAppointmentEditing(object sender, AppointmentEditingEventArgs e)
        {
            if (e.Appointment is EventAppointment)
            {
                var appointment = e.Appointment as EventAppointment;

                var model = appointment.Event;
                await _viewModel.LoadLightEventDetails(model);
                model.RefreshResourceBookingsList();
            }

            if (e.Appointment is CalendarNoteAppointment)
            {
                _cancelAppointmentEditing = true;

                var appointment = e.Appointment as CalendarNoteAppointment;
                _viewModel.EditCalendarNoteCommand.Execute(appointment.CalendarNote);
            }
        }

        private bool _cancelAppointmentEditing;

        private void RadScheduleView_OnShowDialog(object sender, ShowDialogEventArgs e)
        {
            if (e.DialogViewModel is AppointmentDialogViewModel && _cancelAppointmentEditing)
            {
                e.Cancel = true;
                _cancelAppointmentEditing = false; // Set true after editing Calencar Note to prevent showing Edit Appointment window
            }
        }

        private void RadScheduleView_AppointmentDeleted(object sender, AppointmentDeletedEventArgs e)
        {
            if (e.Appointment is EventAppointment)
            {
                var appointment = e.Appointment as EventAppointment;
                var model = appointment.Event;
                _viewModel.DeleteEventNoteCommand.Execute(model);
                //_viewModel.LoadLightEventDetails(model);
                //model.RefreshResourceBookingsList();

            }
            if (e.Appointment is CalendarNoteAppointment)
            {
                var appointment = e.Appointment as CalendarNoteAppointment;
                _viewModel.DeleteCalendarNoteCommand.Execute(appointment.CalendarNote);
            }
        }

        public void RemoveReminder(EventReminderModel eventReminder)
        {
            var radTabControl = RootGrid.Children[0] as RadTabControl;

            if (radTabControl != null)
            {
                var tabItem = radTabControl.Items[3] as RadTabItem;

                if (tabItem != null)
                {
                    var remindersView = tabItem.Content as RemindersView;
                    if (remindersView != null)
                    {
                        var remindersViewModel = remindersView.DataContext as RemindersViewModel;
                        if (remindersViewModel != null && remindersViewModel.EventReminders != null)
                        {
                            remindersViewModel._allEventReminders.RemoveAll(eReminder => eReminder.EventReminder.ID == eventReminder.EventReminder.ID);
                            remindersViewModel.EventReminders = new System.Collections.ObjectModel.ObservableCollection<EventReminderModel>(remindersViewModel._allEventReminders);
                        }
                    }
                }
            }
        }
        public void RefreshReminder(EventReminderModel eventReminder)
        {
            var radTabControl = RootGrid.Children[0] as RadTabControl;

            if (radTabControl != null)
            {
                var tabItem = radTabControl.Items[3] as RadTabItem;

                if (tabItem != null)
                {
                    var remindersView = tabItem.Content as RemindersView;
                    if (remindersView != null)
                    {
                        var remindersViewModel = remindersView.DataContext as RemindersViewModel;
                        if (remindersViewModel != null && remindersViewModel.EventReminders != null)
                        {
                            var reminder = remindersViewModel.EventReminders.FirstOrDefault(eReminder => eReminder.EventReminder.ID == eventReminder.EventReminder.ID);
                            if (reminder != null)
                            {
                                reminder.Refresh();
                            }
                        }
                    }
                }
            }
        }
        public void RefreshRemindersAfterAdding(EventReminderModel eventReminder)
        {
            var radTabControl = RootGrid.Children[0] as RadTabControl;

            var tabItem = radTabControl.Items[3] as RadTabItem;

            if (tabItem != null)
            {
                var remindersView = tabItem.Content as RemindersView;
                if (remindersView != null)
                {
                    var remindersViewModel = remindersView.DataContext as RemindersViewModel;
                    if (remindersViewModel != null && remindersViewModel.EventReminders != null)
                    {
                        remindersViewModel._allEventReminders.Add(eventReminder);
                        remindersViewModel.EventReminders = new System.Collections.ObjectModel.ObservableCollection<EventReminderModel>(remindersViewModel._allEventReminders.OrderBy(reminder => reminder.EventReminder.Status).ThenBy(reminder => reminder.DateDue));
                    }
                }
            }
        }

        private void DropDownOpen(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource != RadDropDown)
            {
                RadDropDown.IsOpen = true;
            }

        }
    }
}
