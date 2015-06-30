using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.EPOS.Tills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace EventManagementSystem.Views.Admin.EPOS.Tills
{
    /// <summary>
    /// Interaction logic for AddTillView.xaml
    /// </summary>
    public partial class AddTillView : RadWindow
    {
        public AddTillViewModel ViewModel { get; private set; }

        public AddTillView(ObservableCollection<TillDivisionModel> tillDivisions)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddTillViewModel(tillDivisions);

            Owner = Application.Current.MainWindow;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
