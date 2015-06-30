using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    class CustomersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private ObservableCollection<EventModel> _events;
        private ObservableCollection<CustomerModel> _customers;

        private List<EventModel> _allEvents;

        private bool _isBusy;

        private bool _summariseEvents;
        private bool _incPrimaryContact;
        private bool _incAddress;
        private bool _incEmail;
        private bool _incTelNumbers;
        private bool _incPastBookings;
        private bool _incFutureBookings;
        private bool _incTotalCharged;
        private bool _incEventDate;
        private bool _incLastVisit;

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

        public ObservableCollection<CustomerModel> Customers
        {
            get { return _customers; }
            set
            {
                if (_customers == value) return;
                _customers = value;
                RaisePropertyChanged(() => Customers);
            }
        }

        public bool SummariseEvents
        {
            get { return _summariseEvents; }
            set
            {
                if (_summariseEvents == value) return;
                _summariseEvents = value;
                RaisePropertyChanged(() => SummariseEvents);
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

        public bool IncAddress
        {
            get { return _incAddress; }
            set
            {
                if (_incAddress == value) return;
                _incAddress = value;
                RaisePropertyChanged(() => IncAddress);
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

        public bool IncPastBookings
        {
            get { return _incPastBookings; }
            set
            {
                if (_incPastBookings == value) return;
                _incPastBookings = value;
                RaisePropertyChanged(() => IncPastBookings);
            }
        }

        public bool IncFutureBookings
        {
            get { return _incFutureBookings; }
            set
            {
                if (_incFutureBookings == value) return;
                _incFutureBookings = value;
                RaisePropertyChanged(() => IncFutureBookings);
            }
        }

        public bool IncTotalCharged
        {
            get { return _incTotalCharged; }
            set
            {
                if (_incTotalCharged == value) return;
                _incTotalCharged = value;
                RaisePropertyChanged(() => IncTotalCharged);
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

        public bool IncLastVisit
        {
            get { return _incLastVisit; }
            set
            {
                if (_incLastVisit == value) return;
                _incLastVisit = value;
                RaisePropertyChanged(() => IncLastVisit);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public CustomersViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();

            _allEvents = new List<EventModel>();
            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted, CancelCommandCanExecute);
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            Properties.Settings.Default.SummariseEvents = SummariseEvents;
            Properties.Settings.Default.IncPrimaryContact = IncPrimaryContact;
            Properties.Settings.Default.IncAddress = IncAddress;
            Properties.Settings.Default.IncEmail = IncEmail;
            Properties.Settings.Default.IncTelNumbers = IncTelNumbers;
            Properties.Settings.Default.IncPastBookings = IncPastBookings;
            Properties.Settings.Default.IncFutureBookings = IncFutureBookings;
            Properties.Settings.Default.IncTotalCharged = IncTotalCharged;
            Properties.Settings.Default.IncEventDate = IncEventDate;
            Properties.Settings.Default.IncLastVisit = IncLastVisit;
            
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

        #region Methods

        public void LoadData()
        {
            IsBusy = true;
            LoadOptions();
        }

        public void LoadOptions()
        {
            ResetOptions();
            OnLoadEvents();
        }

        public void ResetOptions()
        {
            SummariseEvents = Properties.Settings.Default.SummariseEvents;
            IncPrimaryContact = Properties.Settings.Default.IncPrimaryContact;
            IncAddress = Properties.Settings.Default.IncAddress;
            IncEmail = Properties.Settings.Default.IncEmail;
            IncTelNumbers = Properties.Settings.Default.IncTelNumbers;
            IncPastBookings = Properties.Settings.Default.IncPastBookings;
            IncFutureBookings = Properties.Settings.Default.IncFutureBookings;
            IncTotalCharged = Properties.Settings.Default.IncTotalCharged;
            IncEventDate = Properties.Settings.Default.IncEventDate;
            IncLastVisit = Properties.Settings.Default.IncLastVisit;
        }

        private async void OnLoadEvents()
        {
            var events = await _reportsDataUnit.EventsRepository.GetLightEventsAsync(x => !x.IsDeleted);
            _allEvents = new List<EventModel>(events.OrderBy(x => x.Date).Select(x => new EventModel(x)));
            Events = new ObservableCollection<EventModel>(_allEvents);

            Customers = new ObservableCollection<CustomerModel>();

            if (SummariseEvents)
            {
                LoadSummariseEvents();
            }
            else
            {
                LoadUnsummariseEvents();
            }

            IsBusy = false;
        }

        private void LoadSummariseEvents()
        {
            var distinctEvents = Events.GroupBy(x => new { x.Name, x.Event.Contact, x.Event.EventType }).Select(g => g.First()).ToList();
            
            foreach (var distinctEvent in distinctEvents)
            {
                var primaryContact = new Contact();
                string lastVisit = null;

                var eventDates = (from eventModel in Events where distinctEvent.Name == eventModel.Name && distinctEvent.Event.Contact == eventModel.Event.Contact && distinctEvent.EventType == eventModel.Event.EventType select eventModel.Event.Date).ToList();

                var eventPrice = Events.Where(eventModel => distinctEvent.Name == eventModel.Name && distinctEvent.Event.Contact == eventModel.Event.Contact && distinctEvent.EventType == eventModel.Event.EventType).Sum(eventModel => eventModel.EventPrice);


                if (distinctEvent.Event.Contact != null)
                    primaryContact = distinctEvent.Event.Contact;
                if (eventDates.Any(x => x.Date < DateTime.Now))
                    lastVisit = eventDates.Min().ToString();

                Customers.Add(new CustomerModel
                {
                    EventName = distinctEvent.Name,
                    PrimaryContact = new ContactModel(primaryContact),
                    EventType = distinctEvent.EventType.Name,
                    PastEvents = eventDates.Count(x => x.Date < DateTime.Now).ToString(),
                    Future = eventDates.Count(x => x.Date > DateTime.Now).ToString(),
                    TotalCharged = eventPrice,
                    EventDate = null,
                    LastVisit = lastVisit
                });
            }
        }

        private void LoadUnsummariseEvents()
        {
            foreach (var eventModel in Events)
            {
                Customers.Add(new CustomerModel
                {
                    EventName = eventModel.Name,
                    PrimaryContact = eventModel.PrimaryContact,
                    EventType = eventModel.EventType.Name,
                    PastEvents = null,
                    Future = null,
                    TotalCharged = eventModel.EventPrice,
                    EventDate = null,
                    LastVisit = null
                });
            }
        }

        #endregion


    }
}
