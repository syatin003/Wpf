using System;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventPaymentInvoiceModel : ModelBase
    {
        #region Fields

        private readonly EventPayment _eventPayment;
        private readonly Invoice _invoice;

        #endregion

        #region Constructor

        public EventPaymentInvoiceModel(Invoice invoice, Event event_)
        {
            _invoice = invoice;
            Event = new EventModel(event_);
        }

        public EventPaymentInvoiceModel(EventPayment payment, Event event_)
        {
            _eventPayment = payment;
            Event = new EventModel(event_);
        }

        #endregion

        #region Properties

        [DataMember]
        public EventPayment EventPayment
        {
            get { return _eventPayment; }
        }

        public EventModel Event
        {
            get; set;
        }

        public bool IsInvoice
        {
            get
            {
                return _invoice != null;
            }
        }

        [DataMember]
        public Invoice InnerInvoice
        {
            get { return _invoice; }
        }

        [DataMember]
        public double Amount
        {
            get
            {
                return _eventPayment.Amount;
            }
        }

        [DataMember]
        public double Bill
        {
            get
            {
                return _invoice.Amount;
            }
        }

        [DataMember]
        public DateTime Date
        {
            get
            {
                return _eventPayment != null ? _eventPayment.Date : _invoice.InvoiceDate;
            }
        }

        [DataMember]
        public DateTime EventDate
        {
            get
            {
                return Event.Date;
            }
        }

        [DataMember]
        public string EventName
        {
            get
            {
                return Event.Name;
            }
        }

        [DataMember]
        public string CameFrom
        {
            get
            {
                return _eventPayment != null ? _eventPayment.CameFrom : _invoice.CameFrom;
            }
        }

        [DataMember]
        public string Type
        {
            get
            {
                if (_eventPayment != null)
                {
                    if (_eventPayment.IsDeposit)
                        return "Deposit";
                    return "Payment";
                }
                return "Invoice";
            }
        }

        #endregion
    }
}
