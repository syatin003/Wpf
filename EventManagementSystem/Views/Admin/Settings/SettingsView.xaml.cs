using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Settings;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new SettingsViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnSettingsViewLoaded;
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

        private void OnSettingsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
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
