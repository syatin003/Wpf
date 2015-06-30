using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MembershipGroupStyleModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly MembershipGroupStyle _membershipGroupStyle;


        #endregion Fields

        #region Properties

        [DataMember]
        public MembershipGroupStyle MembershipGroupStyle
        {
            get { return _membershipGroupStyle; }
        }
        [DataMember]
        public string Name
        {
            get { return _membershipGroupStyle.Name; }
            set
            {
                if (_membershipGroupStyle.Name == value) return;
                _membershipGroupStyle.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        #endregion Properties

        #region Constructor

        public MembershipGroupStyleModel(MembershipGroupStyle membershipGroupStyle)
        {
            _membershipGroupStyle = membershipGroupStyle;

        }

        #endregion Constructor

        #region IDataErrorInfo Properties

        /// <summary>
        /// Indicates whenever the model has errors
        /// </summary>
        public bool HasErrors
        {
            get { return typeof(MembershipGroupStyleModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
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
