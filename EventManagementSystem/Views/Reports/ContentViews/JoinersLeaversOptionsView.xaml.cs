using System.Windows;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for JoinersLeaversOptionsView.xaml
    /// </summary>
    public partial class JoinersLeaversOptionsView
    {
        private readonly JoinersLeaversOptionsViewModel _viewModel;

        public JoinersLeaversOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new JoinersLeaversOptionsViewModel();
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
