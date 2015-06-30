using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for SelectorValuesView.xaml
    /// </summary>
    public partial class ReceivedMethodView : UserControl
    {
        private readonly ReceivedMethodViewModel _viewModel;

        public ReceivedMethodView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ReceivedMethodViewModel();

            Loaded += OnReceivedMethodViewLoaded;        
        }

        private void OnReceivedMethodViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnReceivedMethodViewLoaded;
            _viewModel.LoadData();
        }
    }
}
