using System;
using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.AlternativeContacts
{
    public class AddAlternativeContactViewModel : ViewModelBase
    {
        #region Fields

        private ContactModel _contact;
        private readonly EventModel _event;
        private readonly IEventDataUnit _eventsDataUnit;

        #endregion

        #region Properties

        public ContactModel Contact
        {
            get { return _contact; }
            set
            {
                if (_contact == value) return;
                _contact = value;
                RaisePropertyChanged(() => Contact);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }

        public EventContact EventContact { get; set; } // Used in AlternativeContactts

        #endregion

        #region Constructor

        public AddAlternativeContactViewModel(EventModel eventModel)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);

            AddContact();
        }

        #endregion

        #region Methods

        private void AddContact()
        {
            Contact = new ContactModel(new Contact()
            {
                ID = Guid.NewGuid(),
            });

            Contact.PropertyChanged += ContactOnPropertyChanged;
        }

        private void ContactOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void SubmitCommandExecuted()
        {
            EventContact = new EventContact()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Contact = Contact.Contact
            };

            _eventsDataUnit.ContactsRepository.Add(Contact.Contact);
            _eventsDataUnit.EventContactsRepository.Add(EventContact);
        }

        private bool SubmitCommandCanExecute()
        {
            return !Contact.HasErrors;
        }

        #endregion
    }
}