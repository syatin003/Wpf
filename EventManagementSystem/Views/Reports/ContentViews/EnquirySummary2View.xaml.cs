using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for EnquirySummary2View.xaml
    /// </summary>
    public partial class EnquirySummary2View : UserControl
    {
        private readonly EnquirySummaryViewModel _viewModel;

        public EnquirySummary2View()
        {
            InitializeComponent();
            DataContext = _viewModel = new EnquirySummaryViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            string[] values = new string[2];
            values[0] = "Enquiry Summary 2 Report";
            values[1] = "for enquiries between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");

            PrintService.ExportPivotToPdf(EnquirySummaryRadPivotGrid, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportPivotToExcel(EnquirySummaryRadPivotGrid);
        }
    }
}
