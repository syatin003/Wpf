using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for ForwardBookOptionsView.xaml
    /// </summary>
    public partial class ForwardBookOptionsView : UserControl
    {
        private readonly ForwardBookViewModel _viewModel;

        public ForwardBookOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ForwardBookViewModel();

            Loaded += OnViewLoaded;
            Unloaded += CustomersOptionsView_Unloaded;
        }

        private void CustomersOptionsView_Unloaded(object sender, RoutedEventArgs e)
        {
            //_viewModel.LoadOptions();
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.SetDescriptionActivated(false);
            _viewModel.LoadOptions();
        }
    }
}
