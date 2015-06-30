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
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for CustomersOptionsView.xaml
    /// </summary>
    public partial class CustomersOptionsView : UserControl
    {
        private readonly CustomersViewModel _viewModel;

        public CustomersOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new CustomersViewModel();
            Loaded += OnViewLoaded;
            Unloaded += CustomersOptionsView_Unloaded;
        }

        private void CustomersOptionsView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetOptions();
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadOptions();
        }
    }
}
