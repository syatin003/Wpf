using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    class ForwardBookViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;

        private ObservableCollection<EventBookedProductModel> _eventBookedProducts;
        private ObservableCollection<GridViewDataColumn> _columnCollection;
        private ObservableCollection<ForwardBookModel> _forwardBook;

        private List<EventBookedProductModel> _allEventBookedProducts;
        private List<ProductModel> _allProducts;

        public IEnumerable<ProductDepartment> ProductDepartments { get; set; }
        public IEnumerable<ProductVATRate> ProductVATRates { get; set; }
        public IEnumerable<ProductGroup> ProductGroups { get; set; }

        private bool _isBusy;
        private DateTime _startDate;
        private DateTime _endDate;

        private bool _isTodayChecked;
        private bool _isYesterdayChecked;
        private bool _isStartOfCurrentMonthChecked;
        private bool _isStartOfPreviousMonthChecked;
        private bool _isStartOfCurrentYearChecked;
        private bool _isCurrentDateBackwardChecked;

        private bool _isTodayEndChecked;
        private bool _isEndOfNextMonthChecked;
        private bool _isEndOfCurrentMonthChecked;
        private bool _isEndOfCurrentYearChecked;
        private bool _isCurrentDateForwardChecked;

        private int _daysForward;
        private int _daysBackward;

        private int _startDateGroupValue;
        private int _endDateGroupValue;

        private DateTime _startDateOption;
        private DateTime _endDateOption;

        private bool _incVATOption;
        private bool _exVATOption;
        private bool _departmentOption;
        private bool _groupOption;
        private bool _enquiriesOption;
        private bool _provisionalOption;
        private bool _confirmedOption;
        private bool _invoicedOption;
        private bool _cancelledOption;
        private bool _minimizedOption;
        private bool _incMonthlyTotalsOption;
        private bool _isDataLoading;
        private bool _isRefreshing;
        private bool _isDescriptionActivated;

        private ObservableCollection<string> _statusItems;

        private string _propertyToGroupRow;

        #endregion

        #region Properties

        public string PropertyToGroupRow
        {
            get
            {
                if (DepartmentOption)
                    return "ProductDepartment";
                else
                    return "ProductGroup";
            }
            set
            {
                if (_propertyToGroupRow == value) return;
                _propertyToGroupRow = value;
                RaisePropertyChanged(() => PropertyToGroupRow);
            }
        }

        public ObservableCollection<string> StatusItems
        {
            get
            {
                _statusItems = new ObservableCollection<string>();
                if (CancelledOption)
                    _statusItems.Add("Cancelled");
                if (ConfirmedOption)
                    _statusItems.Add("Confirmed");
                if (InvoicedOption)
                    _statusItems.Add("Invoiced");
                if (ProvisionalOption)
                    _statusItems.Add("Provisional");
                if (EnquiriesOption)
                    _statusItems.Add("Enquiry");
                return _statusItems;
            }
            set
            {
                if (_statusItems == value) return;
                _statusItems = value;
                RaisePropertyChanged(() => StatusItems);
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                RaisePropertyChanged(() => StartDate);
                RefreshForwardBookings();
                RaisePropertyChanged(() => EventBookedProducts);
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
                if (EndDate.Year < StartDate.Year + 100)
                    RefreshForwardBookings();
                RaisePropertyChanged(() => EventBookedProducts);
            }
        }

        #region StartDateGroupProperties

        public bool IsTodayChecked
        {
            get { return _isTodayChecked; }
            set
            {
                if (_isTodayChecked == value) return;
                _isTodayChecked = value;
                if (_isTodayChecked)
                    StartDateGroupValue = 0;
                RaisePropertyChanged(() => IsTodayChecked);
                RaisePropertyChanged(() => StartDateOption);
            }
        }

        public bool IsYesterdayChecked
        {
            get { return _isYesterdayChecked; }
            set
            {
                if (_isYesterdayChecked == value) return;
                _isYesterdayChecked = value;
                if (_isYesterdayChecked)
                    StartDateGroupValue = 1;
                RaisePropertyChanged(() => IsYesterdayChecked);
                RaisePropertyChanged(() => StartDateOption);
            }
        }

        public bool IsStartOfCurrentMonthChecked
        {
            get { return _isStartOfCurrentMonthChecked; }
            set
            {
                if (_isStartOfCurrentMonthChecked == value) return;
                _isStartOfCurrentMonthChecked = value;
                if (_isStartOfCurrentMonthChecked)
                    StartDateGroupValue = 2;
                RaisePropertyChanged(() => IsStartOfCurrentMonthChecked);
                RaisePropertyChanged(() => StartDateOption);
            }
        }

        public bool IsStartOfPreviousMonthChecked
        {
            get { return _isStartOfPreviousMonthChecked; }
            set
            {
                if (_isStartOfPreviousMonthChecked == value) return;
                _isStartOfPreviousMonthChecked = value;
                if (_isStartOfPreviousMonthChecked)
                    StartDateGroupValue = 3;
                RaisePropertyChanged(() => IsStartOfPreviousMonthChecked);
                RaisePropertyChanged(() => StartDateOption);
            }
        }

        public bool IsStartOfCurrentYearChecked
        {
            get { return _isStartOfCurrentYearChecked; }
            set
            {
                if (_isStartOfCurrentYearChecked == value) return;
                _isStartOfCurrentYearChecked = value;
                if (_isStartOfCurrentYearChecked)
                    StartDateGroupValue = 4;
                RaisePropertyChanged(() => IsStartOfCurrentYearChecked);
                RaisePropertyChanged(() => StartDateOption);
            }
        }

        public bool IsCurrentDateBackwardChecked
        {
            get { return _isCurrentDateBackwardChecked; }
            set
            {
                if (_isCurrentDateBackwardChecked == value) return;
                _isCurrentDateBackwardChecked = value;
                if (_isCurrentDateBackwardChecked)
                    StartDateGroupValue = 5;
                RaisePropertyChanged(() => IsCurrentDateBackwardChecked);
                RaisePropertyChanged(() => StartDateOption);
            }
        }

        public int DaysBackward
        {
            get { return _daysBackward; }
            set
            {
                if (_daysBackward == value) return;
                _daysBackward = value;
                RaisePropertyChanged(() => DaysBackward);
                RaisePropertyChanged(() => StartDateOption);
            }
        }

        public DateTime StartDateOption
        {
            get
            {
                if (IsTodayChecked)
                    return DateTime.Today;
                if (IsYesterdayChecked)
                    return DateTime.Today.Add(TimeSpan.FromDays(-1));
                if (IsStartOfCurrentMonthChecked)
                    return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (IsStartOfPreviousMonthChecked)
                    return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                if (IsStartOfCurrentYearChecked)
                    return new DateTime(DateTime.Now.Year, 1, 1);
                if (IsCurrentDateBackwardChecked)
                    return DateTime.Today.Add(TimeSpan.FromDays(-DaysBackward));
                return _startDateOption;
            }
            set
            {
                if (_startDateOption == value) return;
                _startDateOption = value;
                SetStartDateGroup(false);
                _startDateGroupValue = -1;
                RaisePropertyChanged(() => StartDateOption);
            }
        }

        public int StartDateGroupValue
        {
            get { return _startDateGroupValue; }
            set
            {
                if (_startDateGroupValue == value) return;
                _startDateGroupValue = value;

                RaisePropertyChanged(() => StartDateGroupValue);
            }
        }


        #endregion

        #region EndDateGroupProperties

        public bool IsTodayEndChecked
        {
            get { return _isTodayEndChecked; }
            set
            {
                if (_isTodayEndChecked == value) return;
                _isTodayEndChecked = value;
                if (_isTodayEndChecked)
                    EndDateGroupValue = 0;
                RaisePropertyChanged(() => IsTodayEndChecked);
                RaisePropertyChanged(() => EndDateOption);
            }
        }

        public bool IsEndOfCurrentMonthChecked
        {
            get { return _isEndOfCurrentMonthChecked; }
            set
            {
                if (_isEndOfCurrentMonthChecked == value) return;
                _isEndOfCurrentMonthChecked = value;
                if (_isEndOfCurrentMonthChecked)
                    EndDateGroupValue = 1;
                RaisePropertyChanged(() => IsEndOfCurrentMonthChecked);
                RaisePropertyChanged(() => EndDateOption);
            }
        }

        public bool IsEndOfNextMonthChecked
        {
            get { return _isEndOfNextMonthChecked; }
            set
            {
                if (_isEndOfNextMonthChecked == value) return;
                _isEndOfNextMonthChecked = value;
                if (_isEndOfNextMonthChecked)
                    EndDateGroupValue = 2;
                RaisePropertyChanged(() => IsEndOfNextMonthChecked);
                RaisePropertyChanged(() => EndDateOption);
            }
        }

        public bool IsEndOfCurrentYearChecked
        {
            get { return _isEndOfCurrentYearChecked; }
            set
            {
                if (_isEndOfCurrentYearChecked == value) return;
                _isEndOfCurrentYearChecked = value;
                if (_isEndOfCurrentYearChecked)
                    EndDateGroupValue = 3;
                RaisePropertyChanged(() => IsEndOfCurrentYearChecked);
                RaisePropertyChanged(() => EndDateOption);
            }
        }
        public bool IsCurrentDateForwardChecked
        {
            get { return _isCurrentDateForwardChecked; }
            set
            {
                if (_isCurrentDateForwardChecked == value) return;
                _isCurrentDateForwardChecked = value;
                if (_isCurrentDateForwardChecked)
                    EndDateGroupValue = 4;
                RaisePropertyChanged(() => IsCurrentDateForwardChecked);
                RaisePropertyChanged(() => EndDateOption);
            }
        }
        public int DaysForward
        {
            get { return _daysForward; }
            set
            {
                if (_daysForward == value) return;
                _daysForward = value;
                RaisePropertyChanged(() => DaysForward);
                RaisePropertyChanged(() => EndDateOption);
            }
        }

        public DateTime EndDateOption
        {
            get
            {
                if (IsTodayEndChecked)
                    return DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                if (IsEndOfCurrentMonthChecked)
                    return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                if (IsEndOfNextMonthChecked)
                    return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                if (IsEndOfCurrentYearChecked)
                    return new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
                if (IsCurrentDateForwardChecked)
                    return DateTime.Today.Add(TimeSpan.FromDays(DaysForward));
                return _endDateOption;
            }
            set
            {
                if (_endDateOption == value) return;
                _endDateOption = value;
                SetEndDateGroup(false);
                _endDateGroupValue = -1;
                RaisePropertyChanged(() => EndDateOption);
                RefreshForwardBookings();
                RaisePropertyChanged(() => EventBookedProducts);
            }
        }

        public int EndDateGroupValue
        {
            get { return _endDateGroupValue; }
            set
            {
                if (_endDateGroupValue == value) return;
                _endDateGroupValue = value;
                RaisePropertyChanged(() => EndDateGroupValue);
            }
        }


        #endregion

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

        public List<ProductModel> AllProducts
        {
            get { return _allProducts; }
            set
            {
                if (_allProducts == value) return;
                _allProducts = value;
                RaisePropertyChanged(() => AllProducts);
            }
        }

        public ObservableCollection<EventBookedProductModel> EventBookedProducts
        {
            get { return _eventBookedProducts; }
            set
            {
                if (_eventBookedProducts == value) return;
                _eventBookedProducts = value;
                RaisePropertyChanged(() => EventBookedProducts);
            }
        }

        public ObservableCollection<GridViewDataColumn> ColumnCollection
        {
            get { return _columnCollection; }
            set
            {
                _columnCollection = value;
                RaisePropertyChanged(() => ColumnCollection);
            }
        }

        public ObservableCollection<ForwardBookModel> ForwardBook
        {
            get { return _forwardBook; }
            set
            {
                _forwardBook = value;
                RaisePropertyChanged(() => ForwardBook);
            }
        }

        public bool IncVATOption
        {
            get { return _incVATOption; }
            set
            {
                if (_incVATOption == value) return;
                _incVATOption = value;
                RaisePropertyChanged(() => IncVATOption);
                RefreshForwardBookings();
            }
        }

        public bool ExVATOption
        {
            get { return _exVATOption; }
            set
            {
                if (_exVATOption == value) return;
                _exVATOption = value;
                RaisePropertyChanged(() => ExVATOption);
                RefreshForwardBookings();
            }
        }

        public bool DepartmentOption
        {
            get { return _departmentOption; }
            set
            {
                if (_departmentOption == value) return;
                _departmentOption = value;
                RaisePropertyChanged(() => DepartmentOption);
                RefreshForwardBookings();
            }
        }

        public bool GroupOption
        {
            get { return _groupOption; }
            set
            {
                if (_groupOption == value) return;
                _groupOption = value;
                RaisePropertyChanged(() => GroupOption);
                RefreshForwardBookings();
            }
        }

        public bool EnquiriesOption
        {
            get { return _enquiriesOption; }
            set
            {
                if (_enquiriesOption == value) return;
                _enquiriesOption = value;
                RaisePropertyChanged(() => EnquiriesOption);
                RaisePropertyChanged(() => StatusItems);
                RefreshForwardBookings();
            }
        }

        public bool ProvisionalOption
        {
            get { return _provisionalOption; }
            set
            {
                if (_provisionalOption == value) return;
                _provisionalOption = value;
                RaisePropertyChanged(() => ProvisionalOption);
                RaisePropertyChanged(() => StatusItems);
                RefreshForwardBookings();
            }
        }

        public bool ConfirmedOption
        {
            get { return _confirmedOption; }
            set
            {
                if (_confirmedOption == value) return;
                _confirmedOption = value;
                RaisePropertyChanged(() => ConfirmedOption);
                RaisePropertyChanged(() => StatusItems);
                RefreshForwardBookings();
            }
        }

        public bool InvoicedOption
        {
            get { return _invoicedOption; }
            set
            {
                if (_invoicedOption == value) return;
                _invoicedOption = value;
                RaisePropertyChanged(() => InvoicedOption);
                RefreshForwardBookings();
                RaisePropertyChanged(() => StatusItems);
            }
        }

        public bool CancelledOption
        {
            get { return _cancelledOption; }
            set
            {
                if (_cancelledOption == value) return;
                _cancelledOption = value;
                RaisePropertyChanged(() => CancelledOption);
                RaisePropertyChanged(() => StatusItems);
                RefreshForwardBookings();
            }
        }

        public bool MinimizedOption
        {
            get { return _minimizedOption; }
            set
            {
                if (_minimizedOption == value) return;
                _minimizedOption = value;
                RaisePropertyChanged(() => MinimizedOption);
                RefreshForwardBookings();
            }
        }

        public bool IncMonthlyTotalsOption
        {
            get { return _incMonthlyTotalsOption; }
            set
            {
                if (_incMonthlyTotalsOption == value) return;
                _incMonthlyTotalsOption = value;
                RaisePropertyChanged(() => IncMonthlyTotalsOption);
                RefreshForwardBookings();
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public ForwardBookViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();

            _allEventBookedProducts = new List<EventBookedProductModel>();
            SaveCommand = new RelayCommand(SaveCommandExecuted);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
        }

        #endregion

        #region Methods

        private void SetEndDateGroup(int endDateGroupValue)
        {
            SetEndDateGroup(false);
            switch (endDateGroupValue)
            {
                case 0:
                    IsTodayEndChecked = true;
                    break;
                case 1:
                    IsEndOfCurrentMonthChecked = true;
                    break;
                case 2:
                    IsEndOfNextMonthChecked = true;
                    break;
                case 3:
                    IsEndOfCurrentYearChecked = true;
                    break;
                case 4:
                    IsCurrentDateForwardChecked = true;
                    break;
                default:
                    break;
            }
        }

        private void SetEndDateGroup(bool value)
        {
            IsTodayEndChecked = value;
            IsEndOfCurrentMonthChecked = value;
            IsEndOfNextMonthChecked = value;
            IsEndOfCurrentYearChecked = value;
            IsCurrentDateForwardChecked = value;
        }

        private void SetStartDateGroup(int startDateGroupValue)
        {
            SetStartDateGroup(false);
            switch (startDateGroupValue)
            {
                case 0:
                    IsTodayChecked = true;
                    break;
                case 1:
                    IsYesterdayChecked = true;
                    break;
                case 2:
                    IsStartOfCurrentMonthChecked = true;
                    break;
                case 3:
                    IsStartOfPreviousMonthChecked = true;
                    break;
                case 4:
                    IsStartOfCurrentYearChecked = true;
                    break;
                case 5:
                    IsCurrentDateBackwardChecked = true;
                    break;
                default:
                    break;
            }
        }

        private void SetStartDateGroup(bool value)
        {
            IsTodayChecked = value;
            IsYesterdayChecked = value;
            IsStartOfCurrentMonthChecked = value;
            IsStartOfPreviousMonthChecked = value;
            IsStartOfCurrentYearChecked = value;
            IsCurrentDateBackwardChecked = value;
        }

        public void SetDescriptionActivated(bool value)
        {
            _isDescriptionActivated = value;
        }
        public async void LoadData()
        {
            IsBusy = true;
            if (_isDataLoading) return;
            _isDataLoading = true;
            LoadOptions();

            EndDate = EndDateOption;
            StartDate = StartDateOption;

            var rates = await _reportsDataUnit.ProductVatRatesRepository.GetAllAsync();
            ProductVATRates = rates.OrderBy(x => x.Rate);
            ProductGroups = await _reportsDataUnit.ProductGroupsRepository.GetAllAsync(x => x.GroupName != "Blank");
            ProductDepartments = await _reportsDataUnit.ProductDepartmentsRepository.GetAllAsync(x => x.Department != "Blank");
            var bookedProducts = await _reportsDataUnit.EventBookedProductsRepository.GetAllAsyncWithIncludeForwardBook(x => !x.Event.IsDeleted && x.Event.ShowInForwardBook && x.Event.Date > StartDate && x.Event.Date < EndDate);

            _allEventBookedProducts = new List<EventBookedProductModel>(bookedProducts.Select(x => new EventBookedProductModel(x)));
            EventBookedProducts = new ObservableCollection<EventBookedProductModel>(_allEventBookedProducts);

            ForwardBook = new ObservableCollection<ForwardBookModel>();
            ColumnCollection = new ObservableCollection<GridViewDataColumn>();

            EventBookedProducts = new ObservableCollection<EventBookedProductModel>(_allEventBookedProducts.Where(x => StatusItems.Contains(x.EventStatus)));
            EventBookedProducts.ForEach(p => p.GroupByProductGroup = GroupOption);
            EventBookedProducts.ForEach(p => p.IncVat = IncVATOption);

            _isDataLoading = false;

            IsBusy = false;
        }



        private async void RefreshForwardBookings()
        {
            if (!_isDescriptionActivated) return;

            if (_isDataLoading || _isRefreshing) return;

            _isRefreshing = true;

            ForwardBook = new ObservableCollection<ForwardBookModel>();
            ColumnCollection = new ObservableCollection<GridViewDataColumn>();

            IsBusy = true;

            _reportsDataUnit.EventsRepository.Refresh();
            _reportsDataUnit.EventBookedProductsRepository.Refresh();
            var bookedProducts = await _reportsDataUnit.EventBookedProductsRepository.GetAllAsyncWithIncludeForwardBook(x => !x.Event.IsDeleted && x.Event.ShowInForwardBook && x.Event.Date > StartDate && x.Event.Date < EndDate);
            _allEventBookedProducts = new List<EventBookedProductModel>(bookedProducts.Select(x => new EventBookedProductModel(x)));

            EventBookedProducts = new ObservableCollection<EventBookedProductModel>(_allEventBookedProducts.Where(x => StatusItems.Contains(x.EventStatus)));
            EventBookedProducts.ForEach(p => p.GroupByProductGroup = GroupOption);
            EventBookedProducts.ForEach(p => p.IncVat = IncVATOption);

            _isRefreshing = false;

            IsBusy = false;
        }

        public void LoadOptions()
        {
            StartDateOption = Properties.Settings.Default.StartDateOption;
            EndDateOption = Properties.Settings.Default.EndDateOption;
            IncVATOption = Properties.Settings.Default.IncVATOption;
            ExVATOption = Properties.Settings.Default.ExVATOption;
            DepartmentOption = Properties.Settings.Default.DepartmentOption;
            GroupOption = Properties.Settings.Default.GroupOption;
            EnquiriesOption = Properties.Settings.Default.EnquiriesOption;
            ProvisionalOption = Properties.Settings.Default.ProvisionalOption;
            ConfirmedOption = Properties.Settings.Default.ConfirmedOption;
            InvoicedOption = Properties.Settings.Default.InvoicedOption;
            CancelledOption = Properties.Settings.Default.CancelledOption;
            MinimizedOption = Properties.Settings.Default.MinimizedOption;
            IncMonthlyTotalsOption = Properties.Settings.Default.IncMonthlyTotalsOption;
            DaysForward = Properties.Settings.Default.DaysForward;
            DaysBackward = Properties.Settings.Default.DaysBackward;
            SetStartDateGroup(Properties.Settings.Default.StartDateGroupValue);
            SetEndDateGroup(Properties.Settings.Default.EndDateGroupValue);
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            Properties.Settings.Default.StartDateOption = StartDateOption;
            Properties.Settings.Default.EndDateOption = EndDateOption;
            Properties.Settings.Default.IncVATOption = IncVATOption;
            Properties.Settings.Default.ExVATOption = ExVATOption;
            Properties.Settings.Default.DepartmentOption = DepartmentOption;
            Properties.Settings.Default.GroupOption = GroupOption;
            Properties.Settings.Default.EnquiriesOption = EnquiriesOption;
            Properties.Settings.Default.ProvisionalOption = ProvisionalOption;
            Properties.Settings.Default.ConfirmedOption = ConfirmedOption;
            Properties.Settings.Default.InvoicedOption = InvoicedOption;
            Properties.Settings.Default.CancelledOption = CancelledOption;
            Properties.Settings.Default.MinimizedOption = MinimizedOption;
            Properties.Settings.Default.IncMonthlyTotalsOption = IncMonthlyTotalsOption;
            Properties.Settings.Default.StartDateGroupValue = StartDateGroupValue;
            Properties.Settings.Default.EndDateGroupValue = EndDateGroupValue;
            Properties.Settings.Default.DaysForward = DaysForward;
            Properties.Settings.Default.DaysBackward = DaysBackward;

            Properties.Settings.Default.Save();
        }

        private void CancelCommandExecuted()
        {
            LoadOptions();
        }

        #endregion
    }
}
