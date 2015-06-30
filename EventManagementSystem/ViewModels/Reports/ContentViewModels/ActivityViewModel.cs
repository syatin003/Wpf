using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class ActivityViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private bool _isBusy;
        private DateTime _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        private DateTime _endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        private ObservableCollection<ActivityModel> _activities;
        private List<ActivityModel> _allActivities;

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
                RaisePropertyChanged(() => StartDate);
                UpdateActivitiesDataRange();
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
                UpdateActivitiesDataRange();
            }
        }

        public ObservableCollection<ActivityModel> Activities
        {
            get { return _activities; }
            set
            {
                if (_activities == value) return;
                _activities = value;
                RaisePropertyChanged(() => Activities);
            }
        }

        #endregion

        #region Constructor

        public ActivityViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _reportsDataUnit.ActivitiesRepository.Refresh();
            var activities = await _reportsDataUnit.ActivitiesRepository.GetAllAsync();
            _allActivities = new List<ActivityModel>(activities.Select(x => new ActivityModel(x)));
            Activities = new ObservableCollection<ActivityModel>(_allActivities);

            UpdateActivitiesDataRange();

            IsBusy = false;
        }

        private void UpdateActivitiesDataRange()
        {
            Activities = new ObservableCollection<ActivityModel>(_allActivities.Where(x => x.Date.Date >= StartDate.Date && x.Date.Date <= EndDate.Date));
        }

        #endregion
    }
}
