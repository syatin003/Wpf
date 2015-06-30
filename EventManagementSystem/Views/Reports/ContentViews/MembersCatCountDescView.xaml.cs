using System.Windows;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for MembersCatCountDescView.xaml
    /// </summary>
    public partial class MembersCatCountDescView
    {
        private readonly MembersCatCountDescViewModel _viewModel;

        public MembersCatCountDescView()
        {
            InitializeComponent();
            DataContext = _viewModel = new MembersCatCountDescViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            var values = new string[1];
            values[0] = "Members Category Count Report";

            PrintService.ExportPivotToPdf(MembersPivotGrid, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportPivotToExcel(MembersPivotGrid);
        }
    }
}
