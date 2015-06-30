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
using EventManagementSystem.Enums.Reports;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    class RoundageAnalysisDescriptionViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;

        private ObservableCollection<EventModel> _events;
        private List<EventModel> _allEvents;

        private bool _isBusy;
        private DateTime _startDateOption;
        private DateTime _endDateOption;
        private DateTime _startDate;
        private DateTime _endDate;

        public bool _golfOnlyOption;
        public bool _cateringOnlyOption;
        public bool _bothGolfandCateringOption;
        public RoundAgeReportFilter _roundAgeFilter;

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

        private bool _isDataLoading;
        private bool _isRefreshing;
        private bool _isDescriptionActivated;

        #endregion

        #region Properties

        public RoundAgeReportFilter RoundAgeFilter
        {
            get
            {
                return _roundAgeFilter;
            }
            set
            {
                if (_roundAgeFilter == value) return;
                _roundAgeFilter = value;
                RaisePropertyChanged(() => RoundAgeFilter);
                RefreshRoundageAnalysisFilter();
            }
        }
        public bool GolfOnlyOption
        {
            get { return _golfOnlyOption; }
            set
            {
                if (_golfOnlyOption == value) return;
                _golfOnlyOption = value;
                if (_golfOnlyOption)
                    RoundAgeFilter = RoundAgeReportFilter.GolfOnly;
                RaisePropertyChanged(() => GolfOnlyOption);
            }
        }
        public bool CateringOnlyOption
        {
            get { return _cateringOnlyOption; }
            set
            {
                if (_cateringOnlyOption == value) return;
                _cateringOnlyOption = value;
                if (_cateringOnlyOption)
                    RoundAgeFilter = RoundAgeReportFilter.CateringOnly;
                RaisePropertyChanged(() => CateringOnlyOption);
            }
        }
        public bool BothGolfandCateringOption
        {
            get { return _bothGolfandCateringOption; }
            set
            {
                if (_bothGolfandCateringOption == value) return;
                _bothGolfandCateringOption = value;
                if (_bothGolfandCateringOption)
                    RoundAgeFilter = RoundAgeReportFilter.Both;
                RaisePropertyChanged(() => BothGolfandCateringOption);
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
                RefreshRoundageAnalysis();
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
                    RefreshRoundageAnalysis();
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

        public ObservableCollection<EventModel> Events
        {
            get { return _events; }
            set
            {
                if (_events == value) return;
                _events = value;
                RaisePropertyChanged(() => Events);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public RoundageAnalysisDescriptionViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();

            _allEvents = new List<EventModel>();
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

            StartDate = StartDateOption;
            EndDate = EndDateOption;

            _reportsDataUnit.EventsRepository.Refresh();
            var events = await _reportsDataUnit.EventsRepository.GetEventsForReportsAsync(thisEvent => !thisEvent.IsDeleted && thisEvent.Date >= StartDate && thisEvent.Date <= EndDate);
            _allEvents = new List<EventModel>(events.Select(x => new EventModel(x, true, RoundAgeFilter)));
            Events = new ObservableCollection<EventModel>(_allEvents);

            _isDataLoading = false;
            IsBusy = false;
        }

        private async void RefreshRoundageAnalysis()
        {
            if (!_isDescriptionActivated) return;

            if (_isDataLoading || _isRefreshing) return;

            _isRefreshing = true;
            IsBusy = true;

            _reportsDataUnit.EventsRepository.Refresh();
            var events = await _reportsDataUnit.EventsRepository.GetEventsForReportsAsync(thisEvent => !thisEvent.IsDeleted && thisEvent.Date >= StartDate && thisEvent.Date <= EndDate);
            _allEvents = new List<EventModel>(events.Select(x => new EventModel(x, true, RoundAgeFilter)));
            Events = new ObservableCollection<EventModel>(_allEvents);

            _isRefreshing = false;

            IsBusy = false;
        }
        private void RefreshRoundageAnalysisFilter()
        {
            _allEvents = new List<EventModel>(_allEvents.Select(x => new EventModel(x.Event, true, RoundAgeFilter)));
            Events = new ObservableCollection<EventModel>(_allEvents);
        }
        public void LoadOptions()
        {
            StartDateOption = Properties.Settings.Default.StartDateRoundAgeAnalysis;
            EndDateOption = Properties.Settings.Default.EndDateRoundAgeAnalysis;
            DaysForward = Properties.Settings.Default.DaysForwardRoundAgeAnalysis;
            DaysBackward = Properties.Settings.Default.DaysBackwardRoundAgeAnalysis;
            CateringOnlyOption = Properties.Settings.Default.IsCateringOnlyRoundAge;
            GolfOnlyOption = Properties.Settings.Default.IsGolfOnlyRoundAge;
            BothGolfandCateringOption = Properties.Settings.Default.IsBothGolfAndCateringRoundAge;
            SetStartDateGroup(Properties.Settings.Default.StartDateGroupValueRoundAgeAnalysis);
            SetEndDateGroup(Properties.Settings.Default.EndDateGroupValueRoundAgeAnalysis);
        }


        #endregion
    }
}
