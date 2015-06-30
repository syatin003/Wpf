using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Events;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Events
{
    /// <summary>
    /// Interaction logic for DuplicateView.xaml
    /// </summary>
    public partial class DuplicateView : RadWindow
    {

        public DuplicateViewModel ViewModel { get; private set; }

        public DuplicateView()
        {
            InitializeComponent();
            DataContext = ViewModel = new DuplicateViewModel();

            Owner = Application.Current.MainWindow;
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
