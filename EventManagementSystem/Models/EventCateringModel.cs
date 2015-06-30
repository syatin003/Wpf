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
    public class EventCateringModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private EventCatering _eventCatering;
        private ObservableCollection<EventBookedProductModel> _eventBookedProducts;
        private Room _room;

        #endregion

        #region Properties

        [DataMember]
        public EventCatering EventCatering
        {
            get { return _eventCatering; }
            set { _eventCatering = value; }
        }

        [IgnoreDataMember]
        public DateTime StartTimeEx
        {
            get
            {
                var cateringStartTime = new DateTime(
                            _eventCatering.Event.Date.Year,
                            _eventCatering.Event.Date.Month,
                            _eventCatering.Event.Date.Day,
                            _eventCatering.StartTime.Hour,
                            _eventCatering.StartTime.Minute,
                            0);
                if (_eventCatering.Room.EndTime < _eventCatering.Room.StartTime)
                    if ((new TimeSpan(_eventCatering.StartTime.Hour, _eventCatering.StartTime.Minute, 0)).Ticks < (new TimeSpan(_eventCatering.Room.StartTime.Hours, _eventCatering.Room.StartTime.Minutes, _eventCatering.Room.StartTime.Seconds)).Ticks)
                        return cateringStartTime.AddDays(1);
                return cateringStartTime;

            }
        }

        [IgnoreDataMember]
        public DateTime EndTimeEx
        {
            get
            {
                var cateringEndTime = new DateTime(
                            _eventCatering.Event.Date.Year,
                            _eventCatering.Event.Date.Month,
                            _eventCatering.Event.Date.Day,
                            _eventCatering.EndTime.Hour,
                            _eventCatering.EndTime.Minute,
                            0);
                if (_eventCatering.Room.EndTime < _eventCatering.Room.StartTime)
                    if ((new TimeSpan(_eventCatering.EndTime.Hour, _eventCatering.EndTime.Minute, 0)).Ticks < (new TimeSpan(_eventCatering.Room.StartTime.Hours, _eventCatering.Room.StartTime.Minutes, _eventCatering.Room.StartTime.Seconds)).Ticks)
                        return cateringEndTime.AddDays(1);
                return cateringEndTime;
            }
        }

        [DataMember]
        public DateTime Time
        {
            get { return _eventCatering.Time; }
            set
            {
                if (_eventCatering.Time == value) return;
                _eventCatering.Time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        [DataMember]
        public DateTime StartTime
        {
            get { return _eventCatering.StartTime; }
            set
            {
                if (_eventCatering.StartTime == value) return;
                _eventCatering.StartTime = value;
                RaisePropertyChanged(() => StartTime);
                RaisePropertyChanged(() => EndTime);
            }
        }

        [DataMember]
        public DateTime EndTime
        {
            get { return _eventCatering.EndTime; }
            set
            {
                if (_eventCatering.EndTime == value) return;
                _eventCatering.EndTime = value;
                RaisePropertyChanged(() => EndTime);
                RaisePropertyChanged(() => StartTime);
            }
        }

        [DataMember]
        public Room Room
        {
            get { return _room; }
            set
            {
                if (_room == value) return;
                _room = value;
                _eventCatering.RoomID = _room.ID;
                RaisePropertyChanged(() => Room);
                RaisePropertyChanged(() => Time);
                RaisePropertyChanged(() => StartTime);
                RaisePropertyChanged(() => EndTime);
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
        }

        #endregion

        #region Constructors

        public EventCateringModel(EventCatering eventCatering)
        {
            _eventCatering = eventCatering;
            EventBookedProducts = new ObservableCollection<EventBookedProductModel>();

            _room = _eventCatering.Room;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => Room);
            RaisePropertyChanged(() => Time);
            RaisePropertyChanged(() => StartTime);
            RaisePropertyChanged(() => EndTime);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(EventCateringModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                long timeTicks;
                long startTimeTicks;
                long endTimeTicks;
                long roomStartTimeTicks;
                long roomEndTimeTicks;
                Error = string.Empty;
                if (columnName == "Room")
                    if (Room == null)
                        Error = "Room can't be empty";
                if (Room != null)
                {
                    SetTimeValuesByRoom(out timeTicks, out startTimeTicks, out endTimeTicks, out roomStartTimeTicks, out roomEndTimeTicks);
                    if (columnName == "Time")
                    {
                        //if (timeTicks == 0)
                        //    Error = "Please select Time.";
                        //else 
                        if (timeTicks < roomStartTimeTicks || timeTicks > roomEndTimeTicks)
                            Error = "Please select Valid Time.";
                    }
                    if (columnName == "StartTime")
                    {
                        //if (startTimeTicks == 0)
                        //    Error = "Please select Start Time.";
                        //else 
                        if (startTimeTicks < roomStartTimeTicks || startTimeTicks > roomEndTimeTicks)
                            Error = "Please select Valid Start Time.";
                        else if (timeTicks < startTimeTicks)
                            Error = "Start Time can't be greater than Time";
                        else if (startTimeTicks < roomStartTimeTicks || startTimeTicks > roomEndTimeTicks)
                            Error = "Please select Valid Start Time.";
                        else if (timeTicks < startTimeTicks)
                            Error = "Start Time can't be greater than Time";
                    }

                    if (columnName == "EndTime")
                    {
                        //if (endTimeTicks == 0)
                        //    Error = "Please select End Time.";
                        //else 
                        if (endTimeTicks < roomStartTimeTicks || endTimeTicks > roomEndTimeTicks)
                            Error = "Please select Valid End Time";
                        else if (startTimeTicks > endTimeTicks)
                            Error = "End Time can't be less than Start Time";
                        else if (startTimeTicks == endTimeTicks)
                            Error = "End Time can't be equal to Start Time";
                    }
                }

                if (columnName == "HasValidProducts")
                    if (!HasValidProducts)
                        Error = "Please add at least one product";

                return Error;
            }
        }

        private void SetTimeValuesByRoom(out long timeTicks, out long startTimeTicks, out long endTimeTicks, out long roomStartTimeTicks, out long roomEndTimeTicks)
        {
            timeTicks = (new TimeSpan(Time.Hour, Time.Minute, Time.Second)).Ticks;
            startTimeTicks = (new TimeSpan(StartTime.Hour, StartTime.Minute, StartTime.Second)).Ticks;
            endTimeTicks = (new TimeSpan(EndTime.Hour, EndTime.Minute, EndTime.Second)).Ticks;
            roomStartTimeTicks = Room.StartTime.Ticks;
            roomEndTimeTicks = Room.EndTime.Ticks;
            if (Room.EndTime < Room.StartTime)
            {
                roomEndTimeTicks += new TimeSpan(1, 0, 0, 0).Ticks;
                if (timeTicks <= Room.EndTime.Ticks)
                    timeTicks += new TimeSpan(1, 0, 0, 0).Ticks;
                if (startTimeTicks <= Room.EndTime.Ticks)
                    startTimeTicks += new TimeSpan(1, 0, 0, 0).Ticks;
                if (endTimeTicks <= Room.EndTime.Ticks)
                    endTimeTicks += new TimeSpan(1, 0, 0, 0).Ticks;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}