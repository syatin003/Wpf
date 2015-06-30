using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for CustomersDescriptionView.xaml
    /// </summary>
    public partial class CustomersDescriptionView : UserControl
    {
        private readonly CustomersViewModel _viewModel;

        public CustomersDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new CustomersViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            PrintService.Export(CustomersRadGridView, "Customers Report");
        }
        
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(CustomersRadGridView);
        }
    }
}
