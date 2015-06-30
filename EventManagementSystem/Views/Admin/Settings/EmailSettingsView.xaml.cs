using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Settings;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for EmailSettingsView.xaml
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
