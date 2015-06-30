using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for CalendarOptionsView.xaml
    /// </summary>
    public partial class CalendarOptionsView : UserControl
    {
        private readonly CalendarOptionsViewModel _viewModel;

        public CalendarOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new CalendarOptionsViewModel();
            Loaded += OnViewLoaded;
            Unloaded += CalenderOptionsView_Unloaded;
        }

        private void CalenderOptionsView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetOptions();
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadOptions();
        }
    }
}
