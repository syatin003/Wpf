using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MembershipGroupModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly MembershipGroup _membershipGroup;

        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipGroup MembershipGroup
        {
            get { return _membershipGroup; }
        }

        [DataMember]
        public string Name
        {
            get { return _membershipGroup.Name; }
            set
            {
                if (_membershipGroup.Name == value) return;
                _membershipGroup.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        public string DaysAvailable
        {
            get
            {
                var list = new List<string>();
                if (_membershipGroup.IsMonAvailable)
                    list.Add("Mon");
                if (_membershipGroup.IsTuesAvailable)
                    list.Add("Tues");
                if (_membershipGroup.IsWedAvailable)
                    list.Add("Wed");
                if (_membershipGroup.IsThursAvailable)
                    list.Add("Thurs");
                if (_membershipGroup.IsFriAvailable)
                    list.Add("Fri");
                if (_membershipGroup.IsSatAvailable)
                    list.Add("Sat");
                if (_membershipGroup.IsSunAvailable)
                    list.Add("Sun");
                return string.Join(",", list.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
            }

        }

        #endregion Properties

        #region Constructor

        public MembershipGroupModel(MembershipGroup membershipGroup)
        {
            _membershipGroup = membershipGroup;
        }

        #endregion Constructor

        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(MembershipGroupModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
                if (columnName == "Name")
                {
                    if (string.IsNullOrWhiteSpace(Name))
                        Error = "Name can't be empty!";
                }
                //if (columnName == "DisplayMessage")
                //    if (DisplayMessage.Length > 64)
                //        Error = "Display Message can't be greater than 64 characters!";
                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
