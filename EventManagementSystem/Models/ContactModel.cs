using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class ContactModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly Contact _contact;
        private ContactTitle _contactTitle;
        private bool _isMale;
        private bool _isFemale;
        private Boolean _isDirty;
        private bool _includeInEmail;
        #endregion

        #region Properties

        [DataMember]
        public Contact Contact
        {
            get { return _contact; }
        }

        [DataMember]
        public ContactTitle Title
        {
            get { return _contactTitle; }
            set
            {
                if (_contactTitle == value) return;
                _contactTitle = value;
                _contact.Title = _contactTitle.ID;
                IsDirty = true;
                RaisePropertyChanged(() => Title);
            }
        }

        [DataMember]
        public string Address1
        {
            get { return _contact.Address1; }
            set
            {
                if (_contact.Address1 == value) return;
                _contact.Address1 = value;
                IsDirty = true;
                RaisePropertyChanged(() => Address1);
                RaisePropertyChanged(() => FullAddress);
                RaisePropertyChanged(() => FullAddressSepareted);
            }
        }


        [DataMember]
        public string Address2
        {
            get { return _contact.Address2; }
            set
            {
                if (_contact.Address2 == value) return;
                _contact.Address2 = value;
                IsDirty = true;
                RaisePropertyChanged(() => Address2);
                RaisePropertyChanged(() => FullAddress);
                RaisePropertyChanged(() => FullAddressSepareted);
            }
        }

        [DataMember]
        public string Address3
        {
            get { return _contact.Address3; }
            set
            {
                if (_contact.Address3 == value) return;
                _contact.Address3 = value;
                IsDirty = true;
                RaisePropertyChanged(() => Address3);
                RaisePropertyChanged(() => FullAddress);
                RaisePropertyChanged(() => FullAddressSepareted);
            }
        }

        [DataMember]
        public string City
        {
            get { return _contact.City; }
            set
            {
                if (_contact.City == value) return;
                _contact.City = value;
                IsDirty = true;
                RaisePropertyChanged(() => City);
                RaisePropertyChanged(() => FullAddress);
                RaisePropertyChanged(() => FullAddressSepareted);
            }
        }

        [DataMember]
        public string CompanyName
        {
            get { return _contact.CompanyName; }
            set
            {
                if (_contact.CompanyName == value) return;
                _contact.CompanyName = value;
                IsDirty = true;
                RaisePropertyChanged(() => CompanyName);
            }
        }

        [DataMember]
        public string Country
        {
            get { return _contact.Country; }
            set
            {
                if (_contact.Country == value) return;
                _contact.Country = value;
                IsDirty = true;
                RaisePropertyChanged(() => Country);
                RaisePropertyChanged(() => FullAddress);
                RaisePropertyChanged(() => FullAddressSepareted);
            }
        }

        [DataMember]
        public string Email
        {
            get { return _contact.Email; }
            set
            {
                if (_contact.Email == value) return;
                _contact.Email = value;
                IsDirty = true;
                RaisePropertyChanged(() => Email);
            }
        }


        [DataMember]
        public string FirstName
        {
            get { return _contact.FirstName; }
            set
            {
                if (_contact.FirstName == value) return;
                _contact.FirstName = value;
                IsDirty = true;
                RaisePropertyChanged(() => FirstName);
                RaisePropertyChanged(() => ContactName);
            }
        }

        [DataMember]
        public string LastName
        {
            get { return _contact.LastName; }
            set
            {
                if (_contact.LastName == value) return;
                _contact.LastName = value;
                IsDirty = true;
                RaisePropertyChanged(() => LastName);
                RaisePropertyChanged(() => ContactName);
            }
        }
        [DataMember]
        public string Phone1
        {
            get { return _contact.Phone1; }
            set
            {
                if (_contact.Phone1 == value) return;
                _contact.Phone1 = value;
                IsDirty = true;
                RaisePropertyChanged(() => Phone1);
                RaisePropertyChanged(() => AllTelNumbers);

            }
        }
        [DataMember]
        public string Phone2
        {
            get { return _contact.Phone2; }
            set
            {
                if (_contact.Phone2 == value) return;
                _contact.Phone2 = value;
                IsDirty = true;
                RaisePropertyChanged(() => Phone2);
                RaisePropertyChanged(() => AllTelNumbers);

            }
        }
        [DataMember]
        public string PostCode
        {
            get { return _contact.PostCode; }
            set
            {
                if (_contact.PostCode == value) return;
                _contact.PostCode = value;
                IsDirty = true;
                RaisePropertyChanged(() => PostCode);
                RaisePropertyChanged(() => FullAddress);
                RaisePropertyChanged(() => FullAddressSepareted);
            }
        }

        [DataMember]
        public Enums.Membership.Gender? Gender
        {
            get
            {
                if (_contact.Gender == 0)
                    return Enums.Membership.Gender.Male;
                if (_contact.Gender == 1)
                    return Enums.Membership.Gender.Female;
                return null;
            }
        }

        public Boolean IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                RaisePropertyChanged(() => IsDirty);
            }
        }


        public string ContactName
        {
            get { return string.Format("{0}  {1}", _contact.FirstName, _contact.LastName); }
        }

        public string FullAddressSepareted
        {
            get
            {
                return string.Join("\n", new List<string>()
                {
                    _contact.Address1,
                    _contact.Address2,
                    _contact.Address3,
                    _contact.City,
                    _contact.Country,
                    _contact.PostCode
                }.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        public string FullAddress
        {
            get
            {
                return string.Join(", ", new List<string>()
                {
                    _contact.Address1,
                    _contact.Address2,
                    _contact.Address3,
                    _contact.City,
                    _contact.Country,
                    _contact.PostCode
                }.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        public string AllTelNumbers
        {
            get
            {
                return string.Join(", ", new List<string>()
                {
                    _contact.Phone1,
                    _contact.Phone2
                }.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        public bool IsMale
        {
            get
            { return (_contact.Gender == Convert.ToInt32(Enums.Membership.Gender.Male)); }
            set
            {
                if (_isMale == value) return;
                _isMale = value;
                if (_isMale)
                    _contact.Gender = Convert.ToInt32(Enums.Membership.Gender.Male);
                IsDirty = true;
                RaisePropertyChanged(() => IsMale);
                RaisePropertyChanged(() => Gender);
            }
        }

        public bool IsFemale
        {
            get
            { return (_contact.Gender == Convert.ToInt32(Enums.Membership.Gender.Female)); }
            set
            {
                if (_isFemale == value) return;
                _isFemale = value;
                if (_isFemale)
                    _contact.Gender = Convert.ToInt32(Enums.Membership.Gender.Female);
                IsDirty = true;
                RaisePropertyChanged(() => IsFemale);
                RaisePropertyChanged(() => Gender);
            }
        }

        [DataMember]
        public string IsMember
        {
            get
            {
                if (_contact.Member != null)
                    return "Yes";
                return "No";
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
        #endregion

        #region Constructors

        public ContactModel(Contact contact)
        {
            _contact = contact;

            if (_contact != null)
            {
                _contactTitle = _contact.ContactTitle;
            }
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(ContactModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "FirstName")
                    if (string.IsNullOrWhiteSpace(FirstName))
                        Error = "First name can't be empty.";

                if (columnName == "LastName")
                    if (string.IsNullOrWhiteSpace(LastName))
                        Error = "Last name can't be empty.";


                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}