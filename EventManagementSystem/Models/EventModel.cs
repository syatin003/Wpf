using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Enums.Reports;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private Event _event;
        private ContactModel _primaryContact;
        private DateTime? _startTime;
        private DateTime? _endTime;
        private string _changes;
        private double _eventPrice;
        private double _eventPricePerPerson;
        private double _deposit;
        private double _adjustments;
        private double _payments;
        private double _outstanding;
        private TimeSpan _days;
        private ObservableCollection<string> _resourceBookings;
        private ObservableCollection<EventUpdate> _eventUpdates;
        private ObservableCollection<EventPaymentModel> _eventPayments;
        private ObservableCollection<EventItemModel> _eventItems;
        private ObservableCollection<EventNoteModel> _eventNotes;
        private ObservableCollection<EventChargeModel> _eventCharges;
        private ObservableCollection<EventItemModel> _eventItemsWithNotes;
        private ObservableCollection<EventContact> _eventContacts;
        private ObservableCollection<CorrespondenceModel> _correspondences;
        private ObservableCollection<ReportModel> _reports;
        private ObservableCollection<EventReminderModel> _eventReminders;

        private EventStatus _eventStatus;
        private EventType _eventType;
        private bool _isEventTypeChanged;
        private bool _isEventDateChanged;
        private DateTime? _groupDate;
        private bool _isActualEvent;
        private bool _isGroupDateVisible;

        #endregion

        #region Properties

        public List<EventCateringModel> EventCaterings { get; set; }
        public List<EventRoomModel> EventRooms { get; set; }
        public List<EventGolfModel> EventGolfs { get; set; }
        public List<EventInvoiceModel> EventInvoices { get; set; }
        public List<EventBookedProductModel> EventBookedProducts { get; set; }
        public List<Document> Documents { get; set; }

        public DateTime LoadedTime { get; set; }

        public Event Event
        {
            get { return _event; }
            set { _event = value; }
        }

        public ContactModel PrimaryContact
        {
            get { return _primaryContact; }
            set
            {
                if (_primaryContact == value) return;
                _primaryContact = value;

                if (_primaryContact != null)
                    _event.ContactID = _primaryContact.Contact.ID;

                RaisePropertyChanged(() => PrimaryContact);
            }
        }

        public double Deposit
        {
            get { return _deposit; }
            set
            {
                if (_deposit == value) return;
                _deposit = value;
                RaisePropertyChanged(() => Deposit);
            }
        }
        public double Adjustments
        {
            get { return _adjustments; }
            set
            {
                if (_adjustments == value) return;
                _adjustments = value;
                RaisePropertyChanged(() => Adjustments);
            }
        }
        public double Payments
        {
            get { return _payments; }
            set
            {
                if (_payments == value) return;
                _payments = value;
                RaisePropertyChanged(() => Payments);
            }
        }

        public double Outstanding
        {
            get { return _outstanding; }
            set
            {
                if (_outstanding == value) return;
                _outstanding = value;
                RaisePropertyChanged(() => Outstanding);
            }
        }


        public double Paid
        {
            get { return Payments + Deposit; }
        }

        public TimeSpan Days
        {
            get { return _days; }
            set
            {
                if (_days == value) return;
                _days = value;
                RaisePropertyChanged(() => Days);
            }
        }

        public string Changes
        {
            get { return _changes; }
            set
            {
                if (_changes == value) return;
                _changes = value;
                RaisePropertyChanged(() => Changes);
            }
        }

        public DateTime? StartTime
        {
            get { return _event.StartTime; }
            set
            {
                if (_event.StartTime == value) return;
                _event.StartTime = value;
                RaisePropertyChanged(() => StartTime);
            }
        }

        public DateTime? EndTime
        {
            get { return _event.EndTime; }
            set
            {
                if (_event.EndTime == value) return;
                _event.EndTime = value;
                RaisePropertyChanged(() => EndTime);
            }
        }

        public ObservableCollection<string> ResourceBookings
        {
            get { return _resourceBookings; }
            set
            {
                if (_resourceBookings == value) return;
                _resourceBookings = value;
                RaisePropertyChanged(() => ResourceBookings);
            }
        }

        public EventType EventType
        {
            get { return _eventType; }
            set
            {
                if (_eventType == value) return;
                _eventType = value;
                _event.EventTypeID = _eventType.ID;

                _event.LastEditDate = DateTime.Now;
                IsEventTypeChanged = true;
                RaisePropertyChanged(() => EventType);

            }
        }
        public bool IsEventTypeChanged
        {
            get { return _isEventTypeChanged; }
            set
            {
                if (_isEventTypeChanged == value) return;
                _isEventTypeChanged = value;
                RaisePropertyChanged(() => IsEventTypeChanged);
            }
        }
        public bool IsEventDateChanged
        {
            get { return _isEventDateChanged; }
            set
            {
                if (_isEventDateChanged == value) return;
                _isEventDateChanged = value;
                RaisePropertyChanged(() => IsEventDateChanged);
            }
        }
        public EventStatus EventStatus
        {
            get { return _eventStatus; }
            set
            {
                if (_eventStatus == value) return;
                _eventStatus = value;
                _event.EventStatusID = _eventStatus.ID;

                _event.LastEditDate = DateTime.Now;
                RaisePropertyChanged(() => EventStatus);
            }
        }

        public string Name
        {
            get { return _event.Name; }
            set
            {
                if (_event.Name == value) return;
                _event.Name = value;
                _event.LastEditDate = DateTime.Now;
                RaisePropertyChanged(() => Name);
            }
        }

        public int Places
        {
            get { return _event.Places; }
            set
            {
                if (_event.Places == value) return;
                _event.Places = value;
                _event.LastEditDate = DateTime.Now;
                RaisePropertyChanged(() => Places);
            }
        }

        public DateTime Date
        {
            get { return _event.Date; }
            set
            {
                if (_event.Date == value) return;
                _event.Date = value;
                _event.LastEditDate = DateTime.Now;
                IsEventDateChanged = true;
                RaisePropertyChanged(() => Date);

                UpdateStartAndEndTime();
            }
        }

        public double EventPrice
        {
            get { return _eventPrice; }
            set
            {
                if (_eventPrice == value) return;
                _eventPrice = value;
                RaisePropertyChanged(() => EventPrice);
            }
        }

        public double EventPricePerPerson
        {
            get { return _eventPricePerPerson; }
            set
            {
                if (_eventPricePerPerson == value) return;
                _eventPricePerPerson = value;
                RaisePropertyChanged(() => EventPricePerPerson);
            }
        }

        public bool IsFromEnquiry
        {
            get
            {
                return Event.EnquiryID != null;
            }
        }

        public EnquiryModel Enquiry
        {
            get
            {
                return new EnquiryModel(Event.Enquiry);
            }
        }

        public string EventContactType { get; set; }

        public ObservableCollection<EventUpdate> EventUpdates
        {
            get { return _eventUpdates; }
            set
            {
                if (_eventUpdates == value) return;
                _eventUpdates = value;
                RaisePropertyChanged(() => EventUpdates);
            }
        }

        public ObservableCollection<EventPaymentModel> EventPayments
        {
            get { return _eventPayments; }
            set
            {
                if (_eventPayments == value) return;
                _eventPayments = value;
                RaisePropertyChanged(() => EventPayments);
            }
        }

        public ObservableCollection<EventItemModel> EventItems
        {
            get { return _eventItems; }
            set
            {
                if (_eventItems == value) return;
                _eventItems = value;
                RaisePropertyChanged(() => EventItems);
            }
        }

        public ObservableCollection<EventNoteModel> EventNotes
        {
            get { return _eventNotes; }
            set
            {
                if (_eventNotes == value) return;
                _eventNotes = value;
                RaisePropertyChanged(() => EventNotes);
            }
        }

        public ObservableCollection<EventChargeModel> EventCharges
        {
            get { return _eventCharges; }
            set
            {
                if (_eventCharges == value) return;
                _eventCharges = value;
                RaisePropertyChanged(() => EventCharges);
            }
        }

        // Used in EventNotes viewmodel
        public ObservableCollection<EventItemModel> EventItemsWithNotes
        {
            get { return _eventItemsWithNotes; }
            set
            {
                if (_eventItemsWithNotes == value) return;
                _eventItemsWithNotes = value;
                RaisePropertyChanged(() => EventItemsWithNotes);
            }
        }

        public ObservableCollection<EventContact> EventContacts
        {
            get { return _eventContacts; }
            set
            {
                if (_eventContacts == value) return;
                _eventContacts = value;
                RaisePropertyChanged(() => EventContacts);
            }
        }

        public ObservableCollection<CorrespondenceModel> Correspondences
        {
            get { return _correspondences; }
            set
            {
                if (_correspondences == value) return;
                _correspondences = value;
                RaisePropertyChanged(() => Correspondences);
            }
        }

        public ObservableCollection<ReportModel> Reports
        {
            get { return _reports; }
            set
            {
                if (_reports == value) return;
                _reports = value;
                RaisePropertyChanged(() => Reports);
            }
        }
        public ObservableCollection<EventReminderModel> EventReminders
        {
            get { return _eventReminders; }
            set
            {
                if (_eventReminders == value) return;
                _eventReminders = value;
                RaisePropertyChanged(() => EventReminders);
            }
        }

        public string EventStatusName
        {
            get
            {
                if (EventStatus != null)
                    return EventStatus.Name;
                return "NA";
            }
        }

        public string EventTypeName
        {
            get
            {
                if (EventType != null)
                    return EventType.Name;
                return "NA";
            }
        }

        public string EventString
        {
            get
            {
                if (Date != default(DateTime))
                    return Name + " [ " + Date.ToString("dd/MM/yyyy") + " ] ";
                else
                    return Name;
            }
        }

        public DateTime? GroupDate
        {
            get { return _groupDate; }
            set
            {
                if (_groupDate == value) return;
                _groupDate = value;
                RaisePropertyChanged(() => GroupDate);
            }
        }
        public bool IsActualEvent
        {
            get { return _isActualEvent; }
            set
            {
                if (_isActualEvent == value) return;
                _isActualEvent = value;
                RaisePropertyChanged(() => IsActualEvent);
            }
        }
        public bool IsGroupDateVisible
        {
            get { return _isGroupDateVisible; }
            set
            {
                if (_isGroupDateVisible == value) return;
                _isGroupDateVisible = value;
                RaisePropertyChanged(() => IsGroupDateVisible);
            }
        }
        #endregion

        #region Constructor

        public EventModel()
        {

        }

        public EventModel(Event @event, bool isFromReports = false, RoundAgeReportFilter filterValue = RoundAgeReportFilter.None, bool isActualEvent = true)
        {
            _event = @event;

            _eventType = _event.EventType;
            _eventStatus = _event.EventStatus;

            EventUpdates = new ObservableCollection<EventUpdate>();
            EventItems = new ObservableCollection<EventItemModel>();
            EventNotes = new ObservableCollection<EventNoteModel>();
            EventCharges = new ObservableCollection<EventChargeModel>();
            EventItemsWithNotes = new ObservableCollection<EventItemModel>();
            EventContacts = new ObservableCollection<EventContact>();
            Correspondences = new ObservableCollection<CorrespondenceModel>();
            Reports = new ObservableCollection<ReportModel>();
            EventReminders = new ObservableCollection<EventReminderModel>();

            if (isFromReports)
            {
                EventPayments = new ObservableCollection<EventPaymentModel>(_event.EventPayments.Select(x => new EventPaymentModel(x)));
                EventCaterings = new List<EventCateringModel>(_event.EventCaterings.Select(x => new EventCateringModel(x)));
                EventGolfs = new List<EventGolfModel>(_event.EventGolfs.Where(eventGolf => !eventGolf.IsLinked).Select(x => new EventGolfModel(x)));
                EventRooms = new List<EventRoomModel>(_event.EventRooms.Select(x => new EventRoomModel(x)));
                EventInvoices = new List<EventInvoiceModel>(_event.EventInvoices.Select(x => new EventInvoiceModel(x)));
                EventBookedProducts = new List<EventBookedProductModel>(_event.EventBookedProducts.Select(x => new EventBookedProductModel(x)));
                EventNotes = new ObservableCollection<EventNoteModel>(_event.EventNotes.Select(x => new EventNoteModel(x)));

                SetEventPriceForReports(filterValue);
                UpdatePaymentDetails();
                Days = DateTime.Now - Event.Date;
            }
            else
            {
                EventPayments = new ObservableCollection<EventPaymentModel>();
                EventCaterings = new List<EventCateringModel>();
                EventRooms = new List<EventRoomModel>();
                EventGolfs = new List<EventGolfModel>();
                EventInvoices = new List<EventInvoiceModel>();
                EventBookedProducts = new List<EventBookedProductModel>();
            }
            Documents = new List<Document>();

            if (_event.Contact != null)
                PrimaryContact = new ContactModel(_event.Contact);

            LoadedTime = DateTime.Now;

            RefreshChanges();

            IsActualEvent = isActualEvent;
            IsGroupDateVisible = true;
        }

        public EventModel(DateTime dateTime, Event eventItem, bool isActualEvent)
            : this(eventItem, isActualEvent: isActualEvent)
        {
            GroupDate = dateTime;
        }
        #endregion

        #region Methods

        private void SetEventPriceForReports(RoundAgeReportFilter filterValue)
        {
            EventPrice = 0.0;
            if (filterValue == RoundAgeReportFilter.CateringOnly || filterValue == RoundAgeReportFilter.Both || filterValue == RoundAgeReportFilter.None)
            {
                foreach (var eventCatering in EventCaterings)
                {
                    eventCatering.EventBookedProducts = new ObservableCollection<EventBookedProductModel>(EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == eventCatering.EventCatering.ID));
                    EventPrice = EventPrice + (eventCatering.EventBookedProducts.Any() ? eventCatering.EventBookedProducts.Sum(y => y.TotalPrice) : 0);
                }
            }
            if (filterValue == RoundAgeReportFilter.GolfOnly || filterValue == RoundAgeReportFilter.Both || filterValue == RoundAgeReportFilter.None)
            {
                foreach (var eventGolf in EventGolfs)
                {
                    eventGolf.EventBookedProducts = new ObservableCollection<EventBookedProductModel>(EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == eventGolf.EventGolf.ID));
                    EventPrice = EventPrice + (eventGolf.EventBookedProducts.Any() ? eventGolf.EventBookedProducts.Sum(y => y.TotalPrice) : 0);
                }
            }
            if (filterValue == RoundAgeReportFilter.None)
            {
                foreach (var eventRoom in EventRooms)
                {
                    eventRoom.EventBookedProducts = new ObservableCollection<EventBookedProductModel>(EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == eventRoom.EventRoom.ID));
                    EventPrice = EventPrice + (eventRoom.EventBookedProducts.Any() ? eventRoom.EventBookedProducts.Sum(y => y.TotalPrice) : 0);
                }
                foreach (var eventInvoice in EventInvoices)
                {
                    eventInvoice.EventBookedProducts = new ObservableCollection<EventBookedProductModel>(EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == eventInvoice.EventInvoice.ID));
                    EventPrice = EventPrice + (eventInvoice.EventBookedProducts.Any() ? eventInvoice.EventBookedProducts.Sum(y => y.TotalPrice) : 0);
                }
            }
            if (Double.IsNaN(EventPrice / Places) || Double.IsInfinity(EventPrice / Places))
                EventPricePerPerson = 0;
            else
                EventPricePerPerson = EventPrice / Places;

        }

        private void RefreshChanges()
        {
            if (Event.LastEditDate.HasValue && (DateTime.Now.Date - Event.LastEditDate.Value.Date).TotalDays <= 7)
                Changes = "Mod";
            else if ((DateTime.Now.Date - Event.CreationDate.Date).TotalDays <= 7)
                Changes = "New";
        }
        public void Refresh()
        {
            RaisePropertyChanged(() => EventType);
            RaisePropertyChanged(() => EventStatus);
        }

        public void RefreshEventProp()
        {
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => Date);
            RaisePropertyChanged(() => Places);
        }

        public void RefreshItems()
        {
            EventItems = new ObservableCollection<EventItemModel>();

            foreach (var eventCatering in EventCaterings)
            {
                eventCatering.EventBookedProducts = new ObservableCollection<EventBookedProductModel>(
                    EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == eventCatering.EventCatering.ID));

                EventItems.Add(new EventItemModel()
                {
                    Instance = eventCatering,
                    Note = eventCatering.EventCatering.Notes,
                    Room = eventCatering.Room.Name,
                    Time = new DateTime(0001, 1, 1, eventCatering.Time.Hour, eventCatering.Time.Minute, eventCatering.Time.Second),//eventCatering.StartTime,
                    Title = "Catering",
                    TotalPrice = eventCatering.EventBookedProducts.Any() ? eventCatering.EventBookedProducts.Sum(y => y.TotalPrice) : 0,
                    Products = new ObservableCollection<EventBookedProductModel>(eventCatering.EventBookedProducts),
                    ShowInInvoice = eventCatering.EventCatering.ShowInInvoice,
                    IncludeInCorrespondence = eventCatering.EventCatering.IncludeInCorrespondence
                });
            }

            foreach (var eventRoom in EventRooms)
            {
                eventRoom.EventBookedProducts = new ObservableCollection<EventBookedProductModel>(
                    EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == eventRoom.EventRoom.ID));

                EventItems.Add(new EventItemModel()
                {
                    Instance = eventRoom,
                    Note = eventRoom.EventRoom.Notes,
                    Room = eventRoom.Room.Name,
                    Time = new DateTime(0001, 1, 1, eventRoom.StartTime.Hour, eventRoom.StartTime.Minute, eventRoom.StartTime.Second),
                    Title = "Room",
                    TotalPrice = eventRoom.EventBookedProducts.Any() ? eventRoom.EventBookedProducts.Sum(y => y.TotalPrice) : 0,
                    Products = new ObservableCollection<EventBookedProductModel>(eventRoom.EventBookedProducts),
                    ShowInInvoice = eventRoom.EventRoom.ShowInInvoice,
                    IncludeInCorrespondence = eventRoom.EventRoom.IncludeInCorrespondence
                });
            }

            EventGolfs.ForEach(eventGolf =>
            {
                eventGolf.EventBookedProducts = new ObservableCollection<EventBookedProductModel>(EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == eventGolf.EventGolf.ID));

                EventItems.Add(new EventItemModel()
                {
                    Instance = eventGolf,
                    Time = eventGolf.Time,
                    Note = eventGolf.EventGolf.Notes,
                    Title = "Golf",
                    TotalPrice = eventGolf.EventBookedProducts.Any() ? eventGolf.EventBookedProducts.Sum(y => y.TotalPrice) : 0,
                    Products = new ObservableCollection<EventBookedProductModel>(eventGolf.EventBookedProducts),
                    ShowInInvoice = eventGolf.EventGolf.ShowInInvoice,
                    IncludeInCorrespondence = eventGolf.EventGolf.IncludeInCorrespondence
                });
            });

            EventInvoices.ForEach(eventInvoice =>
            {
                eventInvoice.EventBookedProducts = new ObservableCollection<EventBookedProductModel>(EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == eventInvoice.EventInvoice.ID));

                EventItems.Add(new EventItemModel()
                {
                    Instance = eventInvoice,
                    Time = null,
                    Note = eventInvoice.EventInvoice.Notes,
                    Title = "Invoice",
                    TotalPrice = eventInvoice.EventBookedProducts.Any() ? eventInvoice.EventBookedProducts.Sum(y => y.TotalPrice) : 0,
                    Products = new ObservableCollection<EventBookedProductModel>(eventInvoice.EventBookedProducts),
                    ShowInInvoice = eventInvoice.EventInvoice.ShowInInvoice,
                    IncludeInCorrespondence = eventInvoice.EventInvoice.IncludeInCorrespondence
                });
            });

            EventItems = new ObservableCollection<EventItemModel>(EventItems.OrderBy(eventItem => eventItem.Time ?? DateTime.MaxValue));

            // Used on Event Notes view
            EventItemsWithNotes = new ObservableCollection<EventItemModel>(_eventItems.Where(eventItemWithNotes => !string.IsNullOrWhiteSpace(eventItemWithNotes.Note)));

            RefreshEventStartEndTime();

            RefreshEventPrice();

            UpdatePaymentDetails();

            Days = DateTime.Now - Event.Date;

            RefreshResourceBookingsList();
        }

        public void RefreshEventStartEndTime()
        {
            var minTime = new List<DateTime>();
            var maxTime = new List<DateTime>();

            var caterings = _eventItems.Where(x => x.Instance.GetType() == typeof(EventCateringModel)).ToList();
            if (caterings.Any())
            {
                minTime.Add(caterings.Min(x => ConvertEventItemDateToEventDate(((EventCateringModel)x.Instance).Time)));
                maxTime.Add(caterings.Max(x => ConvertEventItemDateToEventDate(((EventCateringModel)x.Instance).EndTime)));
            }

            var golfs = _eventItems.Where(x => x.Instance.GetType() == typeof(EventGolfModel)).ToList();
            if (golfs.Any())
            {
                minTime.Add(golfs.Min(x => ConvertEventItemDateToEventDate(((EventGolfModel)x.Instance).Time)));
                maxTime.Add(golfs.Max(x => ConvertEventItemDateToEventDate(((EventGolfModel)x.Instance).Time.AddMinutes(Places / 4 * 10))));
                //maxTime.Add(golfs.Max(x => ConvertEventItemDateToEventDate(((EventGolfModel)x.Instance).Time.AddMinutes(((EventGolfModel)x.Instance).EventGolf.Slots * ((EventGolfModel)x.Instance).Golf.TimeInterval.TotalMinutes))));

            }

            var rooms = _eventItems.Where(x => x.Instance.GetType() == typeof(EventRoomModel)).ToList();
            if (rooms.Any())
            {
                minTime.Add(rooms.Min(x => ConvertEventItemDateToEventDate(((EventRoomModel)x.Instance).StartTime)));
                maxTime.Add(rooms.Max(x => ConvertEventItemDateToEventDate(((EventRoomModel)x.Instance).EndTime)));
            }

            if (minTime.Any() && maxTime.Any())
            {
                var minValue = minTime.Min();
                var maxValue = maxTime.Max();

                StartTime = new DateTime(Date.Year, Date.Month, Date.Day, minValue.Hour, minValue.Minute, minValue.Second);
                EndTime = new DateTime(Date.Year, Date.Month, Date.Day, maxValue.Hour, maxValue.Minute, maxValue.Second);
            }
        }

        private DateTime ConvertEventItemDateToEventDate(DateTime itemTime)
        {
            return new DateTime(Date.Year, Date.Month, Date.Day, itemTime.Hour, itemTime.Minute, itemTime.Second);
        }

        public void RefreshEventPrice()
        {
            EventPrice = _eventItems.Where(eventItem => eventItem.ShowInInvoice == true).Sum(x => x.TotalPrice);
            EventPricePerPerson = EventPrice / Places;
            Adjustments = _eventCharges.Where(eventCharges => eventCharges.EventCharge.ShowInInvoice == true).Sum(x => x.TotalPrice) - EventPrice;
        }

        public void UpdatePaymentDetails()
        {
            Deposit = EventPayments.Any() ? EventPayments.Where(x => x.IsDeposit).Sum(x => x.Amount) : 0;
            Payments = EventPayments.Any() ? EventPayments.Where(x => !x.IsDeposit).Sum(x => x.Amount) : 0;

            Outstanding = EventPrice - Deposit - Payments + Adjustments;
        }

        private void UpdateStartAndEndTime()
        {
            if (StartTime != null && EndTime != null)
            {
                StartTime = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Value.Hour, StartTime.Value.Minute, StartTime.Value.Second);
                EndTime = new DateTime(Date.Year, Date.Month, Date.Day, EndTime.Value.Hour, EndTime.Value.Minute, EndTime.Value.Second);
            }
        }

        public void RefreshResourceBookingsList()
        {
            ResourceBookings = new ObservableCollection<string>();

            foreach (var itemModel in _eventItems.OrderBy(x => x.Time))
            {
                switch (itemModel.Title)
                {
                    case "Catering":
                        {
                            var catering = (EventCateringModel)itemModel.Instance;
                            ResourceBookings.Add(string.Format("{0} - {1} {2}",
                                catering.StartTime.ToString("t"),
                                catering.EndTime.ToString("t"),
                                catering.Room.Name
                                ));

                            break;
                        }

                    case "Room":
                        {
                            var room = (EventRoomModel)itemModel.Instance;
                            ResourceBookings.Add(string.Format("{0} - {1} {2}",
                                room.StartTime.ToString("t"),
                                room.EndTime.ToString("t"),
                                room.Room.Name
                                ));

                            break;
                        }

                    case "Golf":
                        {
                            var golf = (EventGolfModel)itemModel.Instance;
                            ResourceBookings.Add(string.Format("{0} - {1} {2}",
                                golf.Time.ToString("t"),
                                golf.Time.AddMinutes(golf.EventGolf.Slots * golf.Golf.TimeInterval.TotalMinutes).ToString("t"),
                                golf.Golf.Name
                                ));

                            break;
                        }
                }
            }
        }

        public void RefreshReports()
        {
            Reports = new ObservableCollection<ReportModel>(Reports.OrderByDescending(report => report.Report.Date));
            var ReportNames = new List<string>(){
                    "Function Sheet","Quote","Confirmation","Invoice"
                };
            var latestReports = Reports.Where(report => report.ReportColor == "White").ToList();
            latestReports.ForEach(report =>
            {
                report.ReportColor = "LightGray";
            });
            ReportNames.ForEach(reportName =>
            {
                var eventReport = Reports.Where(x => x.Name == reportName).OrderByDescending(x => x.Report.Date).FirstOrDefault();
                if (eventReport != null)
                    eventReport.ReportColor = "White";
            });
        }

        public List<ReportModel> GetLatestReports()
        {
            List<ReportModel> latestReports = new List<ReportModel>();
            var reportsGroupsbyDepName = Reports.GroupBy(report => report.Name).ToList();

            reportsGroupsbyDepName.ForEach(reportGroup =>
            {
                latestReports.Add(reportGroup.OrderByDescending(report => report.Report.Date).FirstOrDefault());
            });
            return latestReports;
        }
        #endregion

        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(EventModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
                if (IsActualEvent)
                {
                    if (columnName == "EventType")
                        if (EventType == null)
                            Error = "Event type can't be empty!";

                    if (columnName == "EventStatus")
                        if (EventStatus == null)
                            Error = "Event status can't be empty!";

                    if (columnName == "Name")
                        if (string.IsNullOrWhiteSpace(Name))
                            Error = "Name can't be empty!";

                    if (columnName == "Date")
                        if (Date <= new DateTime(1900, 1, 1))
                            Error = "Date can't be empty or less by 1900!";

                    if (columnName == "Places")
                        if (Places < 0)
                            Error = "Event places can't be less than zero!";

                }
                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}