using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MembershipGroupAgeModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly MembershipGroupAge _membershipGroupAge;

        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipGroupAge MembershipGroupAge
        {
            get { return _membershipGroupAge; }
        }

        [DataMember]
        public string Name
        {
            get { return _membershipGroupAge.Name; }
            set
            {
                if (_membershipGroupAge.Name == value) return;
                _membershipGroupAge.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        [DataMember]
        public DateTime CalculationDate
        {
            get { return _membershipGroupAge.CalculationDate; }
            set
            {
                if (_membershipGroupAge.CalculationDate == value) return;
                _membershipGroupAge.CalculationDate = value;
                RaisePropertyChanged(() => CalculationDate);
            }
        }
        #endregion Properties

        #region Constructor

        public MembershipGroupAgeModel(MembershipGroupAge membershipGroupAge)
        {
            _membershipGroupAge = membershipGroupAge;

        }

        #endregion Constructor

        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(MembershipGroupAgeModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
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
                if (columnName == "CalculationDate")
                    if (CalculationDate <= new DateTime(1900, 1, 1))
                        Error = "Date can't be empty or less by 1900!";
                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
