using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class ProductGroupModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly ProductGroup _productGroup;

        #endregion

        #region Properties

        [DataMember]
        public ProductGroup ProductGroup
        {
            get { return _productGroup; }
        }


        public Till Till
        {
            get { return _productGroup.Till; }
            set
            {

                if (_productGroup.Till == value) return;
                _productGroup.Till = value;
                RaisePropertyChanged(() => Till);
            }
        }


        [DataMember]
        public string Name
        {
            get { return _productGroup.GroupName; }
            set
            {
                if (_productGroup.GroupName == value) return;
                _productGroup.GroupName = value;
                RaisePropertyChanged(() => Name);
            }
        }

        [DataMember]
        public Int32 RecordID
        {
            get { return _productGroup.Record; }
            set
            {
                if (_productGroup.Record == value) return;
                _productGroup.Record = value;
                RaisePropertyChanged(() => RecordID);
            }
        }

        #endregion

        #region Constructors

        public ProductGroupModel(ProductGroup productGroup)
        {
            _productGroup = productGroup;
        }

        #endregion

        #region IDataError Implementation

        public bool HasErrors
        {
            get { return typeof(ProductGroupModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Till")
                    if (Till == null)
                        Error = "Please select the Till.";

                if (columnName == "Name")
                    if (string.IsNullOrWhiteSpace(Name))
                        Error = "Group Name can't be empty";

                if (columnName == "RecordID")
                    if (string.IsNullOrWhiteSpace(RecordID.ToString()))
                        Error = "Record Id can't be empty";
                    else if (RecordID < 1 || RecordID > 256)
                        Error = "Record Id must be between 1 and 256";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
