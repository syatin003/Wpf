using EventManagementSystem.ViewModels.Reports.ContentViewModels;
using System.Windows;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for LeaversOptionsView.xaml
    /// </summary>
    public partial class LeaversOptionsView
    {
        private readonly LeaversOptionsViewModel _viewModel;

        public LeaversOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new LeaversOptionsViewModel();
            Loaded += OnViewLoaded;
            Unloaded += LeaversOptionsView_Unloaded;

        }
        private void LeaversOptionsView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetOptions();
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadOptions();
        }
    }
}
