using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MembershipLinkTypeModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly MembershipLinkType _membershipLinkType;

        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipLinkType MembershipLinkType
        {
            get { return _membershipLinkType; }
        }

        [DataMember]
        public bool AutoRenew
        {
            get { return _membershipLinkType.AutoRenew; }
            set
            {
                if (_membershipLinkType.AutoRenew == value) return;
                _membershipLinkType.AutoRenew = value;
                RaisePropertyChanged(() => AutoRenew);
            }
        }

        [DataMember]
        public bool AutoResign
        {
            get { return _membershipLinkType.AutoResign; }
            set
            {
                if (_membershipLinkType.AutoResign == value) return;
                _membershipLinkType.AutoResign = value;
                RaisePropertyChanged(() => AutoResign);
            }
        }
        [DataMember]
        public MembershipCategory AutoRenewCategory
        {
            get { return _membershipLinkType.MembershipCategory; }
            set
            {
                if (_membershipLinkType.MembershipCategory == value) return;
                _membershipLinkType.MembershipCategory = value;
                if (_membershipLinkType.MembershipCategory != null)
                    _membershipLinkType.AutoRenewCategoryID = _membershipLinkType.MembershipCategory.ID;
                RaisePropertyChanged(() => AutoRenewCategory);
            }
        }

        [DataMember]
        public MembershipCategory AutoResignCategory
        {
            get { return _membershipLinkType.MembershipCategory1; }
            set
            {
                if (_membershipLinkType.MembershipCategory1 == value) return;
                _membershipLinkType.MembershipCategory1 = value;
                if (_membershipLinkType.MembershipCategory1 != null)
                    _membershipLinkType.AutoResignCategoryID = _membershipLinkType.MembershipCategory1.ID;
                RaisePropertyChanged(() => AutoResignCategory);
            }
        }
        #endregion Properties

        #region Constructor

        public MembershipLinkTypeModel(MembershipLinkType membershipLinkType)
        {
            _membershipLinkType = membershipLinkType;

        }

        #endregion Constructor
        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(MembershipLinkTypeModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
                if (columnName == "AutoRenew")
                    if (AutoRenew)
                        if (AutoRenewCategory == null)
                            Error = "Auto renew category can't be empty!";
                if (columnName == "AutoResign")
                    if (AutoResign)
                        if (AutoResignCategory == null)
                            Error = "Auto resign category can't be empty!";
                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
