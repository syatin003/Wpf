using EventManagementSystem.ViewModels.Reports.ContentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for RoundAgeAnalysisOptionsView.xaml
    /// </summary>
    public partial class RoundAgeAnalysisOptionsView : UserControl
    {
        private readonly RoundAgeAnalysisOptionsViewModel _viewModel;

        public RoundAgeAnalysisOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new RoundAgeAnalysisOptionsViewModel();
            Loaded += OnViewLoaded;
            Unloaded += RoundAgeAnalysisOptionsView_Unloaded;
        }
        private void RoundAgeAnalysisOptionsView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetOptions();
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadOptions();
        }
    }
}
