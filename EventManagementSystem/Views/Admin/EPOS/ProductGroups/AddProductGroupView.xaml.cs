using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.EPOS.ProductGroups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace EventManagementSystem.Views.Admin.EPOS.ProductGroups
{
    /// <summary>
    /// Interaction logic for AddGroupView.xaml
    /// </summary>
    public partial class AddProductGroupView : RadWindow
    {
        public AddProductGroupViewModel ViewModel { get; private set; }

        public AddProductGroupView(ProductGroupModel productGroupModel)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddProductGroupViewModel(productGroupModel);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnViewLoaded;
        }
        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
        }
        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }
    }
}
