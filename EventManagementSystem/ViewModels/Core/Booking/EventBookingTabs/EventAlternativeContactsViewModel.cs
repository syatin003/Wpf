using System;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Core.Contacts;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.AlternativeContacts;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventAlternativeContactsViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;
        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;

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

        public EventModel Event
        {
            get { return _event; }
            set
            {
                if (_event == value) return;
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }

        public RelayCommand AddAlternativeContactCommand { get; private set; }
        public RelayCommand<EventContact> EditAlternativeContactCommand { get; private set; }
        public RelayCommand FindContactCommand { get; private set; }
        public RelayCommand<EventContact> DeleteAlternativeContactCommand { get; private set; }

        #endregion

        #region Constructor

        public EventAlternativeContactsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            AddAlternativeContactCommand = new RelayCommand(AddAlternativeContactCommandExecuted);
            FindContactCommand = new RelayCommand(FindContactCommandExecuted);
            DeleteAlternativeContactCommand = new RelayCommand<EventContact>(DeleteAlternativeContactCommandExecuted);
            EditAlternativeContactCommand = new RelayCommand<EventContact>(EditAlternativeContactCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadEventData()
        {
            IsBusy = true;

            if (!_event.EventContacts.Any())
            {
                var contacts = await _eventsDataUnit.EventContactsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                _event.EventContacts = new ObservableCollection<EventContact>(contacts);
            }
            else
            {
                //var desiredEvent = await _eventsDataUnit.EventsRepository.GetUpdatedEvent(_event.Event.ID);

                //if (desiredEvent != null && desiredEvent.LastEditDate != null && _event.LoadedTime < desiredEvent.LastEditDate)
                //{
                //    _eventsDataUnit.EventContactsRepository.Refresh();
                //    var contacts = await _eventsDataUnit.EventContactsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                //    _event.EventContacts = new ObservableCollection<EventContact>(contacts);
                //}
            }

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void AddAlternativeContactCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddAlternativeContactView(_event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.EventContacts.Add(window.ViewModel.EventContact);
            }
        }

        private void FindContactCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new ContactsListView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult == null || window.DialogResult != true || window.ViewModel.SelectedContact == null) return;

            // For database
            var eventContact = new EventContact()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                ContactID = window.ViewModel.SelectedContact.Contact.ID
            };

            // For event data contex
            var linkedContact = new EventContact()
            {
                ID = eventContact.ID,
                EventID = eventContact.EventID,
                Contact = window.ViewModel.SelectedContact.Contact
            };

            _event.EventContacts.Add(linkedContact);
            _eventsDataUnit.EventContactsRepository.Add(eventContact);
        }

        private void DeleteAlternativeContactCommandExecuted(EventContact item)
        {
            _event.EventContacts.Remove(item);
            _eventsDataUnit.EventContactsRepository.Delete(item);
        }

        private void EditAlternativeContactCommandExecuted(EventContact obj)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddContactView(new ContactModel(obj.Contact));
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        #endregion
    }
}