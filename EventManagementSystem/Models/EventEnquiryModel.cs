using System;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.Models
{
    public class EventEnquiryModel : ModelBase
    {
        #region Fields

        private  EventModel _event;
        private  EnquiryModel _enquiry;

        #endregion

        #region Properties

        public EventModel Event
        {
            get { return _event; }
            set
            {
                if(_event == value) return;
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }

        public EnquiryModel Enquiry
        {
            get { return _enquiry; }
            set
            {
                if (_enquiry == value) return;
                _enquiry = value;
                RaisePropertyChanged(() => Enquiry);
            }
        }

        public string Name
        {
            get
            {
                return _enquiry == null ? _event.Name : String.IsNullOrEmpty(_enquiry.Name) ? _enquiry.EnquiryString : _enquiry.Name;
            }
        }

        public string EventType
        {
            get
            {
                return _enquiry == null ? _event.EventContactType : _enquiry.EventTypeName;
            }
        }

        public DateTime? Date
        {
            get
            {
                return _enquiry == null ? _event.Date : _enquiry.Date;
            }
        }

        public int? Places
        {
            get
            {
                return _enquiry == null ? _event.Places : _enquiry.Places;
            }
        }

        public double TotalValue
        {
            get
            {
                return _enquiry == null ? _event.EventPrice : _enquiry.Value;
            }
        }

        public string Status
        {
            get
            {
                return _enquiry == null ? _event.EventStatus.Name : _enquiry.EnquiryDetailStatus;
            }
        }

        #endregion

        #region Constructor

        public EventEnquiryModel(EventModel model)
        {
            _event = model;
        }

        public EventEnquiryModel(EnquiryModel model)
        {
            _enquiry = model;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            //RaisePropertyChanged(() => Direction);
            //RaisePropertyChanged(() => ActivityType);
            //RaisePropertyChanged(() => Details);
            //RaisePropertyChanged(() => Length);
        }

        #endregion
    }
}
