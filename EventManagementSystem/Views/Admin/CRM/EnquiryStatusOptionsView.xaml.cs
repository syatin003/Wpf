using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Data.Model;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for EnquiryStatusOptionsView.xaml
    /// </summary>
    public partial class EnquiryStatusOptionsView : UserControl
    {
        private readonly EnquiryStatusesViewModel _viewModel;

        public EnquiryStatusOptionsView(EnquiryStatus status)
        {
            InitializeComponent();
            DataContext = _viewModel = new EnquiryStatusesViewModel(status);

            Loaded += OnEnquiryStatusViewLoaded;
        }

        private void OnEnquiryStatusViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnEnquiryStatusViewLoaded;
            _viewModel.LoadData();
        }
    }
}
