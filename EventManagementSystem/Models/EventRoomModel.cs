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
    public class EventRoomModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private EventRoom _eventRoom;
        private ObservableCollection<EventBookedProductModel> _eventBookedProducts;
        private Room _room;

        #endregion

        #region Properties

        [DataMember]
        public EventRoom EventRoom
        {
            get { return _eventRoom; }
            set { _eventRoom = value; }
        }

        [IgnoreDataMember]
        public DateTime StartTimeEx
        {
            get
            {

                var roomStartTime = new DateTime(
                    _eventRoom.Event.Date.Year,
                    _eventRoom.Event.Date.Month,
                    _eventRoom.Event.Date.Day,
                    _eventRoom.StartTime.Hour,
                    _eventRoom.StartTime.Minute,
                    0);
                if (_eventRoom.Room.EndTime < _eventRoom.Room.StartTime)
                    if ((new TimeSpan(_eventRoom.StartTime.Hour, _eventRoom.StartTime.Minute, 0)).Ticks < (new TimeSpan(_eventRoom.Room.StartTime.Hours, _eventRoom.Room.StartTime.Minutes, _eventRoom.Room.StartTime.Seconds)).Ticks)
                        return roomStartTime.AddDays(1);
                return roomStartTime;
            }
        }

        [IgnoreDataMember]
        public DateTime EndTimeEx
        {
            get
            {
                var roomEndTime = new DateTime(
                    _eventRoom.Event.Date.Year,
                    _eventRoom.Event.Date.Month,
                    _eventRoom.Event.Date.Day,
                    _eventRoom.StartTime.Hour,
                    _eventRoom.StartTime.Minute,
                    0);
                if (_eventRoom.Room.EndTime < _eventRoom.Room.StartTime)
                    if ((new TimeSpan(_eventRoom.EndTime.Hour, _eventRoom.EndTime.Minute, 0)).Ticks < (new TimeSpan(_eventRoom.Room.StartTime.Hours, _eventRoom.Room.StartTime.Minutes, _eventRoom.Room.StartTime.Seconds)).Ticks)
                        return roomEndTime.AddDays(1);
                return roomEndTime;
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
            get { return !_eventBookedProducts.Any(x => x.Quantity < 0 || x.Product == null); }
            // get { return true; }
        }

        [DataMember]
        public DateTime StartTime
        {
            get { return _eventRoom.StartTime; }
            set
            {
                if (_eventRoom.StartTime == value) return;
                _eventRoom.StartTime = value;
                RaisePropertyChanged(() => StartTime);
                RaisePropertyChanged(() => EndTime);
            }
        }

        [DataMember]
        public DateTime EndTime
        {
            get { return _eventRoom.EndTime; }
            set
            {
                if (_eventRoom.EndTime == value) return;
                _eventRoom.EndTime = value;
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
                _eventRoom.RoomID = _room.ID;
                RaisePropertyChanged(() => Room);
                RaisePropertyChanged(() => StartTime);
                RaisePropertyChanged(() => EndTime);

            }
        }

        #endregion

        #region Constructors

        public EventRoomModel(EventRoom eventRoom)
        {
            _eventRoom = eventRoom;
            EventBookedProducts = new ObservableCollection<EventBookedProductModel>();
            _room = eventRoom.Room;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            RaisePropertyChanged(() => Room);
            RaisePropertyChanged(() => StartTime);
            RaisePropertyChanged(() => EndTime);
        }

        #endregion

        #region IDataErrorInfo

        public bool HasErrors
        {
            get { return typeof(EventRoomModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                long startTimeTicks;
                long endTimeTicks;
                long roomStartTimeTicks;
                long roomEndTimeTicks;
                Error = string.Empty;

                if (columnName == "Room")
                    if (Room == null)
                        Error = "Room can't be empty.";
                if (Room != null)
                {
                    SetTimeValuesByRoom(out startTimeTicks, out endTimeTicks, out roomStartTimeTicks, out roomEndTimeTicks);
                    if (columnName == "StartTime")
                    {
                        //if (startTimeTicks == 0)
                        //    Error = "Please select Start Time.";
                        //else 
                        if (startTimeTicks < roomStartTimeTicks || startTimeTicks > roomEndTimeTicks)
                            Error = "Please select Valid Start Time.";
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
        private void SetTimeValuesByRoom(out long startTimeTicks, out long endTimeTicks, out long roomStartTimeTicks, out long roomEndTimeTicks)
        {
            startTimeTicks = (new TimeSpan(StartTime.Hour, StartTime.Minute, StartTime.Second)).Ticks;
            endTimeTicks = (new TimeSpan(EndTime.Hour, EndTime.Minute, EndTime.Second)).Ticks;
            roomStartTimeTicks = Room.StartTime.Ticks;
            roomEndTimeTicks = Room.EndTime.Ticks;
            if (Room.EndTime < Room.StartTime)
            {
                roomEndTimeTicks += new TimeSpan(1, 0, 0, 0).Ticks;
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
