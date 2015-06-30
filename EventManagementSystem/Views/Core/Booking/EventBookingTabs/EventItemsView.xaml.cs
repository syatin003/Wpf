using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs
{
    /// <summary>
    /// Interaction logic for EventItemsView.xaml
    /// </summary>
    public partial class EventItemsView : UserControl
    {
        private readonly EventItemsViewModel _viewModel;

        public EventItemsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EventItemsViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            IsVisibleChanged += OnIsVisibleChanged;

            EventItemsGridView.MouseDoubleClick += EventItemsGridViewOnMouseDoubleClick;
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

        private void EventItemsGridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var originalSender = mouseButtonEventArgs.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    _viewModel.EditItemCommand.Execute(row.Item as EventItemModel);
                }
            }
        }
    }
}
