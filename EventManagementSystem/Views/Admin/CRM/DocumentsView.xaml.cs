using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for DocumentsView.xaml
    /// </summary>
    public partial class DocumentsView : UserControl
    {
        private readonly DocumentsViewModel _viewModel;

        public DocumentsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new DocumentsViewModel();

            Loaded += OnDocumentsViewLoaded;
        }

        private void OnDocumentsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnDocumentsViewLoaded;
            _viewModel.LoadData();
        }
    }
}
