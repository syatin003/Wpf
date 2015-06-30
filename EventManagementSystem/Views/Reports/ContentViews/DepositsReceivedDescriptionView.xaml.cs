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

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for DepositsReceivedDescriptionView.xaml
    /// </summary>
    public partial class DepositsReceivedDescriptionView : UserControl
    {
        private readonly DepositsReceivedViewModel _viewModel;

        public DepositsReceivedDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new DepositsReceivedViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            string[] values = new string[3];
            values[0] = "Deposits Received Report";
            values[1] = "for events between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");
            //values[2] = "End Date: " + _viewModel.EndDate.ToString("d");
            values[2] = _viewModel.IsChecked ? "Received Date" : "Event Date";

            PrintService.Export(DepositsRecievedRadGridView, values);
        }
        
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(DepositsRecievedRadGridView);
        }
    }
}
