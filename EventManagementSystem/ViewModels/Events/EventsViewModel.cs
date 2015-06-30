using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using EventManagementSystem.CommonObjects.Appointments;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Core.Booking;
using EventManagementSystem.Views.Core.Booking;
using EventManagementSystem.Views.CRM;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp;
using EventManagementSystem.Views.Events;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ScheduleView;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders;
using EventManagementSystem.Enums.Admin;
using EventManagementSystem.Views.Reports.ContentViews;
using Telerik.Windows.Documents.Model;

namespace EventManagementSystem.ViewModels.Events
{
    public class EventsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;
        private ObservableCollection<EventModel> _events;
        private ObservableAppointmentCollection _appointments;
        private EventModel _selectedEvent;

        private List<EventModel> _allEvents;
        private List<EventTypeModel> _allEventTypes;

        private DateTime _eventsFromDate;
        private DateTime _eventsToDate;
        private int _selectedTab;
        private bool _showCalendarOptions;
        private ObservableCollection<string> _calendarColorFilterItems;
        private string _selectedCalendarColorFilterItem;
        private ObservableCollection<SelectableObject<string>> _calencarEventTypesFilterItems;
        private TimeSpan _weeklyStartTime;
        private TimeSpan _dailyStartTime;
        private List<CalendarNoteModel> _calendarNotes;
        private bool _showCalendarNotes;

        private TimeSpan _startTimeForDayView;

        private EventReminderModel _currentlyAddedReminder;

        private ObservableCollection<EventModel> _eventsWithDateGroups;
        private bool _isEventWithDateGroups;

        #endregion

        #region Properties

        public static ObservableCollection<FollowUpStatus> ReminderStatuses { get; set; }

        public TimeSpan StartTimeForDayView
        {
            get { return _startTimeForDayView; }
            set
            {
                _startTimeForDayView = value;
                RaisePropertyChanged(() => StartTimeForDayView);
            }
        }

        public ObservableCollection<string> CalendarColorFilterItems
        {
            get { return _calendarColorFilterItems; }
            set
            {
                if (_calendarColorFilterItems == value) return;
                _calendarColorFilterItems = value;
                RaisePropertyChanged(() => CalendarColorFilterItems);
            }
        }

        public string SelectedCalendarColorFilterItem
        {
            get { return _selectedCalendarColorFilterItem; }
            set
            {
                if (_selectedCalendarColorFilterItem == value) return;
                _selectedCalendarColorFilterItem = value;

                RefreshAppointments();
            }
        }

        public ObservableCollection<SelectableObject<string>> CalencarEventTypesFilterItems
        {
            get { return _calencarEventTypesFilterItems; }
            set
            {
                if (_calencarEventTypesFilterItems == value) return;
                _calencarEventTypesFilterItems = value;
                RaisePropertyChanged(() => CalencarEventTypesFilterItems);
            }
        }

        public bool ShowCalendarOptions
        {
            get { return _showCalendarOptions; }
            set
            {
                if (_showCalendarOptions == value) return;
                _showCalendarOptions = value;
                RaisePropertyChanged(() => ShowCalendarOptions);
            }
        }

