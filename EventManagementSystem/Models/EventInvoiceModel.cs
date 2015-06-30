using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventInvoiceModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private EventInvoice _eventInvoice;
        private ObservableCollection<EventBookedProductModel> _eventBookedProducts;

        #endregion

        #region Properies

        [DataMember]
        public EventInvoice EventInvoice
        {
            get { return _eventInvoice; }
            set { _eventInvoice = value; }
        }

        [DataMember]
        public ObservableCollection<EventBookedProductModel> EventBookedProducts
        {
            get { return _eventBookedProducts; }
            set
            {
                if (_eventBookedProducts == value) return;
                _eventBookedProducts = value;
                RaisePropertyChanged(() => EventBookedProducts);
            }
        }

        [IgnoreDataMember]
        public bool HasValidProducts
        {
            get { return _eventBookedProducts.Any(x => x.Quantity >= 0 && x.Product != null); }
        }

        #endregion

        #region Constructor

        public EventInvoiceModel(EventInvoice invoice)
        {
            _eventInvoice = invoice;
            EventBookedProducts = new ObservableCollection<EventBookedProductModel>();
        }

        #endregion

        #region Methods

        public void Refresh()
        {
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(EventInvoiceModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "HasValidProducts")
                    if (!HasValidProducts)
                        Error = "Please add at least one product";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
