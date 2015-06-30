using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Settings;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for GolfInformationView.xaml
    /// </summary>
    public partial class ClubInformationView : UserControl
    {
        private readonly ClubInformationViewModel _viewModel;

        public ClubInformationView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ClubInformationViewModel();
        }
    }
}
