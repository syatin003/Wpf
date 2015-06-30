using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs
{
    /// <summary>
    /// Interaction logic for EventUpdatesView.xaml
    /// </summary>
    public partial class EventUpdatesView : UserControl
    {
        private readonly EventUpdatesViewModel _viewModel;
        private bool _isEventAssigned;

        public EventUpdatesView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EventUpdatesViewModel();

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
    }
}
