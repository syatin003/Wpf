using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.EPOS.Products;
using Telerik.Windows.Controls;
using System.ComponentModel;

namespace EventManagementSystem.Views.Admin.EPOS.Products
{
    /// <summary>
    /// Interaction logic for AddProductView.xaml
    /// </summary>
    public partial class AddProductView : RadWindow
    {
        public readonly AddProductViewModel ViewModel;

        public AddProductView(ProductModel product = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddProductViewModel(product);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            Owner = Application.Current.MainWindow;

            Loaded += OnAddProductViewLoaded;
        }
        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
        }

        private void OnAddProductViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
