using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Views.Admin.EPOS.Products;

namespace EventManagementSystem.ViewModels.Admin.EPOS.Products
{
    public class ProductsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private readonly IReportsDataUnit _reportsDataUnit;
        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;

        private ObservableCollection<ProductModel> _products;
        private ProductType _selectedProductType;
        private ProductModel _selectedProduct;


        #endregion

        #region Properties

        public List<ProductModel> AllProducts { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ObservableCollection<ProductModel> Products
        {
            get { return _products; }
            set
            {
                if (_products == value) return;
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }
        public ProductType SelectedProductType
        {
            get { return _selectedProductType; }
            set
            {
                if (_selectedProductType == value) return;
                _selectedProductType = value;
                RaisePropertyChanged(() => SelectedProductType);
            }
        }

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct == value) return;
                _selectedProduct = value;
                RaisePropertyChanged(() => SelectedProduct);
                RaisePropertyChanged("ScrollToSelectedItem");
            }
        }
        public RelayCommand<ProductModel> DeleteProductCommand { get; private set; }
        public RelayCommand<ProductModel> EditProductCommand { get; private set; }

        #endregion

        #region Constructors

        public ProductsViewModel(ProductType productType)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
            SelectedProductType = productType;

            DeleteProductCommand = new RelayCommand<ProductModel>(DeleteProductCommandExecuted);
            EditProductCommand = new RelayCommand<ProductModel>(EditProductCommandExecuted);

        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.ProductsRepository.Refresh();

            if (SelectedProductType == null)
            {
                var products = await _adminDataUnit.ProductsRepository.GetAllAsync();
                AllProducts = new List<ProductModel>(products.Select(x => new ProductModel(x)));
            }
            else
            {
                var products = await _adminDataUnit.ProductsRepository.GetAllAsync(product => product.ProductType.ID == SelectedProductType.ID);
                AllProducts = new List<ProductModel>(products.Select(x => new ProductModel(x)));
            }
            RefreshProducts();

            IsBusy = false;
        }
        public void RefreshProducts()
        {
            Products = new ObservableCollection<ProductModel>(
                (SelectedProductType != null) ? AllProducts.Where(x => x.Product.ProductType == SelectedProductType) : AllProducts);
        }

        #endregion

        #region Commands

        private async void DeleteProductCommandExecuted(ProductModel productModel)
        {
            if (productModel == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            // Check if product has dependencies
            if (productModel.Product.EventBookedProducts.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine("Sorry, we can't delete this product :(");
                sb.AppendLine("This product already in use by these events:");

                productModel.Product.EventBookedProducts.Select(x => x.Event.Name).ForEach(x => sb.AppendLine(string.Format("- {0}", x)));

                RaisePropertyChanged("DisableParentWindow");

                RadWindow.Alert(sb.ToString());

                RaisePropertyChanged("EnableParentWindow");

                return;
            }

            // Delete product event types
            var types = await _adminDataUnit.ProductEventTypesRepository.GetAllAsync(x => x.ProductID == productModel.Product.ID);
            _adminDataUnit.ProductEventTypesRepository.Delete(types);

            // Delete product
            _adminDataUnit.ProductsRepository.Delete(productModel.Product);

            await _adminDataUnit.SaveChanges();

            Products.Remove(productModel);
        }

        private void EditProductCommandExecuted(ProductModel productModel)
        {
            RaisePropertyChanged("DisableParentWindow");

            SelectedProduct = productModel;

            var view = new AddProductView(productModel);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true)
            {
                _reportsDataUnit.ProductsRepository.Refresh();
                _eventsDataUnit.ProductsRepository.Refresh();
            }
        }

        #endregion
    }
}