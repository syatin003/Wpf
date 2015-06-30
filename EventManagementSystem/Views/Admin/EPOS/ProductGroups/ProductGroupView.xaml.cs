using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.EPOS.ProductDepartments;
using Telerik.Windows;
using Telerik.Windows.Controls;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;


namespace EventManagementSystem.Views.Admin.EPOS.ProductDepartments
{
    /// <summary>
    /// Interaction logic for ProductDepartmentView.xaml
    /// </summary>
    public partial class ProductDepartmentView : UserControl
    {
        private readonly ProductDepartmentViewModel ViewModel;

        public ProductDepartmentView(TillDivisionGroupDepartmentModel tillDivisionGroupDepartmentModel)
        {
            InitializeComponent();
            DataContext = ViewModel = new ProductDepartmentViewModel(tillDivisionGroupDepartmentModel);

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
        }

        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //Loaded -= OnProductsViewLoaded;
            ViewModel.LoadData();
        }
    }
}
