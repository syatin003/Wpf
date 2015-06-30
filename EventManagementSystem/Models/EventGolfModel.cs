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
    public class EventGolfModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private ObservableCollection<EventBookedProductModel> _eventBookedProducts;
        private Golf _golf;
        private GolfHole _golfHole;

        #endregion

        #region Properties

        [DataMember]
        public EventGolf EventGolf { get; set; }

        [DataMember]
        public DateTime Time
        {
            get { return EventGolf.Time; }
            set
            {
                if (EventGolf.Time == value) return;
                EventGolf.Time = value;
                RaisePropertyChanged(() => Time);
                RaisePropertyChanged(() => TimeEx);
            }
        }

        [IgnoreDataMember]
        public DateTime TimeEx
        {
            get
            {
                return new DateTime(
                    EventGolf.Event.Date.Year,
                    EventGolf.Event.Date.Month,
                    EventGolf.Event.Date.Day,
                    EventGolf.Time.Hour,
                    EventGolf.Time.Minute,
                    0);
            }
        }

        [IgnoreDataMember]
        public DateTime EndTimeEx
        {
            get
            {
                var endTime = Time.AddMinutes(EventGolf.Golf.TimeInterval.TotalMinutes * EventGolf.Slots);

                return new DateTime(
                    EventGolf.Event.Date.Year,
                    EventGolf.Event.Date.Month,
                    EventGolf.Event.Date.Day,
                    endTime.Hour,
                    endTime.Minute,
                    0);
            }
        }

        [DataMember]
        public GolfHole GolfHole
        {
            get { return _golfHole; }
            set
            {
                if (_golfHole == value) return;
                _golfHole = value;
                EventGolf.HoleID = _golfHole.ID;
                RaisePropertyChanged(() => GolfHole);
            }
        }

        [DataMember]
        public Golf Golf
        {
            get { return _golf; }
            set
            {
                if (_golf == value) return;
                _golf = value;
                EventGolf.TeeID = _golf.ID;
                RaisePropertyChanged(() => Golf);
                RaisePropertyChanged(() => Time);
            }
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
            get { return (_eventBookedProducts.Any(x => x.Quantity >= 0 && x.Product != null) && (!_eventBookedProducts.Any(x => x.Quantity < 0 || x.Product == null))); }
            //get { return _eventBookedProducts.Any(x => x.Quantity > 0 && x.Product != null); }
        }

        #endregion

        #region Constructors

        public EventGolfModel(EventGolf golf)
        {
            EventGolf = golf;
            EventBookedProducts = new ObservableCollection<EventBookedProductModel>();

            _golfHole = EventGolf.GolfHole;
            _golf = EventGolf.Golf;

        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => Time);
            RaisePropertyChanged(() => Golf);
            RaisePropertyChanged(() => GolfHole);
        }

        #endregion

        #region IDataErrorInfo
        public bool HasErrors
        {
            get { return typeof(EventGolfModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "GolfHole")
                    if (GolfHole == null)
                        Error = "Hole can't be empty.";

                if (columnName == "Golf")
                    if (Golf == null)
                        Error = "Golf can't be empty.";

                if (columnName == "Time")
                    if (Golf != null)
                    {
                        var ticks = (new TimeSpan(Time.Hour, Time.Minute, Time.Second)).Ticks;
                        if (ticks < Golf.StartTime.Ticks || ticks > Golf.EndTime.Ticks)
                            Error = "Time can't be out of range";
                    }

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