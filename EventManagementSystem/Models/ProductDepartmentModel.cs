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
    public class ProductDepartmentModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly ProductDepartment _productDepartment;

        #endregion

        #region Properties

        [DataMember]
        public ProductDepartment ProductDepartment
        {
            get { return _productDepartment; }
        }


        public Till Till
        {
            get { return _productDepartment.Till; }
            set
            {

                if (_productDepartment.Till == value) return;
                _productDepartment.Till = value;
                RaisePropertyChanged(() => Till);
            }
        }


        [DataMember]
        public string Name
        {
            get { return _productDepartment.Department; }
            set
            {
                if (_productDepartment.Department == value) return;
                _productDepartment.Department = value;
                RaisePropertyChanged(() => Name);
            }
        }

        [DataMember]
        public Int32 RecordID
        {
            get { return _productDepartment.Record; }
            set
            {
                if (_productDepartment.Record == value) return;
                _productDepartment.Record = value;
                RaisePropertyChanged(() => RecordID);
            }
        }

        #endregion

        #region Constructors

        public ProductDepartmentModel(ProductDepartment productDepartment)
        {
            _productDepartment = productDepartment;
        }

        #endregion

        #region IDataError Implementation

        public bool HasErrors
        {
            get { return typeof(ProductDepartmentModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
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
                        Error = "Department Name can't be empty";

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
