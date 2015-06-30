using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Settings;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for SystemSettingsView.xaml
    /// </summary>
    public partial class SystemSettingsView : UserControl
    {
        private readonly SystemSettingsViewModel _viewModel;

        public SystemSettingsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new SystemSettingsViewModel();
            Loaded += SystemSettingsView_Loaded;
        }

        public void SystemSettingsView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.LoadData();
        }
    }
}
