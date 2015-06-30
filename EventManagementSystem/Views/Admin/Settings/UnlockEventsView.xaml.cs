using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Settings;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for UnlockEventsView.xaml
    /// </summary>
    public partial class UnlockEventsView : UserControl
    {
        private readonly UnlockEventsViewModel _viewModel;

        public UnlockEventsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new UnlockEventsViewModel();

            Loaded += OnUnlockEventsViewLoaded;
        }

        private void OnUnlockEventsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnUnlockEventsViewLoaded;
            _viewModel.LoadData();
        }
    }
}
