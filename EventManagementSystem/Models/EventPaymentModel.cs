using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventPaymentModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly EventPayment _eventPayment;
        #endregion

        #region Properties

        [DataMember]
        public EventPayment EventPayment
        {
            get { return _eventPayment; }
        }

        [DataMember]
        public double Amount
        {
            get { return _eventPayment.Amount; }
            set
            {
                if (_eventPayment.Amount == value) return;
                _eventPayment.Amount = value;
                RaisePropertyChanged(() => Amount);
            }
        }

        [DataMember]
        public User User
        {
            get { return _eventPayment.User; }
            set
            {
                if (_eventPayment.User == value) return;
                _eventPayment.User = value;
                RaisePropertyChanged(() => User);
            }
        }

        [DataMember]
        public PaymentMethod PaymentMethod
        {
            get { return _eventPayment.PaymentMethod; }
            set
            {
                if (_eventPayment.PaymentMethod == value) return;
                _eventPayment.PaymentMethod = value;
                RaisePropertyChanged(() => PaymentMethod);
            }
        }

        [DataMember]
        public string Notes
        {
            get { return _eventPayment.Notes; }
            set
            {
                if (_eventPayment.Notes == value) return;
                _eventPayment.Notes = value;
                RaisePropertyChanged(() => Notes);
            }
        }

        [DataMember]
        public DateTime Date
        {
            get { return _eventPayment.Date; }
            set
            {
                if (_eventPayment.Date == value) return;
                _eventPayment.Date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        [DataMember]
        public bool IsDeposit
        {
            get
            {
                return _eventPayment.IsDeposit;
            }
            set
            {
                if (_eventPayment.IsDeposit == value) return;
                _eventPayment.IsDeposit = value;

                RaisePropertyChanged(() => IsDeposit);
            }
        }
        [DataMember]
        public string PaymentType
        {
            get
            {
                if (_eventPayment.IsDeposit)
                    return "Deposit";
                else
                    return "Actual";
            }
        }

        #endregion

        #region Constructor

        public EventPaymentModel(EventPayment payment)
        {
            _eventPayment = payment;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => Amount);
            RaisePropertyChanged(() => PaymentMethod);
            RaisePropertyChanged(() => User);
            RaisePropertyChanged(() => Notes);
            RaisePropertyChanged(() => IsDeposit);
            RaisePropertyChanged(() => Date);
            RaisePropertyChanged(() => PaymentType);
        }

        #endregion

        #region IDataErrorInfo Members

        public bool HasErrors
        {
            get { return typeof(EventPaymentModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Amount")
                    if (Amount <= 0)
                        Error = "Amount can't be less that zero.";

                if (columnName == "User")
                    if (User == null)
                        Error = "User field can't be empty.";

                if (columnName == "PaymentMethod")
                    if (PaymentMethod == null)
                        Error = "Payment method can't be empty.";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}