using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.ContactManager.ContactManagerTabs;
using EventManagementSystem.Views.CRM;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs
{
    public class ActivityViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContactsDataUnit _contactsDataUnit;
        private readonly ICrmDataUnit _crmDataUnit;
        private bool _isBusy;
        private ContactModel _contactModel;
        private ObservableCollection<EventModel> _events;
        private List<EventModel> _allEvents;
        private List<EnquiryModel> _allEnquiries;
        private ObservableCollection<EventEnquiryModel> _activities;

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

        public ContactModel ContactModel
        {
            get { return _contactModel; }
            set
            {
                if (_contactModel == value) return;
                _contactModel = value;
                RaisePropertyChanged(() => ContactModel);
            }
        }

        public RelayCommand<EventEnquiryModel> OpenEventCommand { get; private set; }

        public ObservableCollection<EventEnquiryModel> Activities
        {
            get
            {
                return _activities;
            }
            set
            {
                if (_activities == value) return;
                _activities = value;
                RaisePropertyChanged(() => Activities);
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

        #endregion

        #region Constructors

        public ActivityViewModel(ContactModel model)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            _contactModel = model;

            OpenEventCommand = new RelayCommand<EventEnquiryModel>(OpenEventCommandExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            // Load events
            var events = await _contactsDataUnit.EventsRepository.GetLightEventsAsync(x => !x.IsDeleted
                && (x.ContactID == _contactModel.Contact.ID || x.EventContacts.Select(y => y.ContactID).Contains(_contactModel.Contact.ID)));

            _allEvents = events.OrderBy(x => x.Date).Select(x => new EventModel(x)).ToList();

            _allEvents.Where(x => x.PrimaryContact != null && x.PrimaryContact.Contact == _contactModel.Contact).ForEach(x => x.EventContactType = string.Format("{0} Primary Contact", x.EventType.Name));
            // _allEvents.Where(x => x.EventContacts.Select(y => y.Contact).Contains(_contactModel.Contact)).ForEach(x => x.EventContactType = string.Format("{0} Contact", x.EventType.Name)); // TODO: Fix event model

            Events = new ObservableCollection<EventModel>(_allEvents);
            Activities = new ObservableCollection<EventEnquiryModel>(_allEvents.Select(x => new EventEnquiryModel(x)));

            // Load enquiries
            var enquiries = await _contactsDataUnit.EnquiriesRepository.GetLightEnquiriesAsync(x => x.ContactID == _contactModel.Contact.ID);
            _allEnquiries = new List<EnquiryModel>(enquiries.OrderBy(x => x.Date).Select(x => new EnquiryModel(x)));

            _allEnquiries.ForEach(x => Activities.Add(new EventEnquiryModel(x)));

            IsBusy = false;
        }

        #endregion

        #region Commands

        private async void OpenEventCommandExecute(EventEnquiryModel model)
        {
            RaisePropertyChanged("DisableParentWindow");

            if (model.Event != null)
            {
                var window = new EventDetailsView(model.Event);
                window.ShowDialog();
            }
            else
            {
                var enquiries = await _crmDataUnit.EnquiriesRepository.GetLightEnquiriesAsync(x => x.ID == model.Enquiry.Enquiry.ID);
                var enquiryModel = new EnquiryModel(enquiries.FirstOrDefault());

                var window = new NewEnquiryView(enquiryModel);
                window.ShowDialog();
            }

            RaisePropertyChanged("EnableParentWindow");

        }

        #endregion
    }
}
