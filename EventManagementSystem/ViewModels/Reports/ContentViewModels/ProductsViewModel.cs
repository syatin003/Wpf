using System;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ObjectBuilder2;
using System.Diagnostics;
using EventManagementSystem.Core.Commands;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private bool _isBusy;
        private DateTime _startDate;
        private DateTime _endDate;
        private ObservableCollection<TillTransactionProduct> _transactionProducts;
        private ObservableCollection<dynamic> _groupingTransactionProducts;

        private bool _isDataLoadedOnce;
        private bool _isDirty;

        #endregion

        #region Properties

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

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                Properties.Settings.Default.StartDateOptionProducts = _startDate;
                RaisePropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                Properties.Settings.Default.EndDateOptionProducts = _endDate;
                RaisePropertyChanged(() => EndDate);
            }
        }

        public ObservableCollection<TillTransactionProduct> TransactionProducts
        {
            get { return _transactionProducts; }
            set
            {
                if (_transactionProducts == value) return;
                _transactionProducts = value;
                RaisePropertyChanged(() => TransactionProducts);
            }
        }

        public ObservableCollection<dynamic> GroupedTransactionProducts
        {
            get { return _groupingTransactionProducts; }
            set
            {
                if (_groupingTransactionProducts == value) return;
                _groupingTransactionProducts = value;
                RaisePropertyChanged(() => GroupedTransactionProducts);
            }
        }

        public bool IsDataLoadedOnce
        {
            get { return _isDataLoadedOnce; }
            set
            {
                if (_isDataLoadedOnce == value) return;
                _isDataLoadedOnce = value;
                RaisePropertyChanged(() => IsDataLoadedOnce);
            }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                RaisePropertyChanged(() => IsDirty);
                GoButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand GoButtonCommand { get; private set; }

        #endregion

        #region Constructor

        public ProductsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();

            GoButtonCommand = new RelayCommand(GoButtonCommandExecuted, GoButtonCommandCanExecute);
            this.PropertyChanged += ProductsViewModel_PropertyChanged;
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            IsBusy = true;

            LoadOptions();

            GroupTransactionProducts(StartDate, EndDate);

            IsDataLoadedOnce = true;

        }

        /// <summary>
        /// This method groups all sold products for the period [startDate, endDate] by product's name, clerk's name and priceLevel
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        private async void GroupTransactionProducts(DateTime startDate, DateTime endDate)
        {
            IsBusy = true;

            var transactionProducts = await _reportsDataUnit.TillTransactionProductsRepository.GetTransactionsWithRelatedData(startDate, endDate);
            TransactionProducts = new ObservableCollection<TillTransactionProduct>(transactionProducts);

            var rezult = TransactionProducts.Where(product => product.TillTransaction.Date <= endDate && product.TillTransaction.Date >= startDate).GroupBy(
                product => new { product.TillTransaction.TillID, product.TillProduct.Name, product.TillTransaction.Clerk, product.PriceLevel }).Select(g =>
                {
                    var quantity = g.Sum(x => x.Quantity);            // total quantity of product       
                    var rrp = g.First().SalePrice;                      // retail price of this product
                    if (quantity > 0 && rrp.HasValue && rrp < 0)        //if some product was sold in Refund mode, it's price is negative
                        rrp *= -1;                                      // so we need to make it positive
                    return new
                    {
                        ClerkName = g.Key.Clerk.Name,               // name of the clerk who sold this product
                        TillProductName = g.Key.Name,               //name of the product
                        ProductGroupName = g.First().TillProduct.ProductGroup.GroupName,        //name of the product's group
                        DepartmentName = g.First().TillProduct.ProductDepartment.Department,    //name of the product's department
                        TillID = g.First().TillTransaction.Till.Identifier,                     //id of the till where product was sold
                        Quantity = (decimal)quantity,                                          // total quantity of the product that was sold
                        Value = g.Sum(x => x.Value),                                            //total value of the product that was sold
                        SalePrice = rrp * (decimal)quantity,                                                        // retail price of the product * quantity
                        Cash = g.Where(x => x.TillTransaction.TillTransactionFinaliseKeys.Any(key => key.FinaliseKey.Name == "CASH")).Sum(x => (x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "CASH").Sum(k => k.Value) > x.Value) ? x.Value : x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "CASH").Sum(k => k.Value)),  //total value of cash
                        Cheque = g.Where(x => x.TillTransaction.TillTransactionFinaliseKeys.Any(key => key.FinaliseKey.Name == "CHEQUE")).Sum(x => (x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "CHEQUE").Sum(k => k.Value) > x.Value) ? x.Value : x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "CHEQUE").Sum(k => k.Value)),
                        //CreditCard = g.Where(x => x.TillTransaction.TillTransactionFinaliseKeys.Any(key => key.FinaliseKey.Name == "CREDIT CARD")).Sum(x => x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "CREDIT CARD").Sum(k => k.Value)),
                        CreditCard = g.Where(x => x.TillTransaction.TillTransactionFinaliseKeys.Any(key => key.FinaliseKey.Name == "CREDIT CARD")).Sum(x => (x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "CREDIT CARD").Sum(k => k.Value) > x.Value) ? x.Value : x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "CREDIT CARD").Sum(k => k.Value)),
                        ClubCard = g.Where(x => x.TillTransaction.TillTransactionFinaliseKeys.Any(key => key.FinaliseKey.Name == "Members Card" || key.FinaliseKey.Name == "Bar Card")).Sum(x => (x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "Members Card" || key.FinaliseKey.Name == "Bar Card").Sum(k => k.Value) > x.Value) ? x.Value : x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "Members Card" || key.FinaliseKey.Name == "Bar Card").Sum(k => k.Value)),
                        Events = g.Where(x => x.TillTransaction.TillTransactionFinaliseKeys.Any(key => key.FinaliseKey.Name == "Charge to Event")).Sum(x => (x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "Charge to Event").Sum(k => k.Value) > x.Value) ? x.Value : x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "Charge to Event").Sum(k => k.Value)),
                        Voucher = g.Where(x => x.TillTransaction.TillTransactionFinaliseKeys.Any(key => key.FinaliseKey.Name == "Voucher")).Sum(x => (x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "Voucher").Sum(k => k.Value) > x.Value) ? x.Value : x.TillTransaction.TillTransactionFinaliseKeys.Where(key => key.FinaliseKey.Name == "Voucher").Sum(k => k.Value)),
                        Other = g.Where(x => x.TillTransaction.TillTransactionFinaliseKeys.Any(key => key.FinaliseKey.Record > 0 &      // need to check FinaliseKey.Record > 0 because there are Finalise Keys for Tabs with negative records
                                                                                                      key.FinaliseKey.Name != "Charge to Event" & key.FinaliseKey.Name != "Voucher" & key.FinaliseKey.Name != "Members Card" & key.FinaliseKey.Name != "Bar Card" & key.FinaliseKey.Name != "CREDIT CARD" & key.FinaliseKey.Name != "CHEQUE" & key.FinaliseKey.Name != "CASH")).Sum(x => x.Value),
                        Discount = g.Sum(x => x.Discount),
                    };
                }).OrderBy(data => data.TillProductName).ThenBy(data => data.ClerkName);

            GroupedTransactionProducts = new ObservableCollection<dynamic>(rezult.ToList());

            IsBusy = false;
        }

        private void LoadOptions()
        {
            if (Properties.Settings.Default.StartDateOptionProducts == default(DateTime) && Properties.Settings.Default.EndDateOptionProducts == default(DateTime))
            {
                Properties.Settings.Default.StartDateOptionProducts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                Properties.Settings.Default.EndDateOptionProducts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            }

            StartDate = Properties.Settings.Default.StartDateOptionProducts;
            EndDate = Properties.Settings.Default.EndDateOptionProducts;
        }

        private void ProductsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsDataLoadedOnce)
            {
                if (e.PropertyName == "StartDate" || e.PropertyName == "EndDate")
                {
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region Commands

        private bool GoButtonCommandCanExecute()
        {
            return IsDirty;
        }

        private void GoButtonCommandExecuted()
        {
            GroupTransactionProducts(_startDate, _endDate);

            IsDirty = false;
        }

        #endregion
    }
}
