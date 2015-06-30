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
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    class AgedBalanceViewModel : ViewModelBase
    {

        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private ObservableCollection<EventModel> _events;
        private List<EventModel> _allEvents;

        private DateTime _endDate;

        private bool _isBusy;
        private DateTime _startDate;


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

        public RelayCommand ExportToCSVCommand { get; private set; }

        #endregion

        #region Constructor

        public AgedBalanceViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();
            _allEvents = new List<EventModel>();
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;
            var today = DateTime.Now;
            var todayMorning = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            //var events = await _reportsDataUnit.EventsRepository.GetLightEventsAsync(x => !x.IsDeleted);
            var events = await _reportsDataUnit.EventsRepository.GetEventsForReportsAsync(x => !x.IsDeleted && x.Date < todayMorning);

            _allEvents = new List<EventModel>(events.OrderBy(x => x.Date).Select(x => new EventModel(x, true)));
            Events = new ObservableCollection<EventModel>(_allEvents.Where(x => x.EventPrice != x.Paid));

            StartDate = new DateTime(DateTime.Now.Year - 3, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

            IsBusy = false;
        }

        #endregion
    }
}
