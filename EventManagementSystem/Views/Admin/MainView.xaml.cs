using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private readonly MainViewModel _viewModel;

        public MainView()
        {
            InitializeComponent();
            DataContext = _viewModel = new MainViewModel();
        }
    }
}