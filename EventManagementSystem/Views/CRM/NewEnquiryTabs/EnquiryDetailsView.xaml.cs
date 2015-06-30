using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs
{
    /// <summary>
    /// Interaction logic for EnquiryView.xaml
    /// </summary>
    public partial class EnquiryDetailsView : UserControl
    {
        public EnquiryDetailsView()
        {
            InitializeComponent();

            Loaded += OnEnquiryDetailsViewLoaded;
        }

        private void OnEnquiryDetailsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var enquiryView = (NewEnquiryView)this.ParentOfType<RadWindow>();
            DataContext = enquiryView.ViewModel;
        }
    }
}
