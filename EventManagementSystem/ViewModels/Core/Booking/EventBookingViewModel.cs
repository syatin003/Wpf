using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Core.Extensions;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Models;
using EventManagementSystem.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Core.Booking.Common;
using EventManagementSystem.Views.Core.Booking;
using EventManagementSystem.Views.Core.Contacts;
using EventManagementSystem.Views.Events;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using System.Data.Entity.Core.Objects;
using EventManagementSystem.Properties;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders;

namespace EventManagementSystem.ViewModels.Core.Booking
{
    public class EventBookingViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;
        private String _busyText;
        private ObservableCollection<EventStatus> _eventStatuses;
        private ObservableCollection<EventType> _eventTypes;
        private EventModel _event;
        private bool _isEditMode;
        private ContactModel _contact;
        private List<EventCateringModel> _alreadyBookedCaterings;
        private List<EventRoomModel> _alreadyBookedRooms;
        private List<EventGolfModel> _alreadyBookedGolfs;
        private ObservableCollection<EventItemModel> _alreadyBookedEventItems;
        private List<Event> _allEvents;
        private ObservableCollection<Event> _alreadyCreatedEvents;
        private EventModel _originalEvent;
        private bool _isLocked;
        private string _lockedText;
        private bool _forceRefreshData;

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
        public String BusyText
        {
            get
            {
                return _busyText;
            }
            set
            {
                if (_busyText == value) return;
                _busyText = value;
                RaisePropertyChanged(() => BusyText);
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

        public ObservableCollection<Event> AlreadyCreatedEvents
        {
            get { return _alreadyCreatedEvents; }
            set
            {
                if (_alreadyCreatedEvents == value) return;
                _alreadyCreatedEvents = value;
                RaisePropertyChanged(() => AlreadyCreatedEvents);
            }
        }

        public Event AutoCompleteBoxSelectedItem
        {
            set
            {
                if (value == null) return;
                _event.Name = value.Name;
                ProposePrimaryContact(value);
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
        public List<EventCateringModel> AlreadyBookedCaterings
        {
            get { return _alreadyBookedCaterings; }
            set
            {
                if (_alreadyBookedCaterings == value) return;
                _alreadyBookedCaterings = value;
                RaisePropertyChanged(() => AlreadyBookedCaterings);
            }
        }
        public List<EventRoomModel> AlreadyBookedRooms
        {
            get { return _alreadyBookedRooms; }
            set
            {
                if (_alreadyBookedRooms == value) return;
                _alreadyBookedRooms = value;
                RaisePropertyChanged(() => AlreadyBookedRooms);
            }
        }
        public List<EventGolfModel> AlreadyBookedGolfs
        {
            get { return _alreadyBookedGolfs; }
            set
            {
                if (_alreadyBookedGolfs == value) return;
                _alreadyBookedGolfs = value;
                RaisePropertyChanged(() => AlreadyBookedGolfs);
            }
        }
        public ObservableCollection<EventItemModel> AlreadyBookedEventItems
        {
            get { return _alreadyBookedEventItems; }
            set
            {
                if (_alreadyBookedEventItems == value) return;
                _alreadyBookedEventItems = value;
                RaisePropertyChanged(() => AlreadyBookedEventItems);
            }
        }

        public bool IsEventDetailFilled
        {
            get
            {
                return Event != null && !Event.HasErrors;
            }
        }
        public RelayCommand ShowFindContactWindowCommand { get; private set; }
        public RelayCommand ShowAddContactWindowCommand { get; private set; }
        public RelayCommand EditPrimaryContactCommand { get; private set; }
        public RelayCommand SubmitEventCommand { get; private set; }
        public RelayCommand CancelEditingCommand { get; private set; }
        public RelayCommand ShowResourcesCommand { get; private set; }

        #endregion

        #region Constructors

        public EventBookingViewModel(EventModel eventModel, bool isDuplicate)
        {
            BusyText = "Loading";
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            ShowFindContactWindowCommand = new RelayCommand(ShowFindContactWindowCommandExecuted);
            ShowAddContactWindowCommand = new RelayCommand(ShowAddContactWindowCommandExecuted);
            SubmitEventCommand = new RelayCommand(SubmitEventCommandExecuted, SubmitEventCommandCanExecute);
            CancelEditingCommand = new RelayCommand(CancelEditingCommandExecuted);
            EditPrimaryContactCommand = new RelayCommand(EditPrimaryContactCommandExecuted, EditPrimaryContactCommandCanExecute);
            ShowResourcesCommand = new RelayCommand(ShowResourcesCommandExecuted);
            ProcessEvent(eventModel, isDuplicate);

        }
        #endregion

        #region Methods

        private void ProcessEvent(EventModel eventModel, bool isDuplicate)
        {
            if (isDuplicate)
                _isEditMode = false;
            else
                _isEditMode = (eventModel != null);

            Event = eventModel ?? GetNewEvent();
            Event.PropertyChanged += OnEventPropertyChanged;
        }

        private EventModel GetNewEvent()
        {
            var newEvent = new EventModel(new Event()
            {
                ID = Guid.NewGuid(),
                //Date = DateTime.Now,                              //commented to Set default Date empty
                CreationDate = DateTime.Now,
                ShowOnCalendar = true
            });

            if (Application.Current.Resources["SelectedEventStart"] != null)
            {
                newEvent.Date = (DateTime)(Application.Current.Resources["SelectedEventStart"]);                    //commented to Set default Date empty
                Application.Current.Resources["SelectedEventStart"] = null;
            }

            return newEvent;
        }

        private void OnEventPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            SubmitEventCommand.RaiseCanExecuteChanged();
            EditPrimaryContactCommand.RaiseCanExecuteChanged();
            if (args.PropertyName == "IsEventTypeChanged")
            {
                if (Event.IsEventTypeChanged)
                {
                    RemoveDefaultRemindersOnTypeChange();
                    Event.IsEventTypeChanged = false;
                }
            }
            if (args.PropertyName == "Date")
            {
                if (Event.IsEventDateChanged)
                {
                    RefreshRemindersDueDates();
                    Event.IsEventDateChanged = false;
                }
            }
        }

        private void ProposePrimaryContact(Event selectedEvent)
        {
            var contacts = _allEvents.Where(x => x.Name == selectedEvent.Name).Select(x => x.Contact).ToList();

            if (contacts.Any())
            {
                RaisePropertyChanged("DisableParentWindow");
                RaisePropertyChanged("EnableParentWindow");

                var window = new ProposePrimaryContactView(contacts);
                window.ShowDialog();

                if (window.DialogResult != null && window.DialogResult == true &&
                    window.ViewModel.SelectedContact != null)
                {
                    _event.PrimaryContact = window.ViewModel.SelectedContact;
                }

                RaisePropertyChanged("EnableParentWindow");

            }
        }

        private async void SetContactToEnquiry(ContactModel contact)
        {
            // Selected contact use ContactDataUnit so we need to get the same object but from EventDataUnit
            var contacts = await _eventsDataUnit.ContactsRepository.GetAllAsync(x => x.ID == contact.Contact.ID);

            Application.Current.Dispatcher.BeginInvoke(
                new Action(() => { Event.PrimaryContact = new ContactModel(contacts.FirstOrDefault()); }));
        }

        public async void LoadData()
        {
            IsBusy = true;
            if (_isEditMode)
            {
                var desiredEvent = await _eventsDataUnit.EventsRepository.GetUpdatedEvent(_event.Event.ID);

                // Check locking
                if (desiredEvent.LockedUserID != null && desiredEvent.LockedUserID != AccessService.Current.User.ID)
                {
                    // Okey, someone is editing event right now. 
                    var user = (await _eventsDataUnit.UsersRepository.GetUsersAsync(x => x.ID == desiredEvent.LockedUserID)).FirstOrDefault();

                    IsLocked = true;
                    LockedText = string.Format("{0} is locked by {1} {2}. Please wait till user makes changes", _event.Name, user.FirstName, user.LastName);
                    return;
                }

                // Lock event
                _event.Event.LockedUserID = AccessService.Current.User.ID;
                await _eventsDataUnit.SaveChanges();

                if (desiredEvent.LastEditDate != null && _event.LoadedTime < desiredEvent.LastEditDate)
                {
                    Event.RefreshEventProp();
                    if (desiredEvent.Contact != null)
                        Event.PrimaryContact = new ContactModel(desiredEvent.Contact);
                    if (desiredEvent.EventType != null)
                        Event.EventType = desiredEvent.EventType;
                    if (desiredEvent.EventStatus != null)
                        Event.EventStatus = desiredEvent.EventStatus;
                    Event.PropertyChanged += OnEventPropertyChanged;
                    _forceRefreshData = true;
                }

                if (_forceRefreshData) _eventsDataUnit.EventBookedProductsRepository.Refresh();
                var products = await _eventsDataUnit.EventBookedProductsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventBookedProducts = new List<EventBookedProductModel>(products.Select(x => new EventBookedProductModel(x)));

                if (_forceRefreshData) _eventsDataUnit.EventChargesRepository.Refresh();
                var charges = await _eventsDataUnit.EventChargesRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventCharges = new ObservableCollection<EventChargeModel>(charges.Select(x => new EventChargeModel(x)));

                if (_forceRefreshData) _eventsDataUnit.EventCateringsRepository.Refresh();
                var caterings = await _eventsDataUnit.EventCateringsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventCaterings = new List<EventCateringModel>(caterings.Select(x => new EventCateringModel(x)));

                if (_forceRefreshData) _eventsDataUnit.EventRoomsRepository.Refresh();
                var rooms = await _eventsDataUnit.EventRoomsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventRooms = new List<EventRoomModel>(rooms.Select(x => new EventRoomModel(x)));

                if (_forceRefreshData) _eventsDataUnit.EventGolfsRepository.Refresh();
                var golfs = await _eventsDataUnit.EventGolfsRepository.GetAllAsync(x => x.EventID == Event.Event.ID && x.IsLinked == false);
                Event.EventGolfs = new List<EventGolfModel>(golfs.Select(x => new EventGolfModel(x)));

                if (_forceRefreshData) _eventsDataUnit.EventInvoicesRepository.Refresh();
                var invoices = await _eventsDataUnit.EventInvoicesRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventInvoices = new List<EventInvoiceModel>(invoices.Select(x => new EventInvoiceModel(x)));

                if (_forceRefreshData) _eventsDataUnit.EventPaymentsRepository.Refresh();
                var payments = await _eventsDataUnit.EventPaymentsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventPayments = new ObservableCollection<EventPaymentModel>(payments.Select(x => new EventPaymentModel(x)));

                Event.RefreshItems();

                if (_forceRefreshData) _eventsDataUnit.EventNotesRepository.Refresh();
                var notes = await _eventsDataUnit.EventNotesRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventNotes = new ObservableCollection<EventNoteModel>(notes.Select(x => new EventNoteModel(x)));

                if (_forceRefreshData) _eventsDataUnit.EventRemindersRepository.Refresh();
                var reminders = await _eventsDataUnit.EventRemindersRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventReminders = new ObservableCollection<EventReminderModel>(reminders.Select(x => new EventReminderModel(x)));

                _event.LoadedTime = DateTime.Now;
                _forceRefreshData = false;
            }

            _eventsDataUnit.EventStatusesRepository.Refresh();
            var statuses = await _eventsDataUnit.EventStatusesRepository.GetAllAsync();

            if (Event.EventStatus != null)
                EventStatuses = new ObservableCollection<EventStatus>(statuses.Where(x => x.IsEnabled || x.ID == Event.EventStatus.ID).OrderBy(x => x.Name));
            else
                EventStatuses = new ObservableCollection<EventStatus>(statuses.Where(x => x.IsEnabled).OrderBy(x => x.Name));

            _eventsDataUnit.EventTypesRepository.Refresh();
            var types = await _eventsDataUnit.EventTypesRepository.GetAllAsync();
            if (Event.EventType != null)
                EventTypes = new ObservableCollection<EventType>(types.Where(x => x.IsEnabled && x.AllowToBeBooked || x.ID == Event.EventType.ID).OrderBy(x => x.Name));
            else
                EventTypes = new ObservableCollection<EventType>(types.Where(x => x.IsEnabled && x.AllowToBeBooked).OrderBy(x => x.Name));

            var events = await _eventsDataUnit.EventsRepository.GetLightEventsAsync();
            _allEvents = new List<Event>(events);
            AlreadyCreatedEvents = new ObservableCollection<Event>(_allEvents.DistinctBy(x => x.Name));

            _originalEvent = Event.Clone();

            IsBusy = false;
        }

        private void ProcessUpdates(EventModel model, List<EventUpdate> eventUpdates)
        {
            eventUpdates.ForEach(update =>
            {
                model.EventUpdates.Insert(0, update);
                _eventsDataUnit.EventUpdatesRepository.Add(update);
            });
        }

        /// <summary>
        /// Update the product and charges quantities when number of people changes 
        /// for a particular product if the quantity is same as number of people before changing the number of peope. 
        /// </summary>
        /// <param name="product"></param>
        private void SetProductCountAndCharges(EventBookedProductModel product)
        {
            if (product.Quantity == _originalEvent.Places)
            {
                product.Quantity = Event.Places;
                _event.EventCharges.Where(p => p.EventCharge.ID == product.EventCharge.EventCharge.ID).ForEach(eCharge =>
                {
                    eCharge.Quantity = Event.Places;
                });
            }
        }

        /// <summary>
        /// Check whether the sum of Items to be invoiced and Items not to be invoiced are equal.
        /// </summary>
        /// <returns></returns>
        private bool IsInvoicedandUnInvoicedPriceEquals()
        {
            var sumOfInvoicedItems = 0.0;
            var sumOfNonInvoicedItems = 0.0;
            var IsNonInvoicedItemAvailable = false;

            foreach (var eventItem in Event.EventItems)
            {
                if (eventItem.Instance.GetType() == typeof(EventCateringModel))
                {
                    var model = (EventCateringModel)eventItem.Instance;
                    if (model.EventCatering.ShowInInvoice)
                        sumOfInvoicedItems += eventItem.TotalPrice;
                    else
                    {
                        sumOfNonInvoicedItems += eventItem.TotalPrice;
                        IsNonInvoicedItemAvailable = true;
                    }
                }
                else if (eventItem.Instance.GetType() == typeof(EventRoomModel))
                {
                    var model = (EventRoomModel)eventItem.Instance;
                    if (model.EventRoom.ShowInInvoice)
                        sumOfInvoicedItems += eventItem.TotalPrice;
                    else
                    {
                        sumOfNonInvoicedItems += eventItem.TotalPrice;
                        IsNonInvoicedItemAvailable = true;
                    }
                }
                else if (eventItem.Instance.GetType() == typeof(EventGolfModel))
                {
                    var model = (EventGolfModel)eventItem.Instance;
                    if (model.EventGolf.ShowInInvoice)
                        sumOfInvoicedItems += eventItem.TotalPrice;
                    else
                    {
                        sumOfNonInvoicedItems += eventItem.TotalPrice;
                        IsNonInvoicedItemAvailable = true;
                    }
                }
                else if (eventItem.Instance.GetType() == typeof(EventInvoiceModel))
                {
                    var model = (EventInvoiceModel)eventItem.Instance;
                    if (model.EventInvoice.ShowInInvoice)
                        sumOfInvoicedItems += eventItem.TotalPrice;
                    else
                    {
                        sumOfNonInvoicedItems += eventItem.TotalPrice;
                        IsNonInvoicedItemAvailable = true;
                    }
                }
            }

            if (IsNonInvoicedItemAvailable && (Math.Round(sumOfInvoicedItems, 2) != Math.Round(sumOfNonInvoicedItems, 2)))
            {
                bool? dialogResult = null;
                string confirmText = Resources.MESSAGE_INVOICED_ITEMS_SUM_NOT_EQUAL_TO_NON_INVOICED_ITEMS_SUM;

                RadWindow.Confirm(new System.Windows.Controls.TextBlock() { Text = confirmText, TextWrapping = TextWrapping.Wrap, Width = 300 },
                    new EventHandler<WindowClosedEventArgs>((s, args) => { dialogResult = args.DialogResult; }));
                return dialogResult == null ? false : (bool)dialogResult;
            }
            return true;
        }


        /// <summary>
        /// Double Check whether the resources booked in the event are not booked by someone else meanwhile.
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task ValidateResourcesAvailability()
        {
            AlreadyBookedCaterings = new List<EventCateringModel>();
            AlreadyBookedRooms = new List<EventRoomModel>();
            AlreadyBookedGolfs = new List<EventGolfModel>();
            AlreadyBookedEventItems = new ObservableCollection<EventItemModel>();

            _eventsDataUnit.EventRoomsRepository.Refresh(RefreshMode.ClientWins);
            var rooms = await _eventsDataUnit.EventRoomsRepository.GetAllAsync(p => p.Event.ID != Event.Event.ID);
            var eventRooms = rooms.Select(x => new EventRoomModel(x)).ToList();

            _eventsDataUnit.EventCateringsRepository.Refresh(RefreshMode.ClientWins);
            var caterings = await _eventsDataUnit.EventCateringsRepository.GetAllAsync(p => p.Event.ID != Event.Event.ID);
            var eventCaterings = caterings.Select(x => new EventCateringModel(x)).ToList();

            _eventsDataUnit.EventGolfsRepository.Refresh(RefreshMode.ClientWins);
            var golfs = await _eventsDataUnit.EventGolfsRepository.GetAllAsync(p => p.Event.ID != Event.Event.ID);
            var eventGolfs = golfs.Select(x => new EventGolfModel(x)).ToList();

            var golfBookingService = new BookingsService() { BookedGolfs = eventGolfs };
            var roomBookingService = new BookingsService() { BookedRooms = eventRooms, BookedCaterings = eventCaterings };

            foreach (var eventItem in _event.EventItems)
            {
                if (eventItem.Instance.GetType() == typeof(EventCateringModel))
                {
                    var model = (EventCateringModel)eventItem.Instance;
                    if (!model.EventCatering.Room.MultipleBooking)
                    {
                        var startTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, model.StartTime.Hour, model.StartTime.Minute, 0);
                        var endTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, model.EndTime.Hour, model.EndTime.Minute, 0);
                        if (roomBookingService.IsRoomAvailable(_event.Event.ID, model.Room, startTime, endTime))
                            roomBookingService.BookedCaterings.Add(model);
                        else
                        {
                            AlreadyBookedCaterings.Add(model);
                            AlreadyBookedEventItems.Add(eventItem);
                        }
                    }
                }
                else if (eventItem.Instance.GetType() == typeof(EventRoomModel))
                {
                    var model = (EventRoomModel)eventItem.Instance;
                    if (!model.EventRoom.Room.MultipleBooking)
                    {
                        var startTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, model.StartTime.Hour, model.StartTime.Minute, 0);
                        var endTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, model.EndTime.Hour, model.EndTime.Minute, 0);
                        if (roomBookingService.IsRoomAvailable(_event.Event.ID, model.Room, startTime, endTime))
                            roomBookingService.BookedRooms.Add(model);
                        else
                        {
                            AlreadyBookedRooms.Add(model);
                            AlreadyBookedEventItems.Add(eventItem);
                        }
                    }
                }
                else if (eventItem.Instance.GetType() == typeof(EventGolfModel))
                {
                    var model = (EventGolfModel)eventItem.Instance;

                    var startTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, model.Time.Hour, model.Time.Minute, 0);
                    var endTime = startTime.AddMinutes(model.Golf.TimeInterval.TotalMinutes * model.EventGolf.Slots);
                    if (golfBookingService.IsGolfAvailable(model.Golf, startTime, endTime))
                    {
                        golfBookingService.BookedGolfs.Add(model);
                        if (model.EventGolf.EventGolf1 != null)
                        {
                            var linkedGolf = new EventGolfModel(model.EventGolf.EventGolf1);
                            startTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, linkedGolf.Time.Hour, linkedGolf.Time.Minute, 0);
                            endTime = startTime.AddMinutes(linkedGolf.Golf.TimeInterval.TotalMinutes * linkedGolf.EventGolf.Slots);
                            if (golfBookingService.IsGolfAvailable(linkedGolf.Golf, startTime, endTime))
                                golfBookingService.BookedGolfs.Add(linkedGolf);
                            else
                            {
                                AlreadyBookedGolfs.Add(model);
                                AlreadyBookedEventItems.Add(eventItem);
                            }
                        }
                    }
                    else
                    {
                        AlreadyBookedGolfs.Add(model);
                        AlreadyBookedEventItems.Add(eventItem);
                    }
                }
            }
        }

        /// <summary>
        /// Update the product and charges quantities when number of people changes for a all products.
        /// </summary>
        private void SetItemsCountOnPlacesChange()
        {
            if (_isEditMode)
            {
                if (Event.Places != _originalEvent.Places)
                {
                    bool? dialogResult = null;
                    string confirmText = Resources.CHANGING_PLACES_WILL_CHANGE_ITEM_QUANTITY;

                    RadWindow.Confirm(new DialogParameters
                    {
                        Content = new System.Windows.Controls.TextBlock()
                        {
                            Text = confirmText,
                            Width = 300,
                            TextWrapping = TextWrapping.Wrap
                        },
                        Closed = (sender, args) => { dialogResult = args.DialogResult; },
                        OkButtonContent = "Yes",
                        CancelButtonContent = "No",
                        Owner = Application.Current.MainWindow
                    });

                    if (dialogResult != null && dialogResult.Value)
                    {
                        Event.EventBookedProducts.ForEach(SetProductCountAndCharges);
                    }
                }

            }
        }

        private void RemoveDefaultRemindersOnTypeChange()
        {
            if (Event.EventReminders.Any())
            {
                var eventReminders = new ObservableCollection<EventReminderModel>();
                eventReminders = new ObservableCollection<EventReminderModel>(Event.EventReminders.Where(eventReminder => eventReminder.EventReminder.LastEditDate == null && eventReminder.EventReminder.EventTypeToDoID != null));
                if (eventReminders.ToList().Count > 0)
                {
                    bool? dialogResult = null;
                    string confirmText = Properties.Resources.MESSAGE_CHANGE_REMINDERS_ON_TYPE_CHANGED;

                    RaisePropertyChanged("DisableParentWindow");

                    RadWindow.Confirm(new System.Windows.Controls.TextBlock() { Text = confirmText, TextWrapping = TextWrapping.Wrap, Width = 300 },
                        new EventHandler<WindowClosedEventArgs>((s, args) => { dialogResult = args.DialogResult; }));

                    RaisePropertyChanged("EnableParentWindow");

                    if (dialogResult != null && dialogResult.Value)
                    {
                        foreach (var eventReminder in eventReminders)
                        {
                            _eventsDataUnit.EventRemindersRepository.Delete(eventReminder.EventReminder);
                        }
                        Event.EventReminders = new ObservableCollection<EventReminderModel>(Event.EventReminders.Except(eventReminders));
                    }
                }
            }
        }

        private void RefreshRemindersDueDates()
        {
            var updatedReminders = new ObservableCollection<EventReminderModel>(Event.EventReminders.Where(reminder => reminder.EventReminder.Status == false && reminder.EventReminder.EventTypeTODO != null
                && reminder.EventReminder.EventTypeTODO.RelatedDateType == 0));
            if (updatedReminders.Any())
            {
                bool? dialogResult = null;
                string confirmText = Resources.MESSAGE_CHANGE_REMINDERS_ON_DATE_CHANGED;
                RaisePropertyChanged("DisableParentWindow");
                RadWindow.Confirm(new System.Windows.Controls.TextBlock() { Text = confirmText, TextWrapping = TextWrapping.Wrap, Width = 300 },
                    new EventHandler<WindowClosedEventArgs>((s, args) => { dialogResult = args.DialogResult; }));
                RaisePropertyChanged("EnableParentWindow");
                if (dialogResult != null && dialogResult.Value)
                {
                    RaisePropertyChanged("DisableParentWindow");
                    var view = new UpdatedRemindersView(updatedReminders);
                    view.ShowDialog();
                    RaisePropertyChanged("EnableParentWindow");
                }
            }
        }
        #endregion

        #region Commands

        private void ShowFindContactWindowCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new ContactsListView();
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult.Value && view.ViewModel.SelectedContact != null)
            {
                Contact = view.ViewModel.SelectedContact;
            }
        }

        private void ShowAddContactWindowCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddContactView();
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult.Value && view.ViewModel.ContactModel != null)
            {
                Contact = view.ViewModel.ContactModel;
            }
        }

        private async void SubmitEventCommandExecuted()
        {
            IsBusy = true;
            BusyText = "Processing";

            //Set the Items Count on change of Number of Places 
            SetItemsCountOnPlacesChange();

            //Check whether the Sum of Invoived and Uninvoiced Items is equal or not
            if (!IsInvoicedandUnInvoicedPriceEquals())
            {
                IsBusy = false;
                return;
            }

            //Check whether the resources are available
            await ValidateResourcesAvailability();

            if (AlreadyBookedEventItems.Count > 0)
            {
                var alreadyBookedWindow = new EventItemsAlreadyBooked(Event, AlreadyBookedCaterings, AlreadyBookedRooms, AlreadyBookedGolfs, AlreadyBookedEventItems);
                RaisePropertyChanged("DisableParentWindow");
                alreadyBookedWindow.ShowDialog();
                RaisePropertyChanged("EnableParentWindow");
                if (alreadyBookedWindow.DialogResult == null || !alreadyBookedWindow.DialogResult.Value)
                {
                    IsBusy = false;
                    return;
                }
            }
            if (_isEditMode)
            {
                var eventUpdates = LoggingService.FindDifference(_originalEvent, _event);
                ProcessUpdates(_event, eventUpdates);
                _event.Event.LastEditDate = DateTime.Now;
                // Release lock
                _event.Event.LockedUserID = null;
            }
            else
            {
                _eventsDataUnit.EventsRepository.Add(_event.Event);

                var update = new EventUpdate()
                {
                    ID = Guid.NewGuid(),
                    EventID = _event.Event.ID,
                    Date = DateTime.Now,
                    UserID = AccessService.Current.User.ID,
                    Message = string.Format("Event {0} was created", _event.Name),
                    OldValue = null,
                    NewValue = _event.Name,
                    ItemId = _event.Event.ID,
                    ItemType = "Event",
                    Field = "Event",
                    Action = UpdateAction.Added
                };
                _event.EventUpdates.Add(update);
                _eventsDataUnit.EventUpdatesRepository.Add(update);

                var eventUpdates = LoggingService.FindDifference(_originalEvent, _event, true);
                ProcessUpdates(_event, eventUpdates);
            }
            await _eventsDataUnit.SaveChanges();

            IsBusy = false;
            RaisePropertyChanged("CloseDialog");
            PopupService.ShowMessage(
                _isEditMode
                    ? Properties.Resources.MESSAGE_NEW_EVENT_UPDATED
                    : Properties.Resources.MESSAGE_NEW_EVENT_ADDED, MessageType.Successful);
        }

        private bool SubmitEventCommandCanExecute()
        {
            return (Event != null) && !Event.HasErrors;     // && EventType != null && EventStatus != null;   //changes done for EventType and EventStatus
        }

        private async void CancelEditingCommandExecuted()
        {
            if (_isLocked) return;


            if (_isEditMode)
            {
                _event.EventStatus = EventStatuses.FirstOrDefault(p => p.ID == _originalEvent.Event.EventStatus.ID);
                _event.EventType = EventTypes.FirstOrDefault(p => p.ID == _originalEvent.Event.EventType.ID);

                _eventsDataUnit.EventsRepository.RevertAllChanges();
                _event.Event.LockedUserID = null;

                await _eventsDataUnit.SaveChanges();

                _event.RefreshItems();
            }
            else
            {
                _eventsDataUnit.RevertChanges();
            }
        }

        private bool EditPrimaryContactCommandCanExecute()
        {
            return _event.PrimaryContact != null;
        }

        private void EditPrimaryContactCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddContactView(_event.PrimaryContact);
            window.ShowDialog();

            if (window.DialogResult != null && window.DialogResult == false)
            {
                _event.PrimaryContact = _originalEvent.PrimaryContact.Clone();
            }

            RaisePropertyChanged("EnableParentWindow");
        }

        private void ShowResourcesCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new ResourcesView(_event.Date);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        #endregion
    }
}