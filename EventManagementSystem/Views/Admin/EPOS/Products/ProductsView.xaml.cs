using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.EPOS.Products;
using Telerik.Windows;
using Telerik.Windows.Controls;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Views.Admin.EPOS.Products
{
    /// <summary>
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        private readonly ProductsViewModel _viewModel;

        public ProductsView(ProductType productType)
        {
            InitializeComponent();
            DataContext = _viewModel = new ProductsViewModel(productType);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnProductsViewLoaded;
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
            else if (args.PropertyName == "ScrollToSelectedItem")
            {
                if (RadGridViewProducts.SelectedItem != null)
                    RadGridViewProducts.ScrollIntoView(RadGridViewProducts.SelectedItem);
            }
        }

        private void OnProductsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
    }
}
