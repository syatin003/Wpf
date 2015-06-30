using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Data.Model;
using EventManagementSystem.ViewModels.Admin.CRM;

namespace EventManagementSystem.Views.Admin.CRM
{
    /// <summary>
    /// Interaction logic for StatusOfFollowUpView.xaml
    /// </summary>
    public partial class StatusOfFollowUpView : UserControl
    {
        private readonly StatusOfFollowUpViewModel _viewModel;

        public StatusOfFollowUpView(FollowUpStatus followUpStatus)
        {
            InitializeComponent();

            DataContext = _viewModel = new StatusOfFollowUpViewModel(followUpStatus);
            Loaded += OnCRMViewLoaded;
            Unloaded += StatusOfFollowUpView_Unloaded;
        }

        private void StatusOfFollowUpView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Refresh();
        }

        private void OnCRMViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnCRMViewLoaded;
            _viewModel.LoadData();
        }
    }
}
