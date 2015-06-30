using EventManagementSystem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class TillModel : ModelBase, IDataErrorInfo
    {

        private readonly Till _till;
        private bool _isMaster;
        private TillDivision _tillDivision;

        [DataMember]
        public Till Till
        {
            get { return _till; }
        }

        [DataMember]
        public String Name
        {
            get { return _till.Name; }
            set
            {
                if (_till.Name == value) return;
                _till.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        [DataMember]
        public String IPAddress
        {
            get { return _till.IPAddress; }
            set
            {
                if (_till.IPAddress == value) return;
                _till.IPAddress = value;
                RaisePropertyChanged(() => IPAddress);
            }
        }


        [DataMember]
        public Int32 Identifier
        {
            get { return _till.Identifier; }
            set
            {
                if (_till.Identifier == value) return;
                _till.Identifier = value;
                RaisePropertyChanged(() => Identifier);
            }
        }

        [DataMember]
        public bool IsMaster
        {
            get
            { return _isMaster; }
            set
            {
                if (_isMaster == value) return;
                _isMaster = value;
                RaisePropertyChanged(() => IsMaster);
            }
        }
        public TillDivision TillDivision
        {
            get { return _tillDivision; }
            set
            {
                if (_tillDivision == value) return;
                _tillDivision = value;
                _till.TillDivision = _tillDivision;
                _till.DivisionID = _tillDivision.ID;
                RaisePropertyChanged(() => TillDivision);
            }
        }

        #region Constructor

        public TillModel(Till till)
        {
            _till = till;
            if (_till.TillDivision != null)
                _tillDivision = _till.TillDivision;
            if (_till.TillDivision == null || _till.TillDivision.MasterTillID == null)
                IsMaster = false;
            else if (_till.TillDivision.MasterTillID == _till.ID)
                IsMaster = true;
            else
                IsMaster = false;
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(TillModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "TillDivision")
                {
                    if (TillDivision == null)
                        Error = "Till Division can't be empty!";
                }

                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        Error = "Name can't be empty!";
                }

                if (columnName == "IPAddress")
                {
                    if (string.IsNullOrEmpty(IPAddress))
                        Error = "IP Address can't be empty!";
                    else if (!IsValidIPAddress(IPAddress))
                        Error = "Please enter valid IP Address.";
                }
                if (columnName == "Identifier")
                {
                    if (string.IsNullOrEmpty(Identifier.ToString()))
                        Error = "Identifier can't be empty!";
                    else if (Identifier <= 0 || Identifier > 100)
                        Error = "Identifier must be between 1 and 100!";
                }
                return Error;
            }
        }

        public string Error { get; protected set; }

        #endregion IDataErrorInfo

        #region methods


        private Boolean IsValidIPAddress(string IPAddress)
        {
            Regex myRegex = new Regex(@"^(([01]?\d\d?|2[0-4]\d|25[0-5])\.){3}([01]?\d\d?|25[0-5]|2[0-4]\d)$");
            return myRegex.IsMatch(IPAddress);
        }
        #endregion methods
    }
}
