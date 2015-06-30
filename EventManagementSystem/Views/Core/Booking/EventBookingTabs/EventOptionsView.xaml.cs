using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs
{
    /// <summary>
    /// Interaction logic for EventOptionsView.xaml
    /// </summary>
    public partial class EventOptionsView : UserControl
    {
        private readonly EventOptionsViewModel _viewModel;

        public EventOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EventOptionsViewModel();

            IsVisibleChanged += OnIsVisibleChanged;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            IsVisibleChanged -= OnIsVisibleChanged;

            if ((bool) dependencyPropertyChangedEventArgs.NewValue)
            {
                var eventBookingView = (EventBookingView)this.ParentOfType<UserControl>();
                _viewModel.Event = eventBookingView.ViewModel.Event;
            }
        }
    }
}
