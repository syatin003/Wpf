using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.EPOS.ProductGroups;
using Telerik.Windows;
using Telerik.Windows.Controls;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;

namespace EventManagementSystem.Views.Admin.EPOS.ProductGroups
{
    /// <summary>
    /// Interaction logic for ProductGroupView.xaml
    /// </summary>
    public partial class ProductGroupView : UserControl
    {
        private readonly ProductGroupViewModel ViewModel;

        public ProductGroupView(TillDivisionGroupDepartmentModel tillDivisionGroupDepartmentModel)
        {
            InitializeComponent();
            DataContext = ViewModel = new ProductGroupViewModel(tillDivisionGroupDepartmentModel);

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
            ViewModel.LoadData();
        }
    }
}
