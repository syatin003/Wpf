using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs
{
    /// <summary>
    /// Interaction logic for EventNotesView.xaml
    /// </summary>
    public partial class EventNotesView : UserControl
    {
        private readonly EventNotesViewModel _viewModel;

        public EventNotesView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EventNotesViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            IsVisibleChanged += OnIsVisibleChanged;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            IsVisibleChanged -= OnIsVisibleChanged;

            if ((bool)dependencyPropertyChangedEventArgs.NewValue)
            {
                var eventBookingView = (EventBookingView)this.ParentOfType<UserControl>();
                _viewModel.Event = eventBookingView.ViewModel.Event;
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
