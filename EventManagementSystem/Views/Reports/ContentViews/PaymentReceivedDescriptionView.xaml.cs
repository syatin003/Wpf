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
    /// Interaction logic for PaymentDescriptionView.xaml
    /// </summary>
    public partial class PaymentReceivedDescriptionView : UserControl
    {
        private readonly PaymentReceivedViewModel _viewModel;
        public PaymentReceivedDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new PaymentReceivedViewModel();

            Loaded += OnViewLoaded;
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadData();
        }
        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            string[] values = new string[3];
            values[0] = "Payments Received Report";
            values[1] = "for events between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");
            values[2] = _viewModel.IsReceiveDateChecked ? "Received Date" : "Event Date";

            PrintService.Export(PaymentsReceivedRadGridView, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(PaymentsReceivedRadGridView);
        }
    }
}
