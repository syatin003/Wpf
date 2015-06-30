using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for ActivitySettingsView.xaml
    /// </summary>
    public partial class ActivitySettingsView : UserControl
    {
        private readonly ActivitySettingsViewModel _viewModel;

        public ActivitySettingsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ActivitySettingsViewModel();

            Loaded += OnActivitySettingsViewLoaded;
        }

        private void OnActivitySettingsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnActivitySettingsViewLoaded;
            _viewModel.LoadData();
        }
    }
}
