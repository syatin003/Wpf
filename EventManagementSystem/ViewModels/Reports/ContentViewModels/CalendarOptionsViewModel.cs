using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Helpers;
using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Unity;
using System.ComponentModel;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class CalendarOptionsViewModel : EventManagementSystem.Core.ViewModels.ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private List<EventTypeModel> _allEventTypes;
        private ObservableCollection<CheckedListItem<EventTypeModel>> _checkedEventTypes;
        private System.Collections.Specialized.NameValueCollection _calendarEventTypes;

        private bool _isBusy;

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
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public CalendarOptionsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted, CancelCommandCanExecute);
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            Properties.Settings.Default.IncPlacesOptionCalender = IncPlaces;
            Properties.Settings.Default.IncPrimaryContactOptionCalender = IncPrimaryContact;
            Properties.Settings.Default.IncStatusOptionCalender = IncStatus;
            Properties.Settings.Default.IncEmailOptionCalender = IncEmail;
            Properties.Settings.Default.IncTelNumbersOptionCalender = IncTelNumbers;
            Properties.Settings.Default.IncChangesOptionCalender = IncChanges;
            Properties.Settings.Default.IncStartTimeOptionCalender = IncStartTime;
            Properties.Settings.Default.IncEventDateOptionCalender = IncEventDate;
            Properties.Settings.Default.CalendarEventTypes = CalendarEventTypes;

            Properties.Settings.Default.Save();
        }

        private bool SaveCommandCanExecute()
        {
            return true;
        }

        private void CancelCommandExecuted()
        {
            LoadOptions();
        }

        private bool CancelCommandCanExecute()
        {
            return true;
        }

        #endregion

        #region Methods

        public void ResetOptions()
        {
            LoadOptions();
        }

        public async void LoadOptions()
        {
            IncPlaces = Properties.Settings.Default.IncPlacesOptionCalender;
            IncPrimaryContact = Properties.Settings.Default.IncPrimaryContactOptionCalender;
            IncStatus = Properties.Settings.Default.IncStatusOptionCalender;
            IncEmail = Properties.Settings.Default.IncEmailOptionCalender;
            IncTelNumbers = Properties.Settings.Default.IncTelNumbersOptionCalender;
            IncChanges = Properties.Settings.Default.IncChangesOptionCalender;
            IncStartTime = Properties.Settings.Default.IncStartTimeOptionCalender;
            IncEventDate = Properties.Settings.Default.IncEventDateOptionCalender;
            CalendarEventTypes =new System.Collections.Specialized.NameValueCollection(Properties.Settings.Default.CalendarEventTypes);

            var types = await _adminDataUnit.EventTypesRepository.GetAllAsync();
            _allEventTypes = new List<EventTypeModel>(types.OrderBy(x => x.Name).Select(x => new EventTypeModel(x)));

            var isExist = !string.IsNullOrEmpty(CalendarEventTypes.ToString());
            if (!isExist)
            {
                foreach (var type in _allEventTypes)
                {
                    CalendarEventTypes = new System.Collections.Specialized.NameValueCollection();
                    CalendarEventTypes.Add("EventType" + type.EventType.ID, "true");
                }
            }
            else
            {
                foreach (var type in _allEventTypes)
                {
                    var propEventType = CalendarEventTypes["EventType" + type.EventType.ID];
                    if (propEventType == null)
                        CreateProperty(type.EventType.ID);
                }
            }
            RefreshEventTypes();
        }

        private void CreateProperty(Guid propertyName)
        {
            CalendarEventTypes.Add("EventType" + propertyName, "true");
        }

        private void RefreshEventTypes()
        {
            var types = new List<CheckedListItem<EventTypeModel>>();

            foreach (var type in _allEventTypes)
            {
                var propertyName = "EventType" + type.EventType.ID;
                var isChecked = false;
                var propEventType = CalendarEventTypes[propertyName];
                if (propEventType != null)
                {
                    isChecked = Convert.ToBoolean(propEventType);
                }

                var item = new CheckedListItem<EventTypeModel>(type, isChecked);
                item.PropertyChanged += ItemOnPropertyChanged;
                types.Add(item);
            }

            CheckedEventTypes = new ObservableCollection<CheckedListItem<EventTypeModel>>(types);

        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var item = sender as CheckedListItem<EventTypeModel>;

            var propEventType = CalendarEventTypes["EventType" + item.Item.EventType.ID];
            if (propEventType != null)
            {
                if (item.IsChecked)
                    CalendarEventTypes["EventType" + item.Item.EventType.ID] = "true";
                else
                    CalendarEventTypes["EventType" + item.Item.EventType.ID] = "false";
            }
        }

        #endregion
    }
}
