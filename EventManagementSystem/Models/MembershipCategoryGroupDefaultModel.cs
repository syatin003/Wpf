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
    public class MembershipCategoryGroupDefaultModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly MembershipCategoryGroupDefault _membershipCategoryGroupDefault;

        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipCategoryGroupDefault MembershipCategoryGroupDefault
        {
            get { return _membershipCategoryGroupDefault; }
        }

        [DataMember]
        public string Name
        {
            get { return _membershipCategoryGroupDefault.Name; }
            set
            {
                if (_membershipCategoryGroupDefault.Name == value) return;
                _membershipCategoryGroupDefault.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        public string DaysAvailable
        {
            get
            {
                var list = new List<string>();
                if (_membershipCategoryGroupDefault.IsMonAvailable)
                    list.Add("Mon");
                if (_membershipCategoryGroupDefault.IsTuesAvailable)
                    list.Add("Tues");
                if (_membershipCategoryGroupDefault.IsWedAvailable)
                    list.Add("Wed");
                if (_membershipCategoryGroupDefault.IsThursAvailable)
                    list.Add("Thurs");
                if (_membershipCategoryGroupDefault.IsFriAvailable)
                    list.Add("Fri");
                if (_membershipCategoryGroupDefault.IsSatAvailable)
                    list.Add("Sat");
                if (_membershipCategoryGroupDefault.IsSunAvailable)
                    list.Add("Sun");
                return string.Join(",", list.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
            }

        }

        #endregion Properties

        #region Constructor

        public MembershipCategoryGroupDefaultModel(MembershipCategoryGroupDefault membershipCategoryGroupDefault)
        {
            _membershipCategoryGroupDefault = membershipCategoryGroupDefault;
        }

        #endregion Constructor

        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(MembershipCategoryGroupDefaultModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
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
                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
