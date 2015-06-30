using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;
using System.Windows;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for LeaversDescriptionView.xaml
    /// </summary>
    public partial class LeaversDescriptionView
    {
        private readonly LeaversDescriptionViewModel _viewModel;

        public LeaversDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new LeaversDescriptionViewModel();

            Loaded += OnViewLoaded;
        }
        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            var values = new string[2];
            values[0] = "Leavers Report";
            values[1] = " between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");

            PrintService.Export(LeaversRadGridViewGrid, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(LeaversRadGridViewGrid);
        }
    }
}
