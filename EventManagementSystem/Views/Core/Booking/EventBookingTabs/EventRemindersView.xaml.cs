using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using EventManagementSystem.ViewModels.ViewModels.Core.Booking.EventBookingTabs;
using EventManagementSystem.Views.CRM;
using System.Windows.Input;
using Telerik.Windows.Controls.GridView;
using EventManagementSystem.Models;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs
{
    /// <summary>
    /// Interaction logic for EventRemindersView.xaml
    /// </summary>
    public partial class EventRemindersView : UserControl
    {
        private readonly EventRemindersViewModel _viewModel;
        private bool _isEventAssigned;

        public EventRemindersView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EventRemindersViewModel();

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            IsVisibleChanged += OnIsVisibleChanged;

            gridReminders.MouseDoubleClick += gridReminders_MouseDoubleClick;
        }

        private void gridReminders_MouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var originalSender = mouseButtonEventArgs.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    _viewModel.EditEventReminderCommand.Execute(row.Item as EventReminderModel);
                }
            }
        }
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if ((bool)dependencyPropertyChangedEventArgs.NewValue)
            {
                if (!_isEventAssigned)
                {
                    var eventBookingView = (EventBookingView)this.ParentOfType<UserControl>();
                    _viewModel.Event = eventBookingView.ViewModel.Event;
                    _isEventAssigned = true;
                }
                _viewModel.AddDefaultEventReminderCommand.RaiseCanExecuteChanged();
                //_viewModel.LoadEventData();
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var window = RadWindow.GetParentRadWindow(this);

            if (args.PropertyName == "EnableParentWindow")
            {
                window.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                window.IsEnabled = false;
            }
        }

    }
}
