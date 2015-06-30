using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EnquiryModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly Enquiry _enquiry;
        private ContactModel _primaryContact;
        private ObservableCollection<ActivityModel> _activities;
        private ObservableCollection<FollowUpModel> _followUps;
        private ObservableCollection<EnquiryUpdate> _enquiryUpdates;
        private ObservableCollection<EnquiryNoteModel> _enquiryNotes;
        private ObservableCollection<CorrespondenceModel> _correspondences;

        private bool _isSpecificEventTypeSelected;

        #endregion

        #region Properties

        [DataMember]
        public Enquiry Enquiry
        {
            get { return _enquiry; }
        }

        public DateTime LoadedTime { get; set; }

        [DataMember]
        public ContactModel PrimaryContact
        {
            get { return _primaryContact; }
            set
            {
                if (_primaryContact == value) return;
                _primaryContact = value;

                if (_primaryContact != null)
                {
                    _enquiry.Contact = _primaryContact.Contact;
                }
                RaisePropertyChanged(() => PrimaryContact);
                RaisePropertyChanged(() => PrimaryContact.ContactName);
            }
        }

        public string EnquiryString
        {
            get
            {
                if (PrimaryContact != null)
                    return EventType.Name + " [" + PrimaryContact.ContactName + "] " + Name;
                else
                    return EventType.Name + " " + Name;
            }
        }

        public bool IsSpecificEventTypeSelected
        {
            get
            {
                //if (EventType != null &&
                //    (EventType.Name == "Membership" || EventType.Name == "Employment"))
                //    return true;
                return _isSpecificEventTypeSelected;
            }
            set
            {
                if (_isSpecificEventTypeSelected == value) return;
                _isSpecificEventTypeSelected = value;
                RaisePropertyChanged(() => IsSpecificEventTypeSelected);
                RaisePropertyChanged(() => Name);
                RaisePropertyChanged(() => Date);
                RaisePropertyChanged(() => Places);
            }
        }

        public ActivityModel LastActivity
        {
            get
            {
                return Activities.LastOrDefault();
            }
        }

        public FollowUpModel NextFollowUp
        {
            get
            {
                return FollowUps.FirstOrDefault();
            }
        }

        [DataMember]
        public ObservableCollection<FollowUpModel> FollowUps
        {
            get { return _followUps; }
            set
            {
                if (_followUps == value) return;
                _followUps = value;
                RaisePropertyChanged(() => FollowUps);
            }
        }

        [DataMember]
        public ObservableCollection<ActivityModel> Activities
        {
            get { return _activities; }
            set
            {
                if (_activities == value) return;
                _activities = value;
                RaisePropertyChanged(() => Activities);
            }
        }

        [DataMember]
        public ObservableCollection<EnquiryUpdate> EnquiryUpdates
        {
            get { return _enquiryUpdates; }
            set
            {
                if (_enquiryUpdates == value) return;
                _enquiryUpdates = value;
                RaisePropertyChanged(() => EnquiryUpdates);
            }
        }

        [DataMember]
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

        [DataMember]
        public EventType EventType
        {
            get { return _enquiry.EventType; }
            set
            {
                if (_enquiry.EventType == value) return;
                _enquiry.EventType = value;
                RaisePropertyChanged(() => EventType);
                RaisePropertyChanged(() => IsSpecificEventTypeSelected);
                RaisePropertyChanged(() => Name);
            }
        }

        [DataMember]
        public string EventTypeName
        {
            get { return _enquiry.EventType.Name; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return _enquiry.CreationDate;
            }
            set
            {
                if (_enquiry.CreationDate == value) return;
                _enquiry.CreationDate = value;
                RaisePropertyChanged(() => CreationDate);
            }
        }

        [DataMember]
        public double EnquiryDetailValue
        {
            get
            {
                return Enquiry.Value;
            }
            set
            {
                if (_enquiry.Value == value) return;
                _enquiry.Value = value;
                RaisePropertyChanged(() => EnquiryDetailValue);
            }
        }

        [DataMember]
        public double EnquiryBookedValue
        {
            get
            {
                if (CurrentStatus == "Booked")
                    return Event.EventBookedProducts.Sum(x => x.Price * x.Quantity);
                return 0;
            }
        }

        [DataMember]
        public string EnquiryDetailStatus
        {
            get
            {
                return Enquiry.EventStatus.Name;
            }
        }

        [DataMember]
        public EventStatus EventStatus
        {
            get { return _enquiry.EventStatus; }
            set
            {
                if (_enquiry.EventStatus == value) return;
                _enquiry.EventStatus = value;
                RaisePropertyChanged(() => EventStatus);
            }
        }

        [DataMember]
        public string Name
        {
            get { return _enquiry.Name; }
            set
            {
                if (_enquiry.Name == value) return;
                _enquiry.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        [DataMember]
        public int? Places
        {
            get { return _enquiry.Places; }
            set
            {
                if (_enquiry.Places == value) return;
                _enquiry.Places = value;
                RaisePropertyChanged(() => Places);
            }
        }

        [DataMember]
        public DateTime? Date
        {
            get { return _enquiry.Date; }
            set
            {
                if (_enquiry.Date == value) return;
                _enquiry.Date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        [DataMember]
        public ObservableCollection<EnquiryNoteModel> EnquiryNotes
        {
            get { return _enquiryNotes; }
            set
            {
                if (_enquiryNotes == value) return;
                _enquiryNotes = value;
                RaisePropertyChanged(() => EnquiryNotes);
            }
        }

        [DataMember]
        public Event Event
        {
            get
            {
                return Enquiry.Events.LastOrDefault();
            }
        }

        [DataMember]
        public string CurrentStatus
        {
            get
            {
                if (Event == null)
                    return "Pending";
                switch (Event.EventStatus.Name)
                {
                    case "Provisional":
                        return "Booked";
                    case "Confirmed":
                        return "Sold";
                    case "Cancelled":
                        return "Cancelled";
                }
                return "Pending";
            }
        }

        [DataMember]
        public User LoggedUser
        {
            get { return _enquiry.User; }
            set
            {
                if (_enquiry.User == value) return;
                _enquiry.User = value;
                RaisePropertyChanged(() => LoggedUser);
            }
        }

        [DataMember]
        public User AssignedToUser
        {
            get { return _enquiry.User1; }
            set
            {
                if (_enquiry.User1 == value) return;
                _enquiry.User1 = value;
                RaisePropertyChanged(() => AssignedToUser);
            }
        }

        [DataMember]
        public double Value
        {
            get { return _enquiry.Value; }
            set
            {
                if (_enquiry.Value == value) return;
                _enquiry.Value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        [DataMember]
        public int Likelihood
        {
            get { return _enquiry.Likelihood; }
            set
            {
                if (_enquiry.Likelihood == value) return;
                _enquiry.Likelihood = value;
                RaisePropertyChanged(() => Likelihood);
            }
        }

        [DataMember]
        public EnquiryReceiveMethod ReceivedMethod
        {
            get { return _enquiry.EnquiryReceiveMethod; }
            set
            {
                if (_enquiry.EnquiryReceiveMethod == value) return;
                _enquiry.EnquiryReceiveMethod = value;
                RaisePropertyChanged(() => ReceivedMethod);
            }
        }

        [DataMember]
        public EnquiryStatus EnquiryStatus
        {
            get { return _enquiry.EnquiryStatus; }
            set
            {
                if (_enquiry.EnquiryStatus == value) return;
                _enquiry.EnquiryStatus = value;
                RaisePropertyChanged(() => EnquiryStatus);
            }
        }

        [DataMember]
        public Campaign Campaign
        {
            get { return _enquiry.Campaign; }
            set
            {
                if (_enquiry.Campaign == value) return;
                _enquiry.Campaign = value;
                RaisePropertyChanged(() => Campaign);
            }
        }

        #endregion

        #region Constructor

        public EnquiryModel(Enquiry enquiry)
        {
            _enquiry = enquiry;

            LoadedTime = DateTime.Now;

            if (_enquiry.EventType != null &&
                (EventType.Name == "Membership" || EventType.Name == "Employment"))
                IsSpecificEventTypeSelected = true;

            if (_enquiry.Contact != null)
                PrimaryContact = new ContactModel(_enquiry.Contact);

            EnquiryUpdates = new ObservableCollection<EnquiryUpdate>();
            EnquiryNotes = new ObservableCollection<EnquiryNoteModel>();
            FollowUps = new ObservableCollection<FollowUpModel>(_enquiry.FollowUps.Select(x => new FollowUpModel(x)));
            Activities = new ObservableCollection<ActivityModel>(_enquiry.Activities.Select(x => new ActivityModel(x)));
            Correspondences = new ObservableCollection<CorrespondenceModel>();
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => Date);
            RaisePropertyChanged(() => EnquiryStatus);
            RaisePropertyChanged(() => EventType);
            RaisePropertyChanged(() => ReceivedMethod);
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => PrimaryContact);
            RaisePropertyChanged(() => LastActivity);
            RaisePropertyChanged(() => NextFollowUp);
            RaisePropertyChanged(() => AssignedToUser);
        }

        #endregion

        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(EnquiryModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                //if (columnName == "EventType")
                //    if (EventType == null)
                //        Error = "Event type can't be empty!";

                //if (columnName == "EventStatus")
                //    if (EventStatus == null)
                //        Error = "Event status can't be empty!";

                if (columnName == "Name")
                {
                    if (IsSpecificEventTypeSelected)
                        return Error;
                    if (string.IsNullOrWhiteSpace(Name))
                        Error = "Name can't be empty!";
                }

                if (columnName == "Date")
                {
                    if (IsSpecificEventTypeSelected)
                        return Error; // TODO ???

                    if (Date == null || Date < new DateTime(1900, 1, 1))
                        Error = "Date can't be empty or less by 1900!";
                }

                if (columnName == "Places")
                {
                    if (IsSpecificEventTypeSelected)
                        return Error;
                    if (Places == null || Places < 0)
                        Error = "Event places can't be less than zero!";
                }

                //if (columnName == "AssignedToUser")
                //    if (AssignedToUser == null)
                //        Error = "AssignedTo text can't be empty.";

                //if (columnName == "ReceivedMethod")
                //    if (ReceivedMethod == null)
                //        Error = "ReceivedMethod text can't be empty.";

                //if (columnName == "EnquiryStatus")
                //    if (EnquiryStatus == null)
                //        Error = "EnquiryStatus text can't be empty.";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
