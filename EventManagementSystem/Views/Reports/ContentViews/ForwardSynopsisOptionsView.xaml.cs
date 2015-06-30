using EventManagementSystem.ViewModels.Reports.ContentViewModels;
using System.Windows;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for ForwardSynopsisOptionsView.xaml
    /// </summary>
    public partial class ForwardSynopsisOptionsView
    {
        private readonly ForwardSynopsisDescriptionViewModel _viewModel;

        public ForwardSynopsisOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ForwardSynopsisDescriptionViewModel();

            Loaded +=  OnViewLoaded;
        }

        void  OnViewLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadOptions();
        }
    }
}