        public bool ShowCalendarNotes
        {
            get { return _showCalendarNotes; }
            set
            {
                if (_showCalendarNotes == value) return;
                _showCalendarNotes = value;
                RaisePropertyChanged(() => ShowCalendarNotes);

                RefreshAppointments();
            }
        }

        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab == value) return;
                _selectedTab = value;
                ShowCalendarOptions = (value == 1);
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

        public ObservableAppointmentCollection Appointments
        {
            get { return _appointments; }
            set
            {
                if (_appointments == value) return;
                _appointments = value;
                RaisePropertyChanged(() => Appointments);
            }
        }

        public EventModel SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (_selectedEvent == value) return;
                _selectedEvent = value;
                RaisePropertyChanged(() => SelectedEvent);

                DuplicateEventCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime EventsFromDate
        {
            get { return _eventsFromDate; }
            set
            {
                if (_eventsFromDate == value) return;
                _eventsFromDate = value;
                RaisePropertyChanged(() => EventsFromDate);

                UpdateEventsDataRange();
            }
        }

        public DateTime EventsToDate
        {
            get { return _eventsToDate; }
            set
            {
                if (_eventsToDate == value) return;
                _eventsToDate = value;
                RaisePropertyChanged(() => EventsToDate);

                UpdateEventsDataRange();
            }
        }

        public TimeSpan WeeklyStartTime
        {
            get { return _weeklyStartTime; }
            set
            {
                if (_weeklyStartTime == value) return;
                _weeklyStartTime = value;
                RaisePropertyChanged(() => WeeklyStartTime);
            }
        }

        public TimeSpan DailyStartTime
        {
            get { return _dailyStartTime; }
            set
            {
                if (_dailyStartTime == value) return;
                _dailyStartTime = value;
                RaisePropertyChanged(() => DailyStartTime);
            }
        }
        public EventReminderModel CurrentlyAddedReminder
        {
            get { return _currentlyAddedReminder; }
            set
            {
                if (_currentlyAddedReminder == value) return;
                _currentlyAddedReminder = value;
                RaisePropertyChanged(() => CurrentlyAddedReminder);
            }
        }

        public ObservableCollection<EventModel> EventsWithDateGroups
        {
            get { return _eventsWithDateGroups; }
            set
            {
                if (_eventsWithDateGroups == value) return;
                _eventsWithDateGroups = value;
                RaisePropertyChanged(() => EventsWithDateGroups);
            }
        }
        public bool IsEventWithDateGroups
        {
            get { return _isEventWithDateGroups; }
            set
            {
                if (_isEventWithDateGroups == value) return;
                _isEventWithDateGroups = value;
                RaisePropertyChanged(() => IsEventWithDateGroups);
                UpdateEventsDataRange();
            }
        }

        public RelayCommand AddEventCommand { get; private set; }
        public RelayCommand DuplicateEventCommand { get; private set; }
        public RelayCommand<EventModel> DeleteEventCommand { get; private set; }
        public RelayCommand<EventModel> EditEventCommand { get; private set; }
        public RelayCommand RefreshEventsCommand { get; private set; }
        public RelayCommand ShowTodayEventsCommand { get; private set; }
        public RelayCommand ShowWeekEventsCommand { get; private set; }
        public RelayCommand ShowMonthEventsCommand { get; private set; }
        public RelayCommand ShowFutureEventsCommand { get; private set; }
        public RelayCommand ShowDefaultEventsCommand { get; private set; }
        public RelayCommand ShowResourcesCommand { get; private set; }

        public RelayCommand AddToDoCommand { get; private set; }
        public RelayCommand AddEnquiryCommand { get; private set; }
        public RelayCommand AddCalendarNoteCommand { get; private set; }
        public RelayCommand<CalendarNoteModel> EditCalendarNoteCommand { get; private set; }
        public RelayCommand<CalendarNoteModel> DeleteCalendarNoteCommand { get; private set; }
        public RelayCommand<EventModel> DeleteEventNoteCommand { get; private set; }

        public RelayCommand AddReminderCommand { get; private set; }
        public RelayCommand ShowEventSynopsisReportCommand { get; private set; }
        public RelayCommand ShowCalendarReportCommand { get; private set; }

        #endregion

        #region Constructors

        public EventsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            AddEventCommand = new RelayCommand(AddEventCommandExecuted, AddEventCommandCanExecute);
            DeleteEventCommand = new RelayCommand<EventModel>(DeleteEventCommandExecuted, DeleteEventCommandCanExecute);
            EditEventCommand = new RelayCommand<EventModel>(EditEventCommandExecuted, EditEventCommandCanExecute);
            DuplicateEventCommand = new RelayCommand(DuplicateEventCommandExecuted, DuplicateEventCommandCanExecute);
            RefreshEventsCommand = new RelayCommand(RefreshEventsCommandExecuted);
            ShowTodayEventsCommand = new RelayCommand(ShowTodayEventsCommandExecuted);
            ShowWeekEventsCommand = new RelayCommand(ShowWeekEventsCommandExecuted);
            ShowMonthEventsCommand = new RelayCommand(ShowMonthEventsCommandExecuted);
            ShowFutureEventsCommand = new RelayCommand(ShowFutureEventsCommandExecuted);
            ShowDefaultEventsCommand = new RelayCommand(ShowDefaultEventsCommandExecuted, ShowDefaultEventsCommandCanExecute);
            ShowResourcesCommand = new RelayCommand(ShowResourcesCommandExecuted);

            AddToDoCommand = new RelayCommand(AddToDoCommandExecute);
            AddEnquiryCommand = new RelayCommand(AddEnquiryCommandExecute);
            AddCalendarNoteCommand = new RelayCommand(AddCalendarNoteCommandExecuted);
            EditCalendarNoteCommand = new RelayCommand<CalendarNoteModel>(EditCalendarNoteCommandExecuted);
            DeleteCalendarNoteCommand = new RelayCommand<CalendarNoteModel>(DeleteCalendarNoteCommandExecuted);
            DeleteEventNoteCommand = new RelayCommand<EventModel>(DeleteEventNoteCommandExecuted);

            AddReminderCommand = new RelayCommand(AddReminderCommandExecuted);
            ShowEventSynopsisReportCommand = new RelayCommand(ShowEventSynopsisReportCommandExecuted);
            ShowCalendarReportCommand = new RelayCommand(ShowCalendarReportCommandExecuted);

            CalendarColorFilterItems = new ObservableCollection<string>() { "Type", "Status" };
            SelectedCalendarColorFilterItem = CalendarColorFilterItems.First(); // Default filter is Type

        }
        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            if (EventsFromDate == default(DateTime) && EventsToDate == default(DateTime))
            {
                EventsFromDate = DateTime.Now;
                EventsToDate = DateTime.Now.Date.AddDays(31);
            }
            var weekStartDate = Properties.Settings.Default.WeeklyStartTime;
            WeeklyStartTime = new TimeSpan(weekStartDate.Hour, weekStartDate.Minute, weekStartDate.Second);

            var dayStartDate = Properties.Settings.Default.DailyStartTime;
            DailyStartTime = new TimeSpan(dayStartDate.Hour, dayStartDate.Minute, dayStartDate.Second);

            StartTimeForDayView = new TimeSpan(9, 0, 0);

            await OnLoadEvents();

            await OnLoadCalendarNotes();

            UpdateEventsDataRange();

            RefreshAppointments();

            var types = await _eventsDataUnit.EventTypesRepository.GetAllAsync();
            _allEventTypes = new List<EventTypeModel>(types.OrderBy(x => x.Name).Select(x => new EventTypeModel(x)));

            CalencarEventTypesFilterItems =
                new ObservableCollection<SelectableObject<string>>(
                    types.Select(x => new SelectableObject<string>(x.Name, true)));
            CalencarEventTypesFilterItems.Insert(0, new SelectableObject<string>("Select All", false)); // insert on the top

            CalencarEventTypesFilterItems.ForEach(x => x.PropertyChanged += SelectableEventTypeOnPropertyChanged);

            ShowCalendarNotes = true;

            var statusType = Convert.ToInt32(StatusType.ToDosStatus);
            var reminderStatuses = await _eventsDataUnit.FollowUpStatusesRepository.GetAllAsync(todoStatus => todoStatus.StatusType == statusType);
            ReminderStatuses = new ObservableCollection<FollowUpStatus>(reminderStatuses.OrderBy(x => x.NumberOfDays));
            IsBusy = false;
        }

        private async Task OnLoadEvents()
        {
            IsBusy = true;

            _eventsDataUnit.EventsRepository.Refresh();
            var events = await _eventsDataUnit.EventsRepository.GetLightEventsAsync(x => !x.IsDeleted);
            _allEvents = new List<EventModel>(events.OrderBy(x => x.Date).Select(x => new EventModel(x)));
            Events = new ObservableCollection<EventModel>(_allEvents);

            IsBusy = false;
        }

        private async Task OnLoadCalendarNotes()
        {
            IsBusy = true;

            var notes = await _eventsDataUnit.CalendarNotesRepository.GetAllAsync();
            _calendarNotes = new List<CalendarNoteModel>(notes.Select(x => new CalendarNoteModel(x)));

            IsBusy = false;
        }

        private void SelectableEventTypeOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            const string selectAllItem = "Select All"; // Define const variable to prevent spelling errors

            var obj = sender as SelectableObject<string>;
            if (obj != null && obj.Object == selectAllItem)
            {
                CalencarEventTypesFilterItems.Where(x => x.Object != selectAllItem).ForEach(item =>
                {
                    // Before changing IsSelected we need to remove PropertyChanged handler to prevent execution this method every time for every item
                    item.PropertyChanged -= SelectableEventTypeOnPropertyChanged;
                    item.IsSelected = obj.IsSelected;
                    item.PropertyChanged += SelectableEventTypeOnPropertyChanged;
                });
            }
            else
            {
                // Change selection from Select All if any item was deselected
                if (obj != null && !obj.IsSelected)
                {
                    var item = CalencarEventTypesFilterItems.FirstOrDefault(x => x.Object == selectAllItem);
                    if (item != null)
                    {
                        item.PropertyChanged -= SelectableEventTypeOnPropertyChanged;
                        item.IsSelected = false;
                        item.PropertyChanged += SelectableEventTypeOnPropertyChanged;
                    }
                }

                // If all items is selected select "Select All" item
                if (CalencarEventTypesFilterItems.Where(x => x.Object != selectAllItem).All(x => x.IsSelected))
                {
                    var item = CalencarEventTypesFilterItems.FirstOrDefault(x => x.Object == selectAllItem);
                    if (item != null)
                    {
                        item.PropertyChanged -= SelectableEventTypeOnPropertyChanged;
                        item.IsSelected = true;
                        item.PropertyChanged += SelectableEventTypeOnPropertyChanged;
                    }
                }
            }

            var selectedEventTypes =
                CalencarEventTypesFilterItems.Where(x => x.Object != selectAllItem && x.IsSelected)
                    .Select(x => x.Object);

            var selectedEvents = _allEvents.Where(x => selectedEventTypes.Contains(x.EventType.Name));

            Appointments = new ObservableAppointmentCollection();

            var eventsWithItems =
                selectedEvents.Where(x => x.Event.ShowOnCalendar && x.StartTime != null && x.EndTime != null);
            eventsWithItems.ForEach(
                x =>
                    Application.Current.Dispatcher.BeginInvoke(
                        new Action(() => Appointments.Add(ConvertEventToAppointment(x)))));

            if (_showCalendarNotes)
            {
                _calendarNotes.ForEach(x => Application.Current.Dispatcher.BeginInvoke(
                new Action(() => Appointments.Add(ConvertCalendarNoteToAppointment(x)))));
            }
        }

        private void UpdateEventsDataRange()
        {
            if (_allEvents == null) return;

            Events = new ObservableCollection<EventModel>(_allEvents.Where(x => x.Date >= EventsFromDate.Date && x.Date <= EventsToDate.Date));
            EventsWithDateGroups = new ObservableCollection<EventModel>(Events.Select(x => new EventModel(x.Date, x.Event, true)).OrderBy(eventItem => eventItem.GroupDate));

            if (IsEventWithDateGroups)
            {
                if (EventsToDate == DateTime.MaxValue.Date)
                    EventsToDate = EventsWithDateGroups.Select(eventItem => eventItem.Date).Last();
                Int32 days = Convert.ToInt32((EventsToDate - EventsFromDate).TotalDays);
                for (int i = 0; i <= days; i++)
                {
                    var eventsInGroup = EventsWithDateGroups.Where(eventGroup => eventGroup.GroupDate == EventsFromDate.Date.AddDays(i));
                    if (eventsInGroup.Count() == 0)
                    {
                        EventsWithDateGroups.Add(new EventModel(EventsFromDate.Date.AddDays(i), new Event(), false));
                    }
                    else
                    {
                        var isFirst = true;
                        foreach (var eventGroup in eventsInGroup)
                        {
                            if (!isFirst)
                                eventGroup.IsGroupDateVisible = false;
                            else
                                isFirst = false;
                        }
                    }
                }
                EventsWithDateGroups = new ObservableCollection<EventModel>(EventsWithDateGroups.OrderBy(eventItem => eventItem.GroupDate));
            }
        }

        private void RefreshAppointments()
        {
            Appointments = new ObservableAppointmentCollection();

            if (_allEvents != null && _allEvents.Any())
            {
                _allEvents
                    .Where(x => x.Event.ShowOnCalendar && x.StartTime != null && x.EndTime != null)
                    .ForEach(x => Application.Current.Dispatcher.BeginInvoke(
                        new Action(() => Appointments.Add(ConvertEventToAppointment(x)))));
            }

            if (_showCalendarNotes && _calendarNotes != null)
            {
                _calendarNotes.ForEach(x => Application.Current.Dispatcher.BeginInvoke(
                new Action(() => Appointments.Add(ConvertCalendarNoteToAppointment(x)))));
            }
        }

        private Appointment ConvertEventToAppointment(EventModel _event)
        {
            var color = (SelectedCalendarColorFilterItem == "Type")
                ? _event.EventType.Colour
                : _event.EventStatus.Colour;

            return new EventAppointment()
            {
                Start = (DateTime)_event.StartTime,
                End = (DateTime)_event.EndTime,
                Subject = _event.Name,
                Event = _event,
                Color = new BrushConverter().ConvertFromString(color) as SolidColorBrush
            };
        }

        private Appointment ConvertCalendarNoteToAppointment(CalendarNoteModel note)
        {
            return new CalendarNoteAppointment()
            {
                Start = note.CalendarNote.StartTime,
                End = note.CalendarNote.EndTime,
                Subject = note.Description,
                CalendarNote = note,
                Color = new BrushConverter().ConvertFromString(note.Color) as SolidColorBrush
            };
        }

        public async Task LoadLightEventDetails(EventModel model)
        {
            if (!model.EventItems.Any())
            {
                var products = await _eventsDataUnit.EventBookedProductsRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventBookedProducts = new List<EventBookedProductModel>(products.Select(x => new EventBookedProductModel(x)));

                var caterings = await _eventsDataUnit.EventCateringsRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventCaterings = new List<EventCateringModel>(caterings.Select(x => new EventCateringModel(x)));

                var rooms = await _eventsDataUnit.EventRoomsRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventRooms = new List<EventRoomModel>(rooms.Select(x => new EventRoomModel(x)));

                var golfs = await _eventsDataUnit.EventGolfsRepository.GetAllAsync(x => x.EventID == model.Event.ID && x.IsLinked == false);
                model.EventGolfs = new List<EventGolfModel>(golfs.Select(x => new EventGolfModel(x)));

                var invoices = await _eventsDataUnit.EventInvoicesRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventInvoices = new List<EventInvoiceModel>(invoices.Select(x => new EventInvoiceModel(x)));

                model.RefreshItems();
            }

            if (!model.EventUpdates.Any())
            {
                var updates = await _eventsDataUnit.EventUpdatesRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventUpdates = new ObservableCollection<EventUpdate>(updates.OrderByDescending(x => x.Date));
            }

            if (!model.EventPayments.Any())
            {
                var payments = await _eventsDataUnit.EventPaymentsRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventPayments = new ObservableCollection<EventPaymentModel>(payments.Select(x => new EventPaymentModel(x)));
            }

            model.UpdatePaymentDetails();
        }
        private void SetReminderPriority(EventReminderModel eventReminder)
        {
            var statuses = ReminderStatuses.OrderByDescending(x => x.NumberOfDays);
            foreach (var status in statuses)
            {
                if ((eventReminder.DateDue.Date - DateTime.Today).TotalDays >= status.NumberOfDays)
                {
                    eventReminder.Priority = status.Priority;
                    break;
                }
            }
            if (eventReminder.Priority == 0)
                eventReminder.Priority = statuses.Last().Priority;
        }

        public void RemoveReminders(EventReminderModel eventReminder, Guid? eventID)
        {
            if (eventID != null)
            {
                var thisEvent = Events.Where(eventItem => eventItem.Event.ID == eventID).FirstOrDefault();
                if (thisEvent != null)
                {
                    thisEvent.EventReminders.RemoveAll(reminder => reminder.EventReminder.ID == eventReminder.EventReminder.ID);
                }
            }
        }

        public void RefreshReminders(EventReminderModel eventReminder, Guid? eventID)
        {
            if (eventID != null)
            {
                var thisEvent = Events.Where(eventItem => eventItem.Event.ID == eventID).FirstOrDefault();
                if (thisEvent != null && thisEvent.EventReminders != null)
                {
                    var reminder = thisEvent.EventReminders.Where(ereminder => ereminder.EventReminder.ID == eventReminder.EventReminder.ID).FirstOrDefault();
                    if (reminder != null)
                    {
                        reminder.Refresh();
                    }
                }
            }
        }
        public void AddReminder(EventReminderModel eventReminder, Guid? eventID)
        {
            if (eventID != null)
            {
                var thisEvent = Events.Where(eventItem => eventItem.Event.ID == eventID).FirstOrDefault();
                if (thisEvent != null)
                {
                    thisEvent.EventReminders.Add(eventReminder);
                }
            }
        }
        #endregion

        #region Commands

        private void AddEventCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var bookingView = new BookingView(BookingViews.Event);
            bookingView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            // Refresh grid if event was added
            if (bookingView.DialogResult != null && bookingView.DialogResult == true)
            {
                var eventBookingView = bookingView.ViewModel.Content as EventBookingView;

                _allEvents.Add(eventBookingView.ViewModel.Event);
                _allEvents = new List<EventModel>(_allEvents.OrderBy(x => x.Date));
                Events = new ObservableCollection<EventModel>(_allEvents);
                UpdateEventsDataRange();
                RefreshAppointments();
            }
        }

        private void DeleteEventCommandExecuted(EventModel eventModel)
        {
            if (eventModel == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(new DialogParameters()
            {
                Owner = Application.Current.MainWindow,
                Content = confirmText,
                Closed = (sender, args) => { dialogResult = args.DialogResult; }
            });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            eventModel.Event.IsDeleted = true;

            var update = new EventUpdate()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                Event = eventModel.Event,
                UserID = AccessService.Current.User.ID,
                Message = string.Format("Event {0} was deleted", eventModel.Name),
                OldValue = eventModel.Name,
                NewValue = null,
                ItemId = eventModel.Event.ID,
                ItemType = "Event",
                Field = "Event",
                Action = UpdateAction.Removed
            };

            _eventsDataUnit.EventUpdatesRepository.Add(update);

            _eventsDataUnit.SaveChanges();

            _allEvents.Remove(eventModel);
            Events.Remove(eventModel);
            SelectedEvent = null;

            RefreshAppointments();
        }


        private void EditEventCommandExecuted(EventModel item)
        {

            RaisePropertyChanged("DisableParentWindow");

            var bookingView = new BookingView(BookingViews.Event, item);

            bookingView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (bookingView.DialogResult != null && bookingView.DialogResult == true)
            {
                item.Refresh();
                item.RefreshItems();
                RefreshAppointments();
            }
            else
            {
                item.Refresh();
            }

            UpdateEventsDataRange();

        }

        private bool AddEventCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_ADD_EVENT_ALLOWED);
        }

        private bool DeleteEventCommandCanExecute(EventModel eventModel)
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_DELETE_EVENT_ALLOWED);
        }

        private bool EditEventCommandCanExecute(EventModel arg)
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_EDIT_EVENT_ALLOWED);
        }

        private bool DuplicateEventCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_ADD_EVENT_ALLOWED) && SelectedEvent != null;
        }

        private async void DuplicateEventCommandExecuted()
        {

            RaisePropertyChanged("DisableParentWindow");
            var addDuplicateView = new DuplicateView();
            addDuplicateView.ShowDialog();
            RaisePropertyChanged("EnableParentWindow");

            if (addDuplicateView.DialogResult != null && addDuplicateView.DialogResult == true)
            {
                var addDuplicateViewModel = addDuplicateView.DataContext as DuplicateViewModel;
                if (addDuplicateViewModel != null)
                {
                    IsBusy = true;

                    var fromEvent = SelectedEvent;

                    await LoadLightEventDetails(fromEvent);

                    // General event info
                    var toEvent = new Event()
                    {
                        ID = Guid.NewGuid(),
                        Name = addDuplicateViewModel.EventName,
                        Date = Convert.ToDateTime(addDuplicateViewModel.EventDate),
                        Places = fromEvent.Places,
                        ContactID = fromEvent.Event.ContactID,
                        EventTypeID = fromEvent.Event.EventTypeID,
                        EventType = fromEvent.Event.EventType,
                        EventStatusID = fromEvent.Event.EventStatusID,
                        EventStatus = fromEvent.Event.EventStatus,
                        CreationDate = DateTime.Now,
                        MembersOnly = fromEvent.Event.MembersOnly,
                        ShowInForwardBook = fromEvent.Event.ShowInForwardBook,
                        ShowOnCalendar = fromEvent.Event.ShowOnCalendar,
                        UsedAsTemplate = fromEvent.Event.UsedAsTemplate,
                        InvoiceAddress = fromEvent.Event.InvoiceAddress,
                        StartTime = fromEvent.StartTime,
                        EndTime = fromEvent.EndTime
                    };

                    var toEventModel = new EventModel(toEvent);
                    // Notes
                    if (!fromEvent.EventNotes.Any())
                    {
                        var notes = await _eventsDataUnit.EventNotesRepository.GetAllAsync(x => x.EventID == fromEvent.Event.ID);
                        fromEvent.EventNotes = new ObservableCollection<EventNoteModel>(notes.Select(x => new EventNoteModel(x)));
                    }

                    if (fromEvent.EventNotes.Any())
                    {
                        fromEvent.EventNotes.ForEach(x =>
                        {
                            var note = new EventNote()
                            {
                                ID = Guid.NewGuid(),
                                EventID = toEventModel.Event.ID,
                                EventNoteTypeID = x.EventNote.EventNoteTypeID,
                                UserID = x.EventNote.UserID,
                                Date = DateTime.Now,
                                Note = x.Note
                            };

                            _eventsDataUnit.EventNotesRepository.Add(note);
                            toEventModel.EventNotes.Add(new EventNoteModel(note));
                        });
                    }

                    // Altarnative Contacts
                    if (!fromEvent.EventContacts.Any())
                    {
                        var contacts = await _eventsDataUnit.EventContactsRepository.GetAllAsync(x => x.EventID == fromEvent.Event.ID);
                        fromEvent.EventContacts = new ObservableCollection<EventContact>(contacts);
                    }

                    if (fromEvent.EventContacts.Any())
                    {
                        fromEvent.EventContacts.ForEach(x =>
                        {
                            var contact = new EventContact()
                            {
                                ID = Guid.NewGuid(),
                                EventID = toEventModel.Event.ID,
                                ContactID = x.ContactID
                            };

                            _eventsDataUnit.EventContactsRepository.Add(contact);
                            toEventModel.EventContacts.Add(contact);
                        });
                    }

                    // Event Caterings
                    if (fromEvent.EventCaterings.Any())
                    {
                        fromEvent.EventCaterings.ForEach(x =>
                        {
                            var catering = new EventCatering()
                            {
                                ID = Guid.NewGuid(),
                                EventID = toEventModel.Event.ID,
                                Event = toEventModel.Event,
                                Time = x.EventCatering.Time,
                                RoomID = x.EventCatering.RoomID,
                                StartTime = x.EventCatering.StartTime,
                                EndTime = x.EventCatering.EndTime,
                                Notes = x.EventCatering.Notes,
                                ShowInInvoice = x.EventCatering.ShowInInvoice,
                                IncludeInForwardBook = x.EventCatering.IncludeInForwardBook,
                                IncludeInCorrespondence = x.EventCatering.IncludeInCorrespondence,
                                IsSpecial = x.EventCatering.IsSpecial
                            };

                            var products = fromEvent.EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == x.EventCatering.ID).ToList();

                            if (products.Any())
                            {
                                products.ForEach(y =>
                                {
                                    var product = new EventBookedProduct()
                                    {
                                        ID = Guid.NewGuid(),
                                        EventID = toEventModel.Event.ID,
                                        ProductID = y.EventBookedProduct.ProductID,
                                        EventBookingItemID = catering.ID,
                                        Quantity = y.EventBookedProduct.Quantity,
                                        Price = y.EventBookedProduct.Price
                                    };

                                    var charge = new EventCharge()
                                    {
                                        ID = product.ID,
                                        EventID = toEventModel.Event.ID,
                                        ProductID = product.ProductID,
                                        Quantity = product.Quantity,
                                        Price = product.Price,
                                        ShowInInvoice = catering.ShowInInvoice
                                    };
                                    product.EventCharge = charge;

                                    _eventsDataUnit.EventBookedProductsRepository.Add(product);
                                    _eventsDataUnit.EventChargesRepository.Add(charge);
                                    toEventModel.EventBookedProducts.Add(new EventBookedProductModel(product));
                                    toEventModel.EventCharges.Add(new EventChargeModel(charge));
                                });
                            }
                            _eventsDataUnit.EventCateringsRepository.Add(catering);
                            toEventModel.EventCaterings.Add(new EventCateringModel(catering));
                        });
                    }

                    // Event Rooms
                    if (fromEvent.EventRooms.Any())
                    {
                        fromEvent.EventRooms.ForEach(x =>
                        {
                            var room = new EventRoom()
                            {
                                ID = Guid.NewGuid(),
                                EventID = toEventModel.Event.ID,
                                Event = toEventModel.Event,
                                RoomID = x.EventRoom.RoomID,
                                StartTime = x.EventRoom.StartTime,
                                EndTime = x.EventRoom.EndTime,
                                Notes = x.EventRoom.Notes,
                                ShowInInvoice = x.EventRoom.ShowInInvoice,
                                IncludeInForwardBook = x.EventRoom.IncludeInForwardBook,
                                IncludeInCorrespondence = x.EventRoom.IncludeInCorrespondence,
                            };

                            var products = fromEvent.EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == x.EventRoom.ID).ToList();

                            if (products.Any())
                            {
                                products.ForEach(y =>
                                {
                                    var product = new EventBookedProduct()
                                    {
                                        ID = Guid.NewGuid(),
                                        EventID = toEventModel.Event.ID,
                                        ProductID = y.EventBookedProduct.ProductID,
                                        EventBookingItemID = room.ID,
                                        Quantity = y.EventBookedProduct.Quantity,
                                        Price = y.EventBookedProduct.Price
                                    };

                                    var charge = new EventCharge()
                                    {
                                        ID = product.ID,
                                        EventID = toEventModel.Event.ID,
                                        ProductID = product.ProductID,
                                        Quantity = product.Quantity,
                                        Price = product.Price,
                                        ShowInInvoice = room.ShowInInvoice
                                    };
                                    product.EventCharge = charge;

                                    _eventsDataUnit.EventBookedProductsRepository.Add(product);
                                    _eventsDataUnit.EventChargesRepository.Add(charge);
                                    toEventModel.EventBookedProducts.Add(new EventBookedProductModel(product));
                                    toEventModel.EventCharges.Add(new EventChargeModel(charge));
                                });
                            }
                            _eventsDataUnit.EventRoomsRepository.Add(room);
                            toEventModel.EventRooms.Add(new EventRoomModel(room));
                        });
                    }

                    // Event Golfs
                    var fromEventGolfs = fromEvent.EventGolfs.Where(eventGolf => !eventGolf.EventGolf.IsLinked);
                    if (fromEventGolfs.Any())
                    {
                        fromEventGolfs.ForEach(x =>
                        {
                            var golf = new EventGolf()
                            {
                                ID = Guid.NewGuid(),
                                EventID = toEventModel.Event.ID,
                                Event = toEventModel.Event,
                                Time = x.EventGolf.Time,
                                TeeID = x.EventGolf.TeeID,
                                HoleID = x.EventGolf.HoleID,
                                Slots = x.EventGolf.Slots,
                                Notes = x.EventGolf.Notes,
                                ShowInInvoice = x.EventGolf.ShowInInvoice,
                                IncludeInForwardBook = x.EventGolf.IncludeInForwardBook,
                                IncludeInCorrespondence = x.EventGolf.IncludeInCorrespondence,
                                EventGolf1 = x.EventGolf.EventGolf1 != null ? new EventGolf()
                            {
                                ID = Guid.NewGuid(),
                                EventID = toEventModel.Event.ID,
                                Event = toEventModel.Event,
                                Time = x.EventGolf.EventGolf1.Time,
                                TeeID = x.EventGolf.EventGolf1.TeeID,
                                HoleID = x.EventGolf.EventGolf1.HoleID,
                                Slots = x.EventGolf.EventGolf1.Slots,
                                Notes = x.EventGolf.EventGolf1.Notes,
                                ShowInInvoice = x.EventGolf.EventGolf1.ShowInInvoice,
                                IncludeInForwardBook = x.EventGolf.EventGolf1.IncludeInForwardBook,
                                IncludeInCorrespondence = x.EventGolf.EventGolf1.IncludeInCorrespondence,
                                IsLinked = true
                            } : null
                            };

                            var products = fromEvent.EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == x.EventGolf.ID).ToList();

                            if (products.Any())
                            {
                                products.ForEach(y =>
                                {
                                    var product = new EventBookedProduct()
                                    {
                                        ID = Guid.NewGuid(),
                                        EventID = toEventModel.Event.ID,
                                        ProductID = y.EventBookedProduct.ProductID,
                                        EventBookingItemID = golf.ID,
                                        Quantity = y.EventBookedProduct.Quantity,
                                        Price = y.EventBookedProduct.Price
                                    };

                                    var charge = new EventCharge()
                                    {
                                        ID = product.ID,
                                        EventID = toEventModel.Event.ID,
                                        ProductID = product.ProductID,
                                        Quantity = product.Quantity,
                                        Price = product.Price,
                                        ShowInInvoice = golf.ShowInInvoice
                                    };
                                    product.EventCharge = charge;

                                    _eventsDataUnit.EventBookedProductsRepository.Add(product);
                                    _eventsDataUnit.EventChargesRepository.Add(charge);
                                    toEventModel.EventBookedProducts.Add(new EventBookedProductModel(product));
                                    toEventModel.EventCharges.Add(new EventChargeModel(charge));
                                });
                            }

                            _eventsDataUnit.EventGolfsRepository.Add(golf);
                            toEventModel.EventGolfs.Add(new EventGolfModel(golf));
                        });
                    }

                    // Event Invoices
                    if (fromEvent.EventInvoices.Any())
                    {
                        fromEvent.EventInvoices.ForEach(x =>
                        {
                            var invoice = new EventInvoice()
                            {
                                ID = Guid.NewGuid(),
                                EventID = toEventModel.Event.ID,
                                Event = toEventModel.Event,
                                Notes = x.EventInvoice.Notes,
                                ShowInInvoice = x.EventInvoice.ShowInInvoice,
                                IncludeInForwardBook = x.EventInvoice.IncludeInForwardBook,
                                IncludeInCorrespondence = x.EventInvoice.IncludeInCorrespondence,
                            };

                            var products = fromEvent.EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == x.EventInvoice.ID).ToList();

                            if (products.Any())
                            {
                                products.ForEach(y =>
                                {
                                    var product = new EventBookedProduct()
                                    {
                                        ID = Guid.NewGuid(),
                                        EventID = toEventModel.Event.ID,
                                        ProductID = y.EventBookedProduct.ProductID,
                                        EventBookingItemID = invoice.ID,
                                        Quantity = y.EventBookedProduct.Quantity,
                                        Price = y.EventBookedProduct.Price
                                    };

                                    var charge = new EventCharge()
                                    {
                                        ID = product.ID,
                                        EventID = toEventModel.Event.ID,
                                        ProductID = product.ProductID,
                                        Quantity = product.Quantity,
                                        Price = product.Price,
                                        ShowInInvoice = invoice.ShowInInvoice
                                    };
                                    product.EventCharge = charge;

                                    _eventsDataUnit.EventBookedProductsRepository.Add(product);
                                    _eventsDataUnit.EventChargesRepository.Add(charge);
                                    toEventModel.EventBookedProducts.Add(new EventBookedProductModel(product));
                                    toEventModel.EventCharges.Add(new EventChargeModel(charge));
                                });
                            }
                            _eventsDataUnit.EventInvoicesRepository.Add(invoice);
                            toEventModel.EventInvoices.Add(new EventInvoiceModel(invoice));
                        });
                    }

                    _eventsDataUnit.EventsRepository.DetectChanges();
                    toEventModel.RefreshItems();
                    RaisePropertyChanged("DisableParentWindow");
                    var bookingView = new BookingView(BookingViews.Event, toEventModel, true);
                    bookingView.ShowDialog();
                    RaisePropertyChanged("EnableParentWindow");

                    // Refresh grid if event was added
                    if (bookingView.DialogResult != null && bookingView.DialogResult == true)
                    {
                        var eventBookingView = bookingView.ViewModel.Content as EventBookingView;
                        _allEvents.Add(eventBookingView.ViewModel.Event);
                        _allEvents = new List<EventModel>(_allEvents.OrderBy(x => x.Date));
                        Events = new ObservableCollection<EventModel>(_allEvents);
                        UpdateEventsDataRange();
                        RefreshAppointments();
                    }
                    IsBusy = false;
                }
            }
        }

        private async void RefreshEventsCommandExecuted()
        {
            _eventsDataUnit.EventsRepository.Refresh();
            _eventsDataUnit.EventRemindersRepository.Refresh();
            _eventsDataUnit.CalendarNotesRepository.Refresh();

            await OnLoadEvents();
            await OnLoadCalendarNotes();

            UpdateEventsDataRange();

            RefreshAppointments();
        }

        private void ShowTodayEventsCommandExecuted()
        {
            EventsFromDate = DateTime.Now;
            EventsToDate = DateTime.Now;
        }

        private void ShowWeekEventsCommandExecuted()
        {
            EventsFromDate = DateTime.Now;
            EventsToDate = DateTime.Now.Date.AddDays(7);
        }

        private void ShowMonthEventsCommandExecuted()
        {
            EventsFromDate = DateTime.Now;
            EventsToDate = DateTime.Now.Date.AddDays(31);
        }

        private void ShowFutureEventsCommandExecuted()
        {
            EventsFromDate = DateTime.Now.Date;
            EventsToDate = DateTime.MaxValue.Date;
        }

        private void ShowDefaultEventsCommandExecuted()
        {
        }

        private bool ShowDefaultEventsCommandCanExecute()
        {
            return false;
        }

        private async void ShowResourcesCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new ResourcesView();       //Pass not from event
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            // refresh grid
            if (window.ViewModel.IsEventSaved)
            {
                await OnLoadEvents();
                RefreshAppointments();
            }
        }

        private void AddEnquiryCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new NewEnquiryView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void AddToDoCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddFollowUpView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void AddCalendarNoteCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddCalendarNoteView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                var note = window.ViewModel.CalendarNote;

                _calendarNotes.Add(note);
                Application.Current.Dispatcher.BeginInvoke(new Action(() => Appointments.Add(ConvertCalendarNoteToAppointment(note))));
            }
        }

        private async void EditCalendarNoteCommandExecuted(CalendarNoteModel obj)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddCalendarNoteView(obj);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                await OnLoadCalendarNotes();
                RefreshAppointments();
            }
        }
        private async void DeleteCalendarNoteCommandExecuted(CalendarNoteModel obj)
        {
            _eventsDataUnit.CalendarNotesRepository.Delete(obj.CalendarNote);
            await _eventsDataUnit.SaveChanges();
        }
        private async void DeleteEventNoteCommandExecuted(EventModel obj)
        {
            obj.Event.ShowOnCalendar = false;
            await _eventsDataUnit.SaveChanges();
        }

        private void AddReminderCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");
            var eventReminderView = new AddEventReminderView(_allEvents);
            eventReminderView.ShowDialog();
            RaisePropertyChanged("EnableParentWindow");
            if (eventReminderView.DialogResult != null && eventReminderView.DialogResult == true)
            {
                SetReminderPriority(eventReminderView.ViewModel.EventReminder);
                CurrentlyAddedReminder = eventReminderView.ViewModel.EventReminder;
                RaisePropertyChanged("AddNewReminder");
            }
        }

        private void ShowCalendarReportCommandExecuted()
        {
            var calendarReportView = new CalendarDescriptionView();
            calendarReportView.LoadData(_allEvents, _allEventTypes, EventsFromDate, EventsToDate);
        }

        private async void ShowEventSynopsisReportCommandExecuted()
        {
            var eventSynopsisView = new ForwardSynopsisDescriptionView();
            var events = await _eventsDataUnit.EventsRepository.GetEventsForReportsAsync(x => !x.IsDeleted);
            var allevents = new List<EventModel>(events.Select(x => new EventModel(x, true)));
            var Events = new ObservableCollection<EventModel>(allevents);
            var eventsGroups = new ObservableCollection<EventsGroup>(Events.GroupBy(p => p.Date)
                .Where(x => x.Key.Date >= EventsFromDate.Date && x.Key.Date <= EventsToDate.Date)
                .OrderBy(p => p.Key).Select(x => new EventsGroup(x.Key, new ObservableCollection<EventModel>(x))));

            RadDocument document;
            string[] values;
            eventSynopsisView.GetRadDocumentAndValues(out document, out values, eventsGroups, EventsFromDate, EventsToDate);
            PrintService.Export(document, values);
        }

        #endregion
    }
}