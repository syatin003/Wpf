using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.EPOS;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.EPOS
{
    /// <summary>
    /// Interaction logic for EPOSView.xaml
    /// </summary>
    public partial class EPOSView : UserControl
    {
        public readonly EPOSViewModel ViewModel;

        public EPOSView()
        {
            InitializeComponent();
            DataContext = ViewModel = new EPOSViewModel();

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnViewLoaded;
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
            else if (args.PropertyName == "SetProductsAsDefault")
            {
                radEposTreeView.SelectedItem = radEposTreeView.Items[0];
            }
        }

        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
            ViewModel.LoadData();
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
