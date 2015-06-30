using System.Windows;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for JoinersActivityDescriptionView.xaml
    /// </summary>
    public partial class JoinersActivityDescriptionView
    {
        private readonly JoinersActivityDescriptionViewModel _viewModel;

        public JoinersActivityDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new JoinersActivityDescriptionViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            var values = new string[2];
            values[0] = "Joiners Activity Report";
            values[1] = " between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");

            PrintService.Export(JoinersActivityRadGridViewGrid, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(JoinersActivityRadGridViewGrid);
        }
    }
}
