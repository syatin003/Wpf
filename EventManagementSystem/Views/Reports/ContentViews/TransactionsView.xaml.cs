using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    ///     Interaction logic for TransactionsView.xaml
    /// </summary>
    public partial class TransactionsView : UserControl
    {
        private readonly TransactionsViewModel _viewModel;

        public TransactionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new TransactionsViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
    }
}