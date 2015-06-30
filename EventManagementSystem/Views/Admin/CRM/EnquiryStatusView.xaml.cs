using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Data.Model;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for EnquiryStatusView.xaml
    /// </summary>
    public partial class EnquiryStatusView : UserControl
    {
        private readonly EnquiryStatusesViewModel _viewModel;

        public EnquiryStatusView(EnquiryStatus status)
        {
            InitializeComponent();
            DataContext = _viewModel = new EnquiryStatusesViewModel(status);

            Loaded += OnEnquiryStatusViewLoaded;

            Unloaded += EnquiryStatusView_Unloaded;
        }

        private void EnquiryStatusView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Refresh();
        }

        private void OnEnquiryStatusViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnEnquiryStatusViewLoaded;
            _viewModel.LoadData();
        }
    }
}
