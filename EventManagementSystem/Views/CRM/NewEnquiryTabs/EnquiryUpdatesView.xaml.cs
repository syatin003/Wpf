using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs
{
    /// <summary>
    /// Interaction logic for EnquiryUpdatesView.xaml
    /// </summary>
    public partial class EnquiryUpdatesView : UserControl
    {
        private readonly EnquiryUpdatesViewModel _viewModel;

        public EnquiryUpdatesView()
        {
            InitializeComponent();

            DataContext = _viewModel = new EnquiryUpdatesViewModel();

            Loaded += OnEnquiryUpdatesViewLoaded;
        }

        private void OnEnquiryUpdatesViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var enquiryView = (NewEnquiryView)this.ParentOfType<RadWindow>();
            _viewModel.Enquiry = enquiryView.ViewModel.Enquiry;
        }
    }
}
