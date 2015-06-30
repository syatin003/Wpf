using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventChargeModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly EventCharge _charge;
        private Product _product;

        #endregion

        #region Properties

        [DataMember]
        public EventCharge EventCharge
        {
            get { return _charge; }
        }

        [DataMember]
        public Product Product
        {
            get { return _product; }
            set
            {
                if (_product == value) return;
                _product = value;
                _charge.ProductID = _product.ID;
                Price = _product.GrossPrice;

                RaisePropertyChanged(() => Product);
            }
        }

        [DataMember]
        public int Quantity
        {
            get { return _charge.Quantity; }
            set
            {
                if (_charge.Quantity == value) return;
                _charge.Quantity = value;

                RaisePropertyChanged(() => Quantity);
                RaisePropertyChanged(() => TotalPrice);
            }
        }

        [DataMember]
        public double Price
        {
            get { return _charge.Price; }
            set
            {
                if (_charge.Price == value) return;
                _charge.Price = value;
                RaisePropertyChanged(() => Price);
                RaisePropertyChanged(() => TotalPrice);
            }
        }

        [DataMember]
        public bool IsCommited
        {
            get { return _charge.IsCommited; }
            set
            {
                if (_charge.IsCommited == value) return;
                _charge.IsCommited = value;
                RaisePropertyChanged(() => IsCommited);
            }
        }

        public double TotalPrice
        {
            get { return Price * Quantity; }
        }

        #endregion

        #region Constructor

        public EventChargeModel(EventCharge charge)
        {
            _charge = charge;

            _product = _charge.Product;

        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(EventChargeModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Product")
                    if (Product == null)
                        Error = "Product can't be empty.";

                if (columnName == "Quantity")
                    if (Quantity <= 0)
                        Error = "Quantity can't be empty or zero.";

                if (columnName == "Price")
                    if (Price <= 0)
                        Error = "Price can't be empty or zero.";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion

    }
}
