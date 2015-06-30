using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for ActivityView.xaml
    /// </summary>
    public partial class ActivityView : UserControl
    {
        private readonly ActivityViewModel _viewModel;

        public ActivityView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ActivityViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            string[] values = new string[2];
            values[0] = "Activity Report";
            values[1] = "for activities between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");
            PrintService.Export(ActivitiesRadGridView, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(ActivitiesRadGridView);
        }
    }
}
