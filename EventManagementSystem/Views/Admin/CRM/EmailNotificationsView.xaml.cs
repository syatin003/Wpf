using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for EmailNotificationsView.xaml
    /// </summary>
    public partial class EmailNotificationsView : UserControl
    {
        private readonly EmailNotificationsViewModel _viewModel;

        public EmailNotificationsView()
        {
            InitializeComponent();

            DataContext = _viewModel = new EmailNotificationsViewModel();
        }
    }
}
