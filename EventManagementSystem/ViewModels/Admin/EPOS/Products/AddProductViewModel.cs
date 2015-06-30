using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Helpers;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;
using EventManagementSystem.Services;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.ViewModels.Admin.EPOS.Products
{
    public class AddProductViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private ObservableCollection<CheckedListItem<EventTypeModel>> _checkedEventTypes;
        private bool _isBusy;
        private bool _isEdit;
        private ProductModel _product;
        private ObservableCollection<ProductDepartment> _productDepartments;
        private ObservableCollection<ProductGroup> _productGroups;
        private ObservableCollection<ProductOption> _productOptions;
        private ObservableCollection<ProductType> _productTypes;
        private ObservableCollection<ProductVATRate> _productVatRates;
        private List<EventTypeModel> _allEventTypes;
        private SystemSetting _startingPLUTillProductsSetting;
        private bool _isChargeType;
        private bool _isLevyType;

        #endregion

        #region Properties

        public string DefaultProductType { get; set; }

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
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                if (_isEdit == value) return;
                _isEdit = value;
                RaisePropertyChanged(() => IsEdit);
            }
        }

        public ObservableCollection<CheckedListItem<EventTypeModel>> CheckedEventTypes
        {
            get { return _checkedEventTypes; }
            set
            {
                if (_checkedEventTypes == value) return;
                _checkedEventTypes = value;
                RaisePropertyChanged(() => CheckedEventTypes);
            }
        }

        public ObservableCollection<ProductType> ProductTypes
        {
            get { return _productTypes; }
            set
            {
                if (_productTypes == value) return;
                _productTypes = value;
                RaisePropertyChanged(() => ProductTypes);
            }
        }

        public ObservableCollection<ProductVATRate> ProductVatRates
        {
            get { return _productVatRates; }
            set
            {
                if (_productVatRates == value) return;
                _productVatRates = value;
                RaisePropertyChanged(() => ProductVatRates);
            }
        }

        public ObservableCollection<ProductGroup> ProductGroups
        {
            get { return _productGroups; }
            set
            {
                if (_productGroups == value) return;
                _productGroups = value;
                RaisePropertyChanged(() => ProductGroups);
            }
        }

        public ObservableCollection<ProductDepartment> ProductDepartments
        {
            get { return _productDepartments; }
            set
            {
                if (_productDepartments == value) return;
                _productDepartments = value;
                RaisePropertyChanged(() => ProductDepartments);
            }
        }

        public ObservableCollection<ProductOption> ProductOptions
        {
            get { return _productOptions; }
            set
            {
                if (_productOptions == value) return;
                _productOptions = value;
                RaisePropertyChanged(() => ProductOptions);
            }
        }

        public ProductModel Product
        {
            get { return _product; }
            set
            {
                if (_product == value) return;
                _product = value;
                RaisePropertyChanged(() => Product);
            }
        }

        public SystemSetting StartingPLUTillProductsSetting
        {
            get { return _startingPLUTillProductsSetting; }
            set
            {
                if (_startingPLUTillProductsSetting == value) return;
                _startingPLUTillProductsSetting = value;
                RaisePropertyChanged(() => StartingPLUTillProductsSetting);
            }
        }
        public bool IsChargeType
        {
            get { return _isChargeType; }
            set
            {
                if (_isChargeType == value) return;
                _isChargeType = value;
                RaisePropertyChanged(() => IsChargeType);
            }
        }
        public bool IsLevyType
        {
            get { return _isLevyType; }
            set
            {
                if (_isLevyType == value) return;
                _isLevyType = value;
                RaisePropertyChanged(() => IsLevyType);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public AddProductViewModel(ProductModel model)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessProduct(model);
        }

        #endregion

        #region Methods

        private void ProcessProduct(ProductModel product)
        {
            IsEdit = (product != null);

            Product = (IsEdit) ? product : GetProduct();

            Product.PropertyChanged += ProductOnPropertyChanged;

        }

        private void GetComponentType()
        {
            Product.IsMembership = true;
            if (Product.Product.ComponentType == true)
                IsLevyType = true;
            else
                IsChargeType = true;
        }

        private ProductModel GetProduct()
        {
            IsChargeType = true;
            return new ProductModel(new Product
            {
                ID = Guid.NewGuid(),
                isAvailable = true
            });
        }

        public async void LoadData()
        {
            IsBusy = true;

            var types = await _adminDataUnit.EventTypesRepository.GetAllAsync();
            _allEventTypes = new List<EventTypeModel>(types.OrderBy(x => x.Name).Select(x => new EventTypeModel(x)));

            var rates = await _adminDataUnit.ProductVatRatesRepository.GetAllAsync();
            ProductVatRates = new ObservableCollection<ProductVATRate>(rates.OrderBy(x => x.Rate));

            var groups = await _adminDataUnit.ProductGroupsRepository.GetAllAsync(x => x.GroupName != "Blank");
            ProductGroups = new ObservableCollection<ProductGroup>(groups.OrderBy(x => x.GroupName));


            var productTypes = await _adminDataUnit.ProductTypesRepository.GetAllAsync();
            ProductTypes = new ObservableCollection<ProductType>(productTypes.OrderBy(x => x.Type));

            var departments = await _adminDataUnit.ProductDepartmentsRepository.GetAllAsync(x => x.Department != "Blank");
            ProductDepartments = new ObservableCollection<ProductDepartment>(departments.OrderBy(x => x.Department));

            var options = await _adminDataUnit.ProductOptionsRepository.GetAllAsync();
            ProductOptions = new ObservableCollection<ProductOption>(options.OrderBy(x => x.OptionName));

            var startingPLUSetting = await _adminDataUnit.SystemSettingsRepository.GetSettingByName("StartingPLUID");
            StartingPLUTillProductsSetting = startingPLUSetting;

            if (IsEdit)
            {
                if (Product.ProductType.Type == "Membership")
                    GetComponentType();
            }
            SetDefaultProductType();

            RefreshProductEventTypes(Product);

            IsBusy = false;
        }

        private void SetDefaultProductType()
        {
            if (!string.IsNullOrWhiteSpace(DefaultProductType))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(
                    () => Product.ProductType = _productTypes.FirstOrDefault(x => x.Type == DefaultProductType)));
            }
        }

        private void RefreshProductEventTypes(ProductModel model)
        {
            var types = new List<CheckedListItem<EventTypeModel>>();

            foreach (var type in _allEventTypes)
            {
                var isChecked = Product.EventTypes.Select(x => x.EventType).Contains(type.EventType);
                var item = new CheckedListItem<EventTypeModel>(type, isChecked);

                item.PropertyChanged += ItemOnPropertyChanged;
                types.Add(item);
            }

            CheckedEventTypes = new ObservableCollection<CheckedListItem<EventTypeModel>>(types);
        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var item = sender as CheckedListItem<EventTypeModel>;

            if (item.IsChecked)
                Product.EventTypes.Add(item.Item);
            else
                Product.EventTypes.Remove(item.Item);
            Product.RefreshEventTypesAbbreviations();

        }

        private void ProductOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "ProductType")
            {
                if (Product.ProductType.Type == "Membership")
                {
                    if (!IsEdit)
                    {
                        Product.IsMembership = true;
                        SetMembershipDefaultOptions();
                    }
                    else
                        GetComponentType();
                }
                else
                {
                    Product.IsMembership = false;
                    Product.Product.IsPartPayment = null;
                    Product.Product.IsAutoRenewable = null;
                    Product.Product.IsDiscountable = null;
                    Product.Product.IsAllowAd = null;
                    Product.Product.PointsReceived = null;
                    Product.Product.ComponentType = null;
                    Product.EarlyPaymentDiscount = null;
                }
            }
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void SetMembershipDefaultOptions()
        {
            Product.Product.IsPartPayment = false;
            Product.Product.IsAutoRenewable = false;
            Product.Product.IsDiscountable = false;
            Product.Product.IsAllowAd = false;
            Product.Product.PointsReceived = 0.00;
            Product.Product.ComponentType = false;
            Product.EarlyPaymentDiscount = 0.00;
        }

        private async void SendDataToAllActiveTills()
        {
            var tillProductModel = new EventManagementSystem.Models.TillDomainObjects.TillProductModel
             {
                 Record = Product.Product.PluID,
                 Name = Product.Name,
                 Price1L1 = Convert.ToDecimal(Math.Round(Product.GrossPrice, 2, MidpointRounding.AwayFromZero)),
                 GroupRecord = Product.ProductGroup.Record,
                 DepartmentRecord = Product.ProductDepartment.Record,
                 ProductRateRecord = Product.ProductVATRate.Record
             };

            var activeTills = await _adminDataUnit.TillsRepository.GetAllAsync(tillItem => tillItem.Enabled);

            var tillNames = activeTills.Select(tillItem => tillItem.Name).ToList();
            var tillIPAddresses = activeTills.Select(tillItem => tillItem.IPAddress).ToList();

            TillsCommunicationService.SetTillProduct(tillProductModel, tillNames, tillIPAddresses);

            IsBusy = false;

            RaisePropertyChanged("CloseDialog");

        }
        #endregion

        #region Commands

        private async void SaveCommandExecuted()
        {
            IsBusy = true;

            if (_product.Product.ProductType.Type == "Membership")
            {
                if (IsEdit && Product.Product.ProductOptionID != null)
                {
                    Product.Product.ProductOptionID = null;
                }
                if (Product.EventTypes.Any())
                {
                    var productEventTypes = await _adminDataUnit.ProductEventTypesRepository.GetAllAsync(x => x.ProductID == Product.Product.ID);
                    foreach (var productEventType in productEventTypes)
                    {
                        _adminDataUnit.ProductEventTypesRepository.Delete(productEventType);
                    }
                    Product.EventTypes = new ObservableCollection<EventTypeModel>();
                    Product.RefreshEventTypesAbbreviations();
                }
                if (IsChargeType)
                    _product.Product.ComponentType = false;
                else if (IsLevyType)
                    _product.Product.ComponentType = true;
            }
            else
            {
                if (Product.EventTypes.Any())
                {
                    foreach (var eventType in Product.EventTypes)
                    {
                        var typeAlreadyExist = Product.Product.ProductEventTypes.Where(x => x.EventTypeID == eventType.EventType.ID).FirstOrDefault();

                        if (typeAlreadyExist == null)
                        {
                            var productType = new ProductEventType()
                            {
                                ID = Guid.NewGuid(),
                                EventTypeID = eventType.EventType.ID,
                                ProductID = _product.Product.ID
                            };

                            _adminDataUnit.ProductEventTypesRepository.Add(productType);
                        }
                    }
                }
                var typesToDelete = Product.Product.ProductEventTypes.Where(x => !(Product.EventTypes.Select(eventType => eventType.EventType).Contains(x.EventType))).ToList();
                _adminDataUnit.ProductEventTypesRepository.Delete(typesToDelete);
            }
            if (!IsEdit)
            {
                Product.Product.PluID = Convert.ToInt32(StartingPLUTillProductsSetting.Value);
                StartingPLUTillProductsSetting.Value = Convert.ToString(Convert.ToInt32(StartingPLUTillProductsSetting.Value) + 1);

                _adminDataUnit.ProductsRepository.Add(Product.Product);
            }
            await _adminDataUnit.SaveChanges();

            SendDataToAllActiveTills();
        }

        private bool SaveCommandCanExecute()
        {
            return !Product.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            if (IsEdit)
            {
                _adminDataUnit.RevertChanges();
            }
            else
            {
                Product = null;
            }
        }

        #endregion
    }
}