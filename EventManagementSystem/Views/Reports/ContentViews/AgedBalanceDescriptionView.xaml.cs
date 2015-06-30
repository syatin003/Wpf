using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for AgedBalanceDescriptionView.xaml
    /// </summary>
    public partial class AgedBalanceDescriptionView : UserControl
    {
        private readonly AgedBalanceViewModel _viewModel;

        public AgedBalanceDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new AgedBalanceViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            PrintService.Export(AgedBalanceRadGridView, "Aged Balance Report");
        }
        
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(AgedBalanceRadGridView);
        }
    }
}
