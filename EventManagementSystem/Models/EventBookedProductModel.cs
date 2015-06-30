using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventBookedProductModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private Product _product;

        #endregion


        #region Properties

        public EventChargeModel EventCharge { get; set; }

        [DataMember]
        public EventBookedProduct EventBookedProduct { get; set; }

        [DataMember]
        public double TotalPrice
        {
            get
            {
                if (IncVat)
                    return (Quantity * Price);

                return (Quantity * Price) * (1 - EventBookedProduct.Product.ProductVATRate.Rate / 1.2);
            }
        }

        [DataMember]
        public int Quantity
        {
            get { return EventBookedProduct.Quantity; }
            set
            {
                if (EventBookedProduct.Quantity == value) return;
                EventBookedProduct.Quantity = value;
                RaisePropertyChanged(() => Quantity);

                RaisePropertyChanged(() => TotalPrice);

                if (EventCharge != null)
                    EventCharge.Quantity = value;
            }
        }

        [DataMember]
        public double Price
        {
            get { return EventBookedProduct.Price; }
            set
            {
                if (EventBookedProduct.Price == value) return;
                EventBookedProduct.Price = value;
                RaisePropertyChanged(() => Price);

                RaisePropertyChanged(() => TotalPrice);

                if (EventCharge != null)
                    EventCharge.Price = value;
            }
        }

        [DataMember]
        public Product Product
        {
            get { return _product; }
            set
            {
                if (_product == value) return;
                _product = value;
                EventBookedProduct.ProductID = _product.ID;
                RaisePropertyChanged(() => Product);

                Price = _product.GrossPrice;

                if (EventCharge != null)
                    EventCharge.Product = _product;
            }
        }

        [IgnoreDataMember]
        public bool IsEditable
        {
            get { return !EventBookedProduct.EventCharge.IsCommited; }
        }

        [DataMember]
        public bool GroupByProductGroup { get; set; }

        [DataMember]
        public bool IncVat { get; set; }

        [DataMember]
        public string ProductGroupOrDepartment
        {
            get
            {
                if (GroupByProductGroup)
                    return EventBookedProduct.Product.ProductGroup.GroupName;
                else
                    return EventBookedProduct.Product.ProductDepartment.Department;
            }
        }

        [DataMember]
        public DateTime EventDate
        {
            get { return EventBookedProduct.Event.Date; }
        }

        [DataMember]
        public string EventStatus
        {
            get { return EventBookedProduct.Event.EventStatus.Name; }
        }

        [DataMember]
        public string EventType
        {
            get { return EventBookedProduct.Event.EventType.Name; }
        }

        #endregion

        #region Constructor

        public EventBookedProductModel(EventBookedProduct eventBookedProduct)
        {
            IncVat = true;
            EventBookedProduct = eventBookedProduct;

            _product = eventBookedProduct.Product;

            EventCharge = new EventChargeModel(EventBookedProduct.EventCharge);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(EventBookedProduct).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Quantity")
                    if (Quantity < 0)
                        Error = "Quantity can't be less than zero";

                if (columnName == "Product")
                    if (Product == null)
                        Error = "Product can't be empty";

                if (columnName == "Price")
                    if (Price < 0)
                        Error = "Price can't be less than zero";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}