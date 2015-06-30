using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Resources;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Resources
{
    /// <summary>
    ///     Interaction logic for ResourcesView.xaml
    /// </summary>
    public partial class ResourcesView : UserControl
    {
        private readonly ResourcesViewModel _viewModel;

        public ResourcesView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ResourcesViewModel(); ;

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnResourcesViewLoaded;
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

        private void OnResourcesViewLoaded(object sender, RoutedEventArgs routedEventArgs)
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