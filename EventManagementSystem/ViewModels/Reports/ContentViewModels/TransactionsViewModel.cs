using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using System.Windows.Threading;
using System.Windows.Data;
using System.Diagnostics;
using EventManagementSystem.Core.Commands;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class TransactionsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private List<TillTransaction> _allTransactions;
        private bool _isBusy;
        private DateTime _startDate;
        private DateTime _endDate;
        private ObservableCollection<TillTransaction> _transactions;
        private bool _isDataLoadedOnce;
        private bool _isDirty;

        #endregion

        #region Properties

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                Properties.Settings.Default.StartDateOptionTransactions = _startDate;
                RaisePropertyChanged(() => StartDate);
                //if (_isDataLoadedOnce)
                //    RefreshTransactionByDate(_startDate, _endDate);
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                Properties.Settings.Default.EndDateOptionTransactions = _endDate;
                RaisePropertyChanged(() => EndDate);
                //if (_isDataLoadedOnce)
                //    RefreshTransactionByDate(_startDate, _endDate);
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

        public ObservableCollection<TillTransaction> Transactions
        {
            get { return _transactions; }
            set
            {
                if (_transactions == value) return;
                _transactions = value;
                RaisePropertyChanged(() => Transactions);
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

        public TransactionsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();

            GoButtonCommand = new RelayCommand(GoButtonCommandExecuted, GoButtonCommandCanExecute);
            this.PropertyChanged += TransactionsViewModel_PropertyChanged;
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            IsBusy = true;

            LoadOptions();

            RefreshTransactionByDate(StartDate, EndDate);

            _isDataLoadedOnce = true;
        }

        private async void RefreshTransactionByDate(DateTime startDate, DateTime endDate)
        {
            IsBusy = true;

            _allTransactions = await _reportsDataUnit.TillTransactionsRepository.GetTransactionsWithRelatedData(startDate, endDate);
            Transactions = new ObservableCollection<TillTransaction>(_allTransactions);

            IsBusy = false;
        }

        private void LoadOptions()
        {
            if (Properties.Settings.Default.StartDateOptionTransactions == default(DateTime) && Properties.Settings.Default.EndDateOptionTransactions == default(DateTime))
            {
                Properties.Settings.Default.StartDateOptionTransactions = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                Properties.Settings.Default.EndDateOptionTransactions = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            }

            StartDate = Properties.Settings.Default.StartDateOptionTransactions;
            EndDate = Properties.Settings.Default.EndDateOptionTransactions;
        }

        private void TransactionsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_isDataLoadedOnce)
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
            RefreshTransactionByDate(_startDate, _endDate);

            IsDirty = false;
        }

        #endregion
    }
}
