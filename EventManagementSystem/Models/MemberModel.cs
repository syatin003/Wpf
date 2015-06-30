using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Enums.Membership;
using System.Collections.ObjectModel;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MemberModel : ModelBase, IDataErrorInfo
    {

        #region Fields

        private readonly Member _member;
        private ContactModel _contact;
        private DateTime? _startDate;
        private DateTime? _renewalDate;

        private Status? _status;
        private ObservableCollection<MemberNoteModel> _memberNote;
        private ObservableCollection<MembershipUpdate> _membershipUpdates;
        private bool _includeInEmail;

        #endregion Fields

        #region Properties

        [DataMember]
        public Member Member
        {
            get { return _member; }
        }

        [DataMember]
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

        [DataMember]
        public MembershipCategory Category
        {
            get { return _member.MembershipCategory; }
            set
            {
                if (_member.MembershipCategory == value) return;
                _member.MembershipCategory = value;
                _member.CategoryID = _member.MembershipCategory.ID;
                RaisePropertyChanged(() => Category);
            }
        }

        [DataMember]
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                if (value != null)
                    _member.StartDate = Convert.ToDateTime(value);
                RaisePropertyChanged(() => StartDate);
            }
        }

        [DataMember]
        public DateTime? RenewalDate
        {
            get { return _renewalDate; }
            set
            {
                if (_renewalDate == value) return;
                _renewalDate = value;
                if (value != null)
                    _member.RenewalDate = Convert.ToDateTime(value);
                RaisePropertyChanged(() => RenewalDate);
            }
        }

        [DataMember]
        public Status? Status
        {
            get
            {
                if (_member.Status == 1)
                    return Enums.Membership.Status.Active;
                if (_member.Status == 2)
                    return Enums.Membership.Status.Pending;
                if (_member.Status == 3)
                    return Enums.Membership.Status.Suspended;
                if (_member.Status == 4)
                    return Enums.Membership.Status.Warning;
                return null;
            }
            set
            {
                if (_status == value) return;
                _status = value;
                _member.Status = Convert.ToInt32(_status);
                RaisePropertyChanged(() => Status);
            }
        }

        public String StatusGroup
        {
            get
            {
                if (Status == Enums.Membership.Status.Pending)
                    return "Pending";
                return "Current";
            }
        }

        [DataMember]
        public string CategoryName
        {
            get { return _member.MembershipCategory.Name; }
        }

        [DataMember]
        public string CategoryGroupName
        {
            get { return _member.MembershipCategory.MembershipGroup.Name; }
        }

        public ObservableCollection<MemberNoteModel> MemberNotes
        {
            get { return _memberNote; }
            set
            {
                if (_memberNote == value) return;
                _memberNote = value;
                RaisePropertyChanged(() => MemberNotes);
            }
        }
        public ObservableCollection<MembershipUpdate> MembershipUpdates
        {
            get { return _membershipUpdates; }
            set
            {
                if (_membershipUpdates == value) return;
                _membershipUpdates = value;
                RaisePropertyChanged(() => MembershipUpdates);
            }
        }
        public bool IncludeInEmail
        {
            get
            {
                return _includeInEmail;
            }
            set
            {
                if (_includeInEmail == value) return;
                _includeInEmail = value;
                RaisePropertyChanged(() => IncludeInEmail);
            }
        }

        #endregion Properties

        #region Constructor

        public MemberModel(Member member)
        {
            _member = member;

            MembershipUpdates = new ObservableCollection<MembershipUpdate>();

            if (_member != null)
            {
                if (_member.Contact != null) _contact = new ContactModel(_member.Contact);

                StartDate = _member.StartDate == default(DateTime) ? (DateTime?)null : Convert.ToDateTime(_member.StartDate);

                RenewalDate = _member.RenewalDate == default(DateTime) ? (DateTime?)null : Convert.ToDateTime(_member.RenewalDate);

                MemberNotes = new ObservableCollection<MemberNoteModel>(_member.MemberNotes.OrderByDescending(x => x.CreationDate).Select(x => new MemberNoteModel(x)));
            }
            else
                MemberNotes = new ObservableCollection<MemberNoteModel>();

        }

        #endregion Constructor

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(MemberModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Category")
                    if (Category == null)
                        Error = "Member Category can't be empty!";
                if (columnName == "Status")
                    if (Status == null)
                        Error = "Status can't be empty!";
                if (columnName == "StartDate")
                    if (StartDate == null || StartDate <= new DateTime(1900, 1, 1))
                        Error = "Start Date can't be empty or less by 1900!";
                if (columnName == "RenewalDate")
                    if (RenewalDate == null || RenewalDate <= new DateTime(1900, 1, 1))
                        Error = "Renewal Date can't be empty or less by 1900!";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
