using EventManagementSystem.ViewModels.Reports.ContentViewModels;
using System.Windows;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for JoinersActivityOptionsView.xaml
    /// </summary>
    public partial class JoinersActivityOptionsView
    {
        private readonly JoinersActivityOptionsViewModel _viewModel;

        public JoinersActivityOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new JoinersActivityOptionsViewModel();
            Loaded += OnViewLoaded;
            Unloaded += JoinersLeaversOptionsView_Unloaded;

        }
        private void JoinersLeaversOptionsView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetOptions();
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadOptions();
        }
    }
}
