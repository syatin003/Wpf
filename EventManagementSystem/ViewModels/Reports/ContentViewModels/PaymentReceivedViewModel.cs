using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;


namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    class PaymentReceivedViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private ObservableCollection<EventPaymentModel> _eventPayments;
        private List<EventPaymentModel> _allPayments;

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

        private bool _isBusy;
        private bool _isReceiveDateChecked;


        #endregion

        #region Properties

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                RaisePropertyChanged(() => StartDate);
                UpdateEventsDataRange();
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
                UpdateEventsDataRange();
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

        public ObservableCollection<EventPaymentModel> EventPayments
        {
            get { return _eventPayments; }
            set
            {
                if (_eventPayments == value) return;
                _eventPayments = value;

                RaisePropertyChanged(() => EventPayments);
            }
        }

        public bool IsReceiveDateChecked
        {
            get { return _isReceiveDateChecked; }
            set
            {
                if (_isReceiveDateChecked == value) return;
                _isReceiveDateChecked = value;
                RaisePropertyChanged(() => IsReceiveDateChecked);
            }
        }

        public RelayCommand ExportToCSVCommand { get; private set; }

        #endregion

        #region Constructor


        public PaymentReceivedViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();

            _allPayments = new List<EventPaymentModel>();
        }
        #endregion

        #region Methods

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
        public async void LoadData()
        {
            IsBusy = true;

            var payments = await _reportsDataUnit.EventPaymentsRepository.GetAllAsync(x => !x.Event.IsDeleted);
            _allPayments = new List<EventPaymentModel>(payments.Select(x => new EventPaymentModel(x)));
            EventPayments = new ObservableCollection<EventPaymentModel>(_allPayments);

            ResetOptions();
            StartDate = StartDateOption;
            EndDate = EndDateOption;
            UpdateEventsDataRange();
            IsBusy = false;
        }

        public void ResetOptions()
        {

            StartDateOption = Properties.Settings.Default.StartDateInPaymentOption;
            EndDateOption = Properties.Settings.Default.EndDateInPaymentOption;
            DaysForward = Properties.Settings.Default.DaysForwardPayment;
            DaysBackward = Properties.Settings.Default.DaysBackwardPayment;
            SetStartDateGroup(Properties.Settings.Default.StartDateGroupValuePayment);
            SetEndDateGroup(Properties.Settings.Default.EndDateGroupValuePayment);
            IsReceiveDateChecked = Properties.Settings.Default.IsPaymentReceivedDateChecked;
        }

        private void UpdateEventsDataRange()
        {
            if (IsReceiveDateChecked)
                EventPayments = new ObservableCollection<EventPaymentModel>(_allPayments.Where(x => x.Date.Date >= StartDateOption.Date && x.Date.Date <= EndDateOption.Date));
            else
                EventPayments = new ObservableCollection<EventPaymentModel>(_allPayments.Where(x => x.EventPayment.Event.Date >= StartDateOption.Date && x.EventPayment.Event.Date <= EndDateOption.Date));
        }

        #endregion
    }
}
