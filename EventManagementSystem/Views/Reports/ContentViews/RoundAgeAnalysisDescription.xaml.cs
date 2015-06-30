using System.Windows;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for RoundAgeAnalysisDescription.xaml
    /// </summary>
    public partial class RoundAgeAnalysisDescription
    {

        private readonly RoundageAnalysisDescriptionViewModel _viewModel;

        public RoundAgeAnalysisDescription()
        {
            InitializeComponent();
            DataContext = _viewModel = new RoundageAnalysisDescriptionViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.SetDescriptionActivated(false);
            _viewModel.LoadData();
            _viewModel.SetDescriptionActivated(true);

        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            var values = new string[2];
            values[0] = "Roundage Analysis Report";
            values[1] = "for events between " + _viewModel.StartDateOption.ToString("d") + " and " + _viewModel.EndDateOption.ToString("d");

            PrintService.ExportPivotToPdf(RoundAgeAnalysisPivotGrid, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportPivotToExcel(RoundAgeAnalysisPivotGrid);
        }
    }
}
