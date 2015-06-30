using System;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class InvoiceModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private EventModel _eventModel;

        #endregion

        #region Properties

        public Invoice InnerInvoice { get; set; }

        public EventModel EventModel
        {
            get { return _eventModel; }
            set
            {
                _eventModel = value;
                InnerInvoice.Event = _eventModel.Event;
            }
        }

        public int InvoiceNumber
        {
            get { return InnerInvoice.InvoiceNumber; }
            set
            {
                if (InnerInvoice.InvoiceNumber == value) return;
                InnerInvoice.InvoiceNumber = value;
                RaisePropertyChanged(() => InvoiceNumber);
            }
        }

        public DateTime InvoiceDate
        {
            get { return InnerInvoice.InvoiceDate; }
            set
            {
                if (InnerInvoice.InvoiceDate == value) return;
                InnerInvoice.InvoiceDate = value;
                RaisePropertyChanged(() => InvoiceDate);
            }
        }

        public DateTime PaymentDue
        {
            get { return InnerInvoice.PaymentDue; }
            set
            {
                if (InnerInvoice.PaymentDue == value) return;
                InnerInvoice.PaymentDue = value;
                RaisePropertyChanged(() => PaymentDue);
            }
        }

        public string InvoiceAddress
        {
            get { return InnerInvoice.InvoiceAddress; }
            set
            {
                if (InnerInvoice.InvoiceAddress == value) return;
                InnerInvoice.InvoiceAddress = value;
                RaisePropertyChanged(() => InvoiceAddress);
            }
        }

        public string Notes
        {
            get { return InnerInvoice.Notes; }
            set
            {
                if (InnerInvoice.Notes == value) return;
                InnerInvoice.Notes = value;
                RaisePropertyChanged(() => Notes);
            }
        }

        #endregion

        #region Constructor

        public InvoiceModel(Invoice invoice)
        {
            InnerInvoice = invoice;
        }

        #endregion

        #region IDataErrorInfo implementation

        public bool HasErrors
        {
            get { return typeof(InvoiceModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "InvoiceNumber")
                    if (InvoiceNumber <= 0)
                        Error = "Invoice number can't be empty.";

                if (columnName == "InvoiceAddress")
                    if (string.IsNullOrWhiteSpace(InvoiceAddress))
                        Error = "Invoice address can't be empty.";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion

    }
}
