using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core;

namespace EventManagementSystem.Views.Core
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : UserControl
    {
        private readonly WelcomeViewModel _viewModel;

        public WelcomeView()
        {
            InitializeComponent();
            DataContext = _viewModel = new WelcomeViewModel();
        }
    }
}
