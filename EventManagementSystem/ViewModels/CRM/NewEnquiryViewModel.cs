using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Core.Booking;
using EventManagementSystem.Views.Core.Booking;
using EventManagementSystem.Views.Core.Contacts;
using EventManagementSystem.Views.Events;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using EventManagementSystem.CommonObjects.Comparers;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.CRM
{
    public class NewEnquiryViewModel : ViewModelBase
    {
        #region Fields

        private readonly ICrmDataUnit _crmDataUnit;
        private readonly IEventDataUnit _eventDataUnit;
        private bool _isBusy;
        private ObservableCollection<EventStatus> _eventStatuses;
        private ObservableCollection<EventType> _eventTypes;
        private EnquiryModel _enquiry;
        private ObservableCollection<Enquiry> _alreadyCreatedEnquiries;
        private bool _isEditMode;
        private List<EventNoteType> _eventNoteTypes;
        private ObservableCollection<EnquiryStatus> _enquiryStatuses;
        private ObservableCollection<EnquiryReceiveMethod> _enquiryReceivedMethods;
        private ObservableCollection<User> _users;
        private ObservableCollection<Campaign> _campaigns;
        private ContactModel _contact;
        private EnquiryModel _originalEnquiry;
        private bool _isLocked;
        private string _lockedText;
        private bool _forceRefreshEnquiryData;

        private User _loggedUser;
        private User _assignedToUser;
        private EnquiryStatus _enquiryStatus;
        private EnquiryReceiveMethod _receivedMethod;
        private EventType _eventType;
        private EventStatus _eventStatus;
        private Campaign _campaign;
        #endregion

        #region Properties

        public bool ActivityChanged, FoolowUpsChanged;

        public User LoggedUser
        {
            get { return _loggedUser; }
            set
            {
                _loggedUser = value;
                RaisePropertyChanged(() => LoggedUser);
            }
        }
        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked == value) return;
                _isLocked = value;
                RaisePropertyChanged(() => IsLocked);
            }
        }

        public string LockedText
        {
            get { return _lockedText; }
            set
            {
                if (_lockedText == value) return;
                _lockedText = value;
                RaisePropertyChanged(() => LockedText);
            }
        }

        public Campaign Campaign
        {
            get { return _campaign; }
            set
            {
                _campaign = value;
                RaisePropertyChanged(() => Campaign);
            }
        }

        public EnquiryReceiveMethod ReceivedMethod
        {
            get { return _receivedMethod; }
            set
            {
                _receivedMethod = value;
                RaisePropertyChanged(() => ReceivedMethod);
                SubmitEventCommand.RaiseCanExecuteChanged();
            }
        }

        public EventStatus EventStatus
        {
            get { return _eventStatus; }
            set
            {
                _eventStatus = value;
                RaisePropertyChanged(() => EventStatus);
            }
        }

        

        public EnquiryStatus EnquiryStatus
        {
            get { return _enquiryStatus; }
            set
            {
                _enquiryStatus = value;
                RaisePropertyChanged(() => EnquiryStatus);
                SubmitEventCommand.RaiseCanExecuteChanged();
            }
        }

        public User AssignedToUser
        {
            get { return _assignedToUser; }
            set
            {
                _assignedToUser = value;
                RaisePropertyChanged(() => AssignedToUser);
                SubmitEventCommand.RaiseCanExecuteChanged();
            }
        }

        public bool AllowBooking
        {
            get
            {
                return _isEditMode && !Enquiry.IsSpecificEventTypeSelected;
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

        public ObservableCollection<EventStatus> EventStatuses
        {
            get { return _eventStatuses; }
            set
            {
                if (_eventStatuses == value) return;
                _eventStatuses = value;
                RaisePropertyChanged(() => EventStatuses);
            }
        }
        public EventType EventType
        {
            get { return _eventType; }
            set
            {
                _eventType = value;
                RaisePropertyChanged(() => EventType);
                RaisePropertyChanged(() => IsSpecificEventTypeSelected);
                SubmitEventCommand.RaiseCanExecuteChanged();

            }
        }
        public ObservableCollection<EventType> EventTypes
        {
            get { return _eventTypes; }
            set
            {
                if (_eventTypes == value) return;
                _eventTypes = value;
                RaisePropertyChanged(() => EventTypes);
            }
        }

        public EnquiryModel Enquiry
        {
            get { return _enquiry; }
            set
            {
                if (_enquiry == value) return;
                _enquiry = value;
                RaisePropertyChanged(() => Enquiry);
            }
        }

        public ContactModel Contact
        {
            get { return _contact; }
            set
            {
                if (_contact == value) return;
                _contact = value;
                RaisePropertyChanged(() => Contact);

                SetContactToEnquiry(value);
            }
        }

        public ObservableCollection<Enquiry> AlreadyCreatedEnquiries
        {
            get { return _alreadyCreatedEnquiries; }
            set
            {
                if (_alreadyCreatedEnquiries == value) return;
                _alreadyCreatedEnquiries = value;
                RaisePropertyChanged(() => AlreadyCreatedEnquiries);
            }
        }

        public ObservableCollection<EnquiryStatus> EnquiryStatuses
        {
            get { return _enquiryStatuses; }
            set
            {
                if (_enquiryStatuses == value) return;
                _enquiryStatuses = value;
                RaisePropertyChanged(() => EnquiryStatuses);
            }
        }

        public ObservableCollection<EnquiryReceiveMethod> EnquiryReceivedMethods
        {
            get { return _enquiryReceivedMethods; }
            set
            {
                if (_enquiryReceivedMethods == value) return;
                _enquiryReceivedMethods = value;
                RaisePropertyChanged(() => EnquiryReceivedMethods);
            }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                if (_users == value) return;
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public ObservableCollection<Campaign> Campaigns
        {
            get { return _campaigns; }
            set
            {
                if (_campaigns == value) return;
                _campaigns = value;
                RaisePropertyChanged(() => Campaigns);
            }
        }

        public object AutoCompleteBoxSelectedItem
        {
            set
            {
                if (value is Enquiry)
                {
                    var @enquiry = value as Enquiry;

                    _enquiry.Name = @enquiry.Name;
                    ProposePrimaryContact(@enquiry);
                }
            }
        }

        public bool IsSpecificEventTypeSelected
        {
            get
            {
                if (EventType != null &&
                    (EventType.Name == "Membership" || EventType.Name == "Employment"))
                {
                    Enquiry.IsSpecificEventTypeSelected = true;
                    RaisePropertyChanged(() => IsEnquiryAndDetailFilled);
                    return true;
                }
                Enquiry.IsSpecificEventTypeSelected = false;
                RaisePropertyChanged(() => IsEnquiryAndDetailFilled);
                return false;
            }
        }

        public RelayCommand ShowFindContactWindowCommand { get; private set; }
        public RelayCommand ShowAddContactWindowCommand { get; private set; }
        public RelayCommand EditPrimaryContactCommand { get; private set; }
        public RelayCommand SubmitEventCommand { get; private set; }
        public RelayCommand CancelEditingCommand { get; private set; }
        public RelayCommand ShowResourcesCommand { get; private set; }
        public RelayCommand BookCommand { get; private set; }

        public bool IsEnquiryAndDetailFilled
        {
            get
            {
                return (Enquiry != null) && !Enquiry.HasErrors;
            }
        }

        #endregion

        #region Constructors

        public NewEnquiryViewModel(EnquiryModel enquiryModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
            //_crmDataUnit.EnquiriesRepository.RevertAllChanges();
            ShowFindContactWindowCommand = new RelayCommand(ShowFindContactWindowCommandExecuted);
            ShowAddContactWindowCommand = new RelayCommand(ShowAddContactWindowCommandExecuted);
            SubmitEventCommand = new RelayCommand(SubmitEventCommandExecuted, SubmitEventCommandCanExecute);
            CancelEditingCommand = new RelayCommand(CancelEditingCommandExecuted);
            EditPrimaryContactCommand = new RelayCommand(EditPrimaryContactCommandExecuted, EditPrimaryContactCommandCanExecute);
            ShowResourcesCommand = new RelayCommand(ShowResourcesCommandExecuted, ShowResourcesCommandCanExecute);
            BookCommand = new RelayCommand(BookCommandExecute, BookCommandCanExecute);

            ProcessEnquiry(enquiryModel);
        }

        #endregion

        #region Methods

        private void ProcessEnquiry(EnquiryModel enquiryModel)
        {
            _isEditMode = (enquiryModel != null);

            Enquiry = enquiryModel ?? GetNewEnquiry();
            Enquiry.PropertyChanged += OnEnquiryPropertyChanged;
        }

        private EnquiryModel GetNewEnquiry()
        {
            return new EnquiryModel(new Enquiry()
            {
                ID = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                TakenByID = AccessService.Current.User.ID
            });
        }

        private void OnEnquiryPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            ShowResourcesCommand.RaiseCanExecuteChanged();
            SubmitEventCommand.RaiseCanExecuteChanged();
            EditPrimaryContactCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(() => AllowBooking);
            RaisePropertyChanged(() => IsEnquiryAndDetailFilled);
        }

        private void ProposePrimaryContact(Enquiry @enquiry)
        {
            if (@enquiry.Contact != null)
            {
                bool? dialogResult = null;
                string confirmText = string.Format("Do you want to choise {0} {1} as primary contact?", @enquiry.Contact.FirstName, @enquiry.Contact.LastName);

                RadWindow.Confirm(new DialogParameters()
                {
                    Owner = Application.Current.MainWindow,
                    Content = confirmText,
                    Closed = (sender, args) => { dialogResult = args.DialogResult; }
                });

                if (dialogResult == true)
                {
                    _enquiry.PrimaryContact = new ContactModel(@enquiry.Contact);
                    //_enquiry.Enquiry.Contact = @enquiry.Contact;   //Primary Contact issue
                }

            }
        }

        private async void SetContactToEnquiry(ContactModel model)
        {
            // Selected contact use ContactDataUnit so we need to get tha same object but from CrmDataUnit
            var contacts = await _crmDataUnit.ContactsRepository.GetAllAsync(x => x.ID == model.Contact.ID);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Enquiry.PrimaryContact = new ContactModel(contacts.FirstOrDefault());
                //_enquiry.Enquiry.Contact = contacts.FirstOrDefault();    //Primary Contact issue
            }));
        }

        public async void LoadData()
        {
            IsBusy = true;

            var types = await _crmDataUnit.EventTypesRepository.GetAllAsync(x => x.AllowEnquiry);
            EventTypes = new ObservableCollection<EventType>(types.OrderBy(x => x.Name));

            var statuses = await _crmDataUnit.EventStatusesRepository.GetAllAsync();
            EventStatuses = new ObservableCollection<EventStatus>(statuses.OrderBy(x => x.Name));

            _eventNoteTypes = await _crmDataUnit.EventNoteTypesRepository.GetAllAsync();

            var enquiries = await _crmDataUnit.EnquiriesRepository.GetLightEnquiriesAsync();
            AlreadyCreatedEnquiries = new ObservableCollection<Enquiry>(enquiries);

            var users = await _crmDataUnit.UsersRepository.GetUsersAsync();
            Users = new ObservableCollection<User>(users);

            OnLoadCurrentUser();

            var campaigns = await _crmDataUnit.CampaignsRepository.GetAllAsync(x => x.IsActive);
            Campaigns = new ObservableCollection<Campaign>(campaigns);

            var enquiryStatuses = await _crmDataUnit.EnquiryStatusesRepository.GetAllAsync(x => x.IsEnabled);
            EnquiryStatuses = new ObservableCollection<EnquiryStatus>(enquiryStatuses.OrderBy(x => x.Status));

            var methods = await _crmDataUnit.EnquiryReceiveMethodsRepository.GetAllAsync();
            EnquiryReceivedMethods = new ObservableCollection<EnquiryReceiveMethod>(methods.OrderBy(x => x.ReceiveMethod));

            if (_isEditMode)
            {
                var desiredEnquiry = await _crmDataUnit.EnquiriesRepository.GetUpdatedEnquiry(_enquiry.Enquiry.ID);

                if (desiredEnquiry.LockedUserID != null && desiredEnquiry.LockedUserID != AccessService.Current.User.ID)
                {
                    // Okey, someone is editing event right now. 
                    var user = (await _crmDataUnit.UsersRepository.GetUsersAsync(x => x.ID == desiredEnquiry.LockedUserID)).FirstOrDefault();

                    IsLocked = true;
                    LockedText = string.Format("{0} is locked by {1} {2}. Please wait till user makes changes", _enquiry.Name, user.FirstName, user.LastName);
                    return;
                }

                // Lock event
                _enquiry.Enquiry.LockedUserID = AccessService.Current.User.ID;
                await _crmDataUnit.SaveChanges();

                // Check if we have new changes
                if (desiredEnquiry != null && desiredEnquiry.LastEditDate != null && Enquiry.LoadedTime < desiredEnquiry.LastEditDate)
                {
                    Enquiry = new EnquiryModel(desiredEnquiry);
                    _forceRefreshEnquiryData = true;
                }

                LoadEnquiryData(_enquiry);
                EventType = _enquiry.EventType;
                AssignedToUser = _enquiry.AssignedToUser;
                LoggedUser = _enquiry.LoggedUser;
                Campaign = _enquiry.Campaign;
                EnquiryStatus = _enquiry.EnquiryStatus;
                ReceivedMethod = _enquiry.ReceivedMethod;
            }
            else
            {
                SetEnquiryStatus();
            }

            IsBusy = false;
        }

        private async void LoadEnquiryData(EnquiryModel model)
        {
            // if (!model.FollowUps.Any())
            {
                if (_forceRefreshEnquiryData) _crmDataUnit.FollowUpsRepository.Refresh();
                var followUps = await _crmDataUnit.FollowUpsRepository.GetAllAsync(x => x.EnquiryID == model.Enquiry.ID);
                model.FollowUps = new ObservableCollection<FollowUpModel>(followUps.Select(x => new FollowUpModel(x)));
            }
            //if (!model.Activities.Any())
            {
                if (_forceRefreshEnquiryData) _crmDataUnit.ActivitiesRepository.Refresh();
                var activities = await _crmDataUnit.ActivitiesRepository.GetAllAsync(x => x.EnquiryID == model.Enquiry.ID);
                model.Activities = new ObservableCollection<ActivityModel>(activities.OrderByDescending(p => p.Date).Select(x => new ActivityModel(x)));
            }

            if (!model.EnquiryUpdates.Any())
            {
                var updates = await _crmDataUnit.EnquiryUpdatesRepository.GetAllAsync(x => x.EnquiryID == model.Enquiry.ID);
                model.EnquiryUpdates = new ObservableCollection<EnquiryUpdate>(updates.OrderByDescending(x => x.Date));
            }

            if (!model.EnquiryNotes.Any())
            {
                var notes = await _crmDataUnit.EnquiryNotesRepository.GetAllAsync(x => x.EnquiryID == model.Enquiry.ID);
                model.EnquiryNotes = new ObservableCollection<EnquiryNoteModel>(notes.Select(x => new EnquiryNoteModel(x)));
            }

            if (!model.Correspondences.Any())
            {
                if (_forceRefreshEnquiryData) _crmDataUnit.CorresponcencesRepository.Refresh();
                var correspondence = await _crmDataUnit.CorresponcencesRepository.GetAllAsync(x => x.OwnerID == model.Enquiry.ID);
                model.Correspondences = new ObservableCollection<CorrespondenceModel>(
                    correspondence.OrderByDescending(x => x.Date).Select(x => new CorrespondenceModel(x)));

                var documents = await _crmDataUnit.DocumentsRepository.GetAllAsync(x => x.IsCommon);

                foreach (var enquiryCorresponcence in model.Correspondences)
                {
                    foreach (var cd in enquiryCorresponcence.Correspondence.CorrespondenceDocuments)
                    {
                        enquiryCorresponcence.Documents.Add(documents.First(x => x.ID == cd.DocumentID));
                    }
                }
            }

            _originalEnquiry = model.Clone();

            //  IsBusy = false;
        }

        private void SetEnquiryStatus()
        {
            if (!_isEditMode)
            {
                EventStatus = EventStatuses.FirstOrDefault(x => x.Name.Equals("Enquiry"));
            }
        }

        private void OnLoadCurrentUser()
        {
            if (_isEditMode) return;

            LoggedUser = Users.FirstOrDefault(x => x.ID == AccessService.Current.User.ID);
        }

        private void ProcessUpdates(EnquiryModel model, IEnumerable<string> diff)
        {
            diff.ForEach(difference =>
            {
                var update = new EnquiryUpdate()
                {
                    ID = Guid.NewGuid(),
                    Date = DateTime.Now,
                    EnquiryID = model.Enquiry.ID,
                    UserID = AccessService.Current.User.ID,
                    Message = difference
                };

                model.EnquiryUpdates.Add(update);
                _crmDataUnit.EnquiryUpdatesRepository.Add(update);
            });

            model.EnquiryUpdates = new ObservableCollection<EnquiryUpdate>(model.EnquiryUpdates.OrderByDescending(x => x.Date));

            //    _crmDataUnit.SaveChanges();
        }

        #endregion

        #region Commands

        private void ShowFindContactWindowCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var contactList = new ContactsListView();
            contactList.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (contactList.DialogResult == null || contactList.DialogResult != true || contactList.ViewModel.SelectedContact == null) return;

            Contact = contactList.ViewModel.SelectedContact;
        }

        private void ShowAddContactWindowCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addContactView = new AddContactView();
            addContactView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (addContactView.DialogResult == null || addContactView.DialogResult != true || addContactView.ViewModel.ContactModel == null) return;

            Contact = addContactView.ViewModel.ContactModel;
        }

        private void SubmitEventCommandExecuted()
        {
            if (!_isEditMode)
            {
                Enquiry.LoggedUser = LoggedUser;
                Enquiry.EnquiryStatus = EnquiryStatus;
                Enquiry.AssignedToUser = AssignedToUser;
                Enquiry.ReceivedMethod = ReceivedMethod;
                Enquiry.EventType = EventType;
                Enquiry.EventStatus = EventStatus;
                Enquiry.Campaign = Campaign;

                if (!_enquiry.Enquiry.Activities.Any() && !_enquiry.Enquiry.FollowUps.Any())
                    _crmDataUnit.EnquiriesRepository.Add(_enquiry.Enquiry);

                var update = new EnquiryUpdate()
                {
                    ID = Guid.NewGuid(),
                    EnquiryID = _enquiry.Enquiry.ID,
                    Date = DateTime.Now,
                    UserID = AccessService.Current.User.ID,
                    Message = string.Format("Enquiry {0} was created", _enquiry.Name)
                };

                _enquiry.EnquiryUpdates.Add(update);
                _crmDataUnit.EnquiryUpdatesRepository.Add(update);

            }
            else
            {
                _enquiry.Enquiry.TakenByID = LoggedUser.ID;
                _enquiry.Enquiry.EnquiryStatusID = EnquiryStatus.ID;
                _enquiry.Enquiry.AssignedToID = AssignedToUser.ID;
                _enquiry.Enquiry.ReceivedMethodID = ReceivedMethod.ID;
                _enquiry.Enquiry.EventTypeID = EventType.ID;
                _enquiry.Enquiry.Campaign = Campaign;
                _enquiry.AssignedToUser = AssignedToUser;
                _enquiry.EnquiryStatus = EnquiryStatus;
                _enquiry.ReceivedMethod = ReceivedMethod;
                _enquiry.EventType = EventType;

                var diff = LoggingService.FindDifference(_originalEnquiry, _enquiry, out ActivityChanged, out FoolowUpsChanged);
                ProcessUpdates(_enquiry, diff);

                _enquiry.Enquiry.LastEditDate = DateTime.Now;
                _enquiry.Enquiry.LockedUserID = null;
            }
            _crmDataUnit.SaveChanges();

            //if (_isEditMode)
            //{
            //    var diff = LoggingService.FindDifference(_originalEnquiry, _enquiry, out ActivityChanged, out FoolowUpsChanged);              
            //    ProcessUpdates(_enquiry, diff);
            //}

            PopupService.ShowMessage(Properties.Resources.MESSAGE_NEW_EVENT_ADDED, MessageType.Successful);
        }

        private bool SubmitEventCommandCanExecute()
        {
            return (Enquiry != null) && !Enquiry.HasErrors && EventType != null && EnquiryStatus != null && ReceivedMethod != null &&
                AssignedToUser != null;
        }

        private void CancelEditingCommandExecuted()
        {
            if (_isEditMode)
            {
                if (_isLocked) return;

                _enquiry.Enquiry.Value = _originalEnquiry.Enquiry.Value;
                _enquiry.Enquiry.Likelihood = _originalEnquiry.Enquiry.Likelihood;
                _enquiry.Enquiry.Name = _originalEnquiry.Enquiry.Name;
                _enquiry.Enquiry.Places = _originalEnquiry.Enquiry.Places;
                _enquiry.Enquiry.Date = _originalEnquiry.Enquiry.Date;
                _crmDataUnit.EnquiriesRepository.RevertAllChanges();
                _enquiry.Enquiry.LockedUserID = null;
                _crmDataUnit.SaveChanges();
            }
            else
            {
                _crmDataUnit.RevertChanges();
            }
        }

        private bool EditPrimaryContactCommandCanExecute()
        {
            return _enquiry.PrimaryContact != null;
        }

        private void EditPrimaryContactCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddContactView(_enquiry.PrimaryContact);
            window.ShowDialog();

            if (window.DialogResult != null && window.DialogResult == false)
            {
                //_enquiry.PrimaryContact = _originalEnquiry.PrimaryContact;
                _enquiry.PrimaryContact.Address1 = _originalEnquiry.PrimaryContact.Address1;
                _enquiry.PrimaryContact.Address2 = _originalEnquiry.PrimaryContact.Address2;
                _enquiry.PrimaryContact.Address3 = _originalEnquiry.PrimaryContact.Address3;
                _enquiry.PrimaryContact.City = _originalEnquiry.PrimaryContact.City;
                _enquiry.PrimaryContact.CompanyName = _originalEnquiry.PrimaryContact.CompanyName;
                _enquiry.PrimaryContact.Country = _originalEnquiry.PrimaryContact.Country;
                _enquiry.PrimaryContact.Email = _originalEnquiry.PrimaryContact.Email;
                _enquiry.PrimaryContact.FirstName = _originalEnquiry.PrimaryContact.FirstName;
                _enquiry.PrimaryContact.LastName = _originalEnquiry.PrimaryContact.LastName;
                _enquiry.PrimaryContact.Phone1 = _originalEnquiry.PrimaryContact.Phone1;
                _enquiry.PrimaryContact.Phone2 = _originalEnquiry.PrimaryContact.Phone2;
                _enquiry.PrimaryContact.PostCode = _originalEnquiry.PrimaryContact.PostCode;
                _enquiry.PrimaryContact.Title = _originalEnquiry.PrimaryContact.Title;

            }
            else
            {
                // _enquiry.PrimaryContact = window.ViewModel.ContactModel;
            }

            RaisePropertyChanged("EnableParentWindow");
        }

        private void ShowResourcesCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new ResourcesView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private bool ShowResourcesCommandCanExecute()
        {
            return _enquiry.Date != null;
        }

        private async void BookCommandExecute()
        {
            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_BOOKING_ENQUIRY;

            RadWindow.Confirm(new DialogParameters()
            {
                Owner = Application.Current.MainWindow,
                Content = confirmText,
                Closed = (sender, args) => { dialogResult = args.DialogResult; }
            });

            if (dialogResult != true) return;

            Enquiry.EnquiryStatus = EnquiryStatuses.FirstOrDefault(x => x.Status == "Booked");
            EnquiryStatus = Enquiry.EnquiryStatus;

            var newEvent = new Event()
            {
                ID = Guid.NewGuid(),
                Name = Enquiry.Name,
                Date = (DateTime)Enquiry.Date,
                Places = (int)Enquiry.Places,
                CreationDate = DateTime.Now,
                ShowOnCalendar = true,
                IsDeleted = false,
                LastEditDate = DateTime.Now,
                EventTypeID = Enquiry.EventType.ID,
                EnquiryID = Enquiry.Enquiry.ID,
                EventStatusID = _eventStatuses.FirstOrDefault(x => x.Name == "Provisional").ID
            };

            if (_enquiry.PrimaryContact != null)
                newEvent.ContactID = _enquiry.PrimaryContact.Contact.ID;

            _crmDataUnit.EventsRepository.Add(newEvent);

            var update = new EventUpdate()
            {
                ID = Guid.NewGuid(),
                EventID = newEvent.ID,
                Date = DateTime.Now,
                UserID = AccessService.Current.User.ID,
                Message = string.Format("Event {0} was created", newEvent.Name),
                OldValue = null,
                NewValue = newEvent.Name,
                ItemId = newEvent.ID,
                ItemType = "Event",
                Field = "Event",
                Action = UpdateAction.Added
            };

            _crmDataUnit.EventUpdatesRepository.Add(update);
            _crmDataUnit.SaveChanges();

            // Warning: Here we use EventDataUnit!

            var events = await _eventDataUnit.EventsRepository.GetLightEventsAsync(x => x.ID == newEvent.ID);
            var @event = events.FirstOrDefault();

            var item = new EventModel(@event);

            // Open Add Event window in UI thread
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var bookingView = new BookingView(BookingViews.Event, item);
                bookingView.ShowDialog();

                if (bookingView.DialogResult != null && bookingView.DialogResult == true)
                {
                    string campaingText = Enquiry.Campaign == null ? "" : ", via Campaign " + Enquiry.Campaign.Name;

                    var note = new EventNoteModel(new EventNote()
                    {
                        ID = Guid.NewGuid(),
                        EventID = newEvent.ID,
                        Date = DateTime.Now,
                        EventNoteType = _eventNoteTypes.FirstOrDefault(x => x.Type == "Internal"),
                        UserID = AccessService.Current.User.ID,
                        Note = String.Format("From Enquiry, made on {0}, Taken by {1} Assigned to {2} enquiry via {3} {4}. {5} Notes, {6} Updates, {7} Activities & {8} Follow-Ups.",
                        DateTime.Now,
                        Enquiry.LoggedUser.FirstName,
                        Enquiry.AssignedToUser.FirstName,
                        Enquiry.ReceivedMethod.ReceiveMethod,
                        campaingText,
                        Enquiry.EnquiryNotes.Count,
                        Enquiry.EnquiryUpdates.Count,
                        Enquiry.Activities.Count,
                        Enquiry.FollowUps.Count)
                    });

                    _crmDataUnit.EventNotesRepository.Add(note.EventNote);
                    _crmDataUnit.SaveChanges();
                }
            }));

            BookCommand.RaiseCanExecuteChanged();
        }

        private bool BookCommandCanExecute()
        {
            return !Enquiry.HasErrors && Enquiry.Event == null;
        }

        #endregion
    }
}
