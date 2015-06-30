using System.Windows;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for JoinersLeaversDescriptionView.xaml
    /// </summary>
    public partial class JoinersLeaversDescriptionView
    {
        private readonly JoinersLeaversDescriptionViewModel _viewModel;

        public JoinersLeaversDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new JoinersLeaversDescriptionViewModel();

            Loaded += OnViewLoaded;
        }
        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            var values = new string[2];
            values[0] = "Joiners Leavers Report";
            values[1] = " between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");

            PrintService.Export(JoinersLeaversRadGridViewGrid, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(JoinersLeaversRadGridViewGrid);
        }
    }
}
