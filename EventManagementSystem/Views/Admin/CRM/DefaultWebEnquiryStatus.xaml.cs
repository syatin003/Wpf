using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for DefaultWebEnquiryStatus.xaml
    /// </summary>
    public partial class DefaultWebEnquiryStatus : UserControl
    {
        private readonly DefaultWebEnquiryStatusViewModel _viewModel;

        public DefaultWebEnquiryStatus()
        {
            InitializeComponent();
            DataContext = _viewModel = new DefaultWebEnquiryStatusViewModel();

            Loaded += OnDefaultWebEnquiryStatusViewLoaded;
        }

        private void OnDefaultWebEnquiryStatusViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnDefaultWebEnquiryStatusViewLoaded;
            _viewModel.LoadData();
        }
    }
}
