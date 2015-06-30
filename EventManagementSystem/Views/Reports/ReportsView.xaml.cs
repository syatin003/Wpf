using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Reports;

namespace EventManagementSystem.Views.Reports
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    public partial class ReportsView : UserControl
    {
        private readonly ReportsViewModel _viewModel;

        public ReportsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ReportsViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
            // _viewModel.LoadData();
        }
    }
}
