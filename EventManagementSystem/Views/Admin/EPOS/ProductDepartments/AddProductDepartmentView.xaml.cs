using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.EPOS.ProductDepartments;
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

namespace EventManagementSystem.Views.Admin.EPOS.ProductDepartments
{
    /// <summary>
    /// Interaction logic for AddGroupView.xaml
    /// </summary>
    public partial class AddProductDepartmentView : RadWindow
    {
        public AddProductDepartmentViewModel ViewModel { get; private set; }

        public AddProductDepartmentView(ProductDepartmentModel productGroupModel)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddProductDepartmentViewModel(productGroupModel);
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
        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            //DialogResult = true;
            //Close();
        }
    }
}
