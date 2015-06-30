using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Events;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Events
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
        }

        private void OnEventsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnEventsViewLoaded;
            _viewModel.LoadData();
        }

        /// <summary>
        ///     Hide RadDropDownButton after user clicks on MenuItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideRadMenuItem_OnClick(object sender, RadRoutedEventArgs e)
        {
            var item = e.Source as RadMenuItem;

            var btn = item.ParentOfType<RadDropDownButton>();
            if (btn != null)
                btn.IsOpen = false;
        }
    }
}
