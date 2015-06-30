using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models.Custom;
using Microsoft.Practices.Unity;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Events;

namespace EventManagementSystem.ViewModels.Events
{
    public class UpdatesViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventsModuleDataUnit;
        private bool _isBusy;
        private ObservableCollection<EventUpdateModel> _alleventUpdates;
        private DateTime _selectedDate;
        private List<EventUpdate> _allEventUpdates;
        private ObservableCollection<EventUpdate> _eventUpdates;
        private bool _isDataLoadedOnce;

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

        public ObservableCollection<EventUpdateModel> AllEventUpdates
        {
            get
            {
                return _alleventUpdates;
            }
            set
            {
                if (_alleventUpdates == value) return;
                _alleventUpdates = value;
                RaisePropertyChanged(() => AllEventUpdates);
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                RaisePropertyChanged(() => SelectedDate);
                if (IsDataLoadedOnce)
                    OnSelectedDateChanged();
            }
        }
        public ObservableCollection<EventUpdate> EventUpdates
        {
            get
            {
                return _eventUpdates;
            }
            set
            {
                if (_eventUpdates == value) return;
                _eventUpdates = value;
                RaisePropertyChanged(() => EventUpdates);
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

        public RelayCommand<EventUpdateModel> ShowHistoryCommand { get; private set; }

        #endregion

        #region Constructor

        public UpdatesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsModuleDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
            ShowHistoryCommand = new RelayCommand<EventUpdateModel>(ShowHistoryCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, 0, 0, 0);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            _eventsModuleDataUnit.EventUpdatesRepository.Refresh();

            var updates = await _eventsModuleDataUnit.EventUpdatesRepository.GetEventUpdatesByDate(startDate, endDate);
            _allEventUpdates = new List<EventUpdate>(updates.OrderByDescending(eventUpdate => eventUpdate.Date));
            EventUpdates = new ObservableCollection<EventUpdate>(_allEventUpdates.Where(x => x.Field != "Notes"));
            AllEventUpdates = new ObservableCollection<EventUpdateModel>(EventUpdates.Select(x => new EventUpdateModel(x)));
            ProcessNotesUpdates();

            IsBusy = false;

            IsDataLoadedOnce = true;
        }

        private void ProcessNotesUpdates()
        {
            var updatesgroups = _allEventUpdates.Where(x => x.Field == "Notes").OrderByDescending(x => x.Date).GroupBy(x => x.ItemId);

            foreach (var updatesgroup in updatesgroups)
            {
                var updateHiistList = updatesgroup.Select(eventUpdate => new UpdatesHistoryModel()
              {
                  EventUpdate = eventUpdate

              }).ToList();

                var updateModel = new EventUpdateModel(updatesgroup.FirstOrDefault());
                if (updateModel.EventUpdate != null)
                {
                    updateModel.UpdatesHistory = updateHiistList.OrderBy(eventUpdate => eventUpdate.EventUpdate.Date).ToList();
                    AllEventUpdates.Add(updateModel);
                }
            }
            AllEventUpdates = new ObservableCollection<EventUpdateModel>(AllEventUpdates.OrderByDescending(eventUpdate => eventUpdate.EventUpdate.Date));
        }

        private async void OnSelectedDateChanged()
        {
            IsBusy = true;

            var startDate = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, 0, 0, 0);
            var endDate = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, 23, 59, 59);
            var updates = await _eventsModuleDataUnit.EventUpdatesRepository.GetEventUpdatesByDate(startDate, endDate);
            _allEventUpdates = new List<EventUpdate>(updates.OrderByDescending(eventUpdate => eventUpdate.Date));
            EventUpdates = new ObservableCollection<EventUpdate>(_allEventUpdates.Where(x => x.Field != "Notes"));
            AllEventUpdates = new ObservableCollection<EventUpdateModel>(EventUpdates.Select(x => new EventUpdateModel(x)));
            ProcessNotesUpdates();

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void ShowHistoryCommandExecuted(EventUpdateModel update)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new UpdatesHistoryView(update.UpdatesHistory);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        #endregion Commands
    }
}
