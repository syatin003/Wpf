using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Unity;
using EventManagementSystem.Helpers;
using System.ComponentModel;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class CalendarDescriptionViewModel : EventManagementSystem.Core.ViewModels.ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private readonly IAdminDataUnit _adminDataUnit;
        private List<EventTypeModel> _allEventTypes;
        private ObservableCollection<CheckedListItem<EventTypeModel>> _checkedEventTypes;
        private bool _isBusy;
        private DateTime _startDate;
        private DateTime _endDate;
        private ObservableCollection<EventModel> _events;

        private List<EventModel> _allEvents;
        private ObservableCollection<string> _enabledItems;
        private System.Collections.Specialized.NameValueCollection _calendarEventTypes;

        private bool _incPlaces;
        private bool _incPrimaryContact;
        private bool _incStatus;
        private bool _incEmail;
        private bool _incTelNumbers;
        private bool _incChanges;
        private bool _incStartTime;
        private bool _incEventDate;

        #endregion Fields

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
                UpdateCalenderDataRange(_allEvents, StartDate, EndDate);
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
                UpdateCalenderDataRange(_allEvents, StartDate, EndDate);
            }
        }
        public bool IncPlaces
        {
            get { return _incPlaces; }
            set
            {
                if (_incPlaces == value) return;
                _incPlaces = value;
                RaisePropertyChanged(() => IncPlaces);
            }
        }

        public bool IncPrimaryContact
        {
            get { return _incPrimaryContact; }
            set
            {
                if (_incPrimaryContact == value) return;
                _incPrimaryContact = value;
                RaisePropertyChanged(() => IncPrimaryContact);
            }
        }

        public bool IncStatus
        {
            get { return _incStatus; }
            set
            {
                if (_incStatus == value) return;
                _incStatus = value;
                RaisePropertyChanged(() => IncStatus);
            }
        }

        public bool IncEmail
        {
            get { return _incEmail; }
            set
            {
                if (_incEmail == value) return;
                _incEmail = value;
                RaisePropertyChanged(() => IncEmail);
            }
        }

        public bool IncTelNumbers
        {
            get { return _incTelNumbers; }
            set
            {
                if (_incTelNumbers == value) return;
                _incTelNumbers = value;
                RaisePropertyChanged(() => IncTelNumbers);
            }
        }

        public bool IncChanges
        {
            get { return _incChanges; }
            set
            {
                if (_incChanges == value) return;
                _incChanges = value;
                RaisePropertyChanged(() => IncChanges);
            }
        }

        public bool IncStartTime
        {
            get { return _incStartTime; }
            set
            {
                if (_incStartTime == value) return;
                _incStartTime = value;
                RaisePropertyChanged(() => IncStartTime);
            }
        }

        public bool IncEventDate
        {
            get { return _incEventDate; }
            set
            {
                if (_incEventDate == value) return;
                _incEventDate = value;
                RaisePropertyChanged(() => IncEventDate);
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

        public ObservableCollection<string> EnabledItems
        {
            get { return _enabledItems; }
            set
            {
                if (_enabledItems == value) return;
                _enabledItems = value;
                RaisePropertyChanged(() => EnabledItems);
            }
        }

        public System.Collections.Specialized.NameValueCollection CalendarEventTypes
        {
            get { return _calendarEventTypes; }
            set
            {
                if (_calendarEventTypes == value) return;
                _calendarEventTypes = value;
                RaisePropertyChanged(() => CalendarEventTypes);
            }
        }

        #endregion Properties

        #region Constructor

        public CalendarDescriptionViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            _allEvents = new List<EventModel>();
            EnabledItems = new ObservableCollection<string>();
        }

        #endregion Constructor

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            LoadOptions();


            var events = await _reportsDataUnit.EventsRepository.GetLightEventsAsync(x => !x.IsDeleted);
            _allEvents = new List<EventModel>(events.OrderBy(x => x.Date).Select(x => new EventModel(x)));

            var types = await _adminDataUnit.EventTypesRepository.GetAllAsync();
            _allEventTypes = new List<EventTypeModel>(types.OrderBy(x => x.Name).Select(x => new EventTypeModel(x)));

            RefreshEventTypes(_allEventTypes);

            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date.AddDays(31);
            UpdateCalenderDataRange(_allEvents, StartDate, EndDate);

            IsBusy = false;
        }

        public void UpdateCalenderDataRange(List<EventModel> allEvents, DateTime startDate, DateTime endDate)
        {
            if (EnabledItems.Count > 0)
                Events = new ObservableCollection<EventModel>(allEvents.Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date && EnabledItems.Contains(x.EventType.Name)).OrderBy(x => x.Date));
            else
                Events = new ObservableCollection<EventModel>();

        }

        public void LoadOptions()
        {
            IncPlaces = Properties.Settings.Default.IncPlacesOptionCalender;
            IncPrimaryContact = Properties.Settings.Default.IncPrimaryContactOptionCalender;
            IncStatus = Properties.Settings.Default.IncStatusOptionCalender;
            IncEmail = Properties.Settings.Default.IncEmailOptionCalender;
            IncTelNumbers = Properties.Settings.Default.IncTelNumbersOptionCalender;
            IncChanges = Properties.Settings.Default.IncChangesOptionCalender;
            IncStartTime = Properties.Settings.Default.IncStartTimeOptionCalender;
            IncEventDate = Properties.Settings.Default.IncEventDateOptionCalender;
            CalendarEventTypes = Properties.Settings.Default.CalendarEventTypes;
        }
        public void RefreshEventTypes(List<EventTypeModel> allEventTypes)
        {
            var types = new List<CheckedListItem<EventTypeModel>>();

            foreach (var type in allEventTypes)
            {
                var isChecked = true;
                if (!string.IsNullOrEmpty(CalendarEventTypes.ToString()))
                {
                    var propEventType = CalendarEventTypes["EventType" + type.EventType.ID];
                    if (propEventType != null)
                    {
                        isChecked = Convert.ToBoolean(propEventType);
                    }
                }
                var item = new CheckedListItem<EventTypeModel>(type, isChecked);
                item.PropertyChanged += ItemOnPropertyChanged;

                types.Add(item);
            }
            CheckedEventTypes = new ObservableCollection<CheckedListItem<EventTypeModel>>(types);
            EnabledItems = new ObservableCollection<string>();
            foreach (var item in CheckedEventTypes)
            {
                if (item.IsChecked)
                {
                    EnabledItems.Add(item.Item.EventType.Name);
                }
            }
        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var item = sender as CheckedListItem<EventTypeModel>;
            if (item != null && item.IsChecked)
                EnabledItems.Add(item.Item.EventType.Name);
            else if (item != null) EnabledItems.Remove(item.Item.EventType.Name);
            UpdateCalenderDataRange(_allEvents, StartDate, EndDate);
        }
        #endregion Methods
    }
}
