using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.CRM;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for GolfInformationView.xaml
    /// </summary>
    public partial class EmailSettingsView : UserControl
    {
        private readonly EmailSettingsViewModel _viewModel;

        public EmailSettingsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EmailSettingsViewModel();
        }
    }
}
