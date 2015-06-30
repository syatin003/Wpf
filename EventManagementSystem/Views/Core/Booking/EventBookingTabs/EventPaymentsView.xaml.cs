using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs
{
    /// <summary>
    /// Interaction logic for EventPaymentsView.xaml
    /// </summary>
    public partial class EventPaymentsView : UserControl
    {
        private readonly EventPaymentsViewModel _viewModel;
        private bool _isEventAssigned;

        public EventPaymentsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EventPaymentsViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            IsVisibleChanged += OnIsVisibleChanged;
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

                _viewModel.LoadEventData();
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
            else if (args.PropertyName == "UpdateEventStatus")
            {
                var eventBookingView = (EventBookingView)this.ParentOfType<UserControl>();
                //eventBookingView.ViewModel.EventStatus = _viewModel.Event.EventStatus;
            }
        }
    }
}
