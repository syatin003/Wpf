using System;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class DepositsReceivedOptionsViewModel : ViewModelBase
    {
        #region Fields

        private bool _isTodayChecked;
        private bool _isYesterdayChecked;
        private bool _isStartOfCurrentMonthChecked;
        private bool _isStartOfPreviousMonthChecked;
        private bool _isStartOfCurrentYearChecked;
        private bool _isCurrentDateBackwardChecked;

        private DateTime _startDate;
        private DateTime _endDate;

        private bool _isTodayEndChecked;
        private bool _isEndOfNextMonthChecked;
        private bool _isEndOfCurrentMonthChecked;
        private bool _isEndOfCurrentYearChecked;
        private bool _isCurrentDateForwardChecked;

        private bool _isReceivedDateChecked;

        private int _daysForward;
        private int _daysBackward;

        private int _startDateGroupValue;
        private int _endDateGroupValue;

        #endregion

        #region Properties

        public int DaysForward
        {
            get { return _daysForward; }
            set
            {
                if (_daysForward == value) return;
                _daysForward = value;
                RaisePropertyChanged(() => DaysForward);
                RaisePropertyChanged(() => EndDate);
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
                RaisePropertyChanged(() => StartDate);
            }
        }

        public DateTime StartDate
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
                return _startDate;
            }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                SetStartDateGroup(false);
                _startDateGroupValue = -1;
                RaisePropertyChanged(() => StartDate);
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
                RaisePropertyChanged(() => StartDate);
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
                RaisePropertyChanged(() => StartDate);
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
                RaisePropertyChanged(() => StartDate);
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
                RaisePropertyChanged(() => StartDate);
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
                RaisePropertyChanged(() => StartDate);
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
                RaisePropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
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
                return _endDate;
            }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                SetEndDateGroup(false);
                _endDateGroupValue = -1;
                RaisePropertyChanged(() => EndDate);
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
                RaisePropertyChanged(() => EndDate);
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
                RaisePropertyChanged(() => EndDate);
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
                RaisePropertyChanged(() => EndDate);
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
                RaisePropertyChanged(() => EndDate);
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
                RaisePropertyChanged(() => EndDate);
            }
        }

        public bool IsReceivedDateChecked
        {
            get
            { return _isReceivedDateChecked; }
            set
            {
                if (_isReceivedDateChecked == value) return;
                _isReceivedDateChecked = value;
                RaisePropertyChanged(() => IsReceivedDateChecked);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public DepositsReceivedOptionsViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted, CancelCommandCanExecute);
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            Properties.Settings.Default.StartDateInDepositsRecevedOption = StartDate;
            Properties.Settings.Default.EndDateInDepositsRecevedOption = EndDate;
            Properties.Settings.Default.StartDateGroupValueDepositReceived = StartDateGroupValue;
            Properties.Settings.Default.EndDateGroupValueDepositReceived = EndDateGroupValue;
            Properties.Settings.Default.DaysForwardDepositReceived = DaysForward;
            Properties.Settings.Default.DaysBackwardDepositReceived = DaysBackward;
            Properties.Settings.Default.IsReceivedDateChecked = IsReceivedDateChecked;

            Properties.Settings.Default.Save();
        }

        private bool SaveCommandCanExecute()
        {
            return true;
        }

        private void CancelCommandExecuted()
        {
            ResetOptions();
        }

        private bool CancelCommandCanExecute()
        {
            return true;
        }

        #endregion

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

        public void LoadOptions()
        {
            ResetOptions();
        }

        public void ResetOptions()
        {
            StartDate = Properties.Settings.Default.StartDateInDepositsRecevedOption;
            EndDate = Properties.Settings.Default.EndDateInDepositsRecevedOption;
            DaysForward = Properties.Settings.Default.DaysForwardDepositReceived;
            DaysBackward = Properties.Settings.Default.DaysBackwardDepositReceived;
            SetStartDateGroup(Properties.Settings.Default.StartDateGroupValueDepositReceived);
            SetEndDateGroup(Properties.Settings.Default.EndDateGroupValueDepositReceived);
            IsReceivedDateChecked = Properties.Settings.Default.IsReceivedDateChecked;
        }
    }
}
