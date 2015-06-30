using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for EnquiryStatusView.xaml
    /// </summary>
    public partial class EnquiryStatusView : UserControl
    {
        private readonly EnquiryStatusViewModel _viewModel;

        public EnquiryStatusView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EnquiryStatusViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            string[] values = new string[2];
            values[0] = "Enquiry Status Report";
            values[1] = "for enquiries between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");
            PrintService.Export(EnquiriesRadGridView, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(EnquiriesRadGridView);
        }
    }
}
