using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    ///     Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        private readonly ProductsViewModel _viewModel;

        public ProductsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ProductsViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
            _viewModel.LoadData();
        }
    }
}