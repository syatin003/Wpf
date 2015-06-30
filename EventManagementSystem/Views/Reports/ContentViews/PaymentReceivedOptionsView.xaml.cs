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
    /// Interaction logic for PaymentOptionsView.xaml
    /// </summary>
    public partial class PaymentReceivedOptionsView : UserControl
    {
        private readonly PaymentReceivedOptionsViewModel _viewModel;
        public PaymentReceivedOptionsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new PaymentReceivedOptionsViewModel();
            Loaded += PaymentOptionsView_Loaded;
            Unloaded += PaymentOptionsView_Unloaded;
        }

        void PaymentOptionsView_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetOptions();
        }

        void PaymentOptionsView_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadOptions();
        }
    }
}
