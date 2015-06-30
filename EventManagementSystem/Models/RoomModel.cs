using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using System;
using System.Collections.ObjectModel;

namespace EventManagementSystem.Models
{
    public class RoomModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly Room _room;
        private string _roomFacilities;
        private ObservableCollection<TimeSpan> _clockItems;

        #endregion

        #region Properties

        public Room Room
        {
            get { return _room; }
        }

        public string Name
        {
            get { return _room.Name; }
            set
            {
                if (_room.Name == value) return;
                _room.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string Color
        {
            get { return _room.Color; }
            set
            {
                if (_room.Color == value) return;
                _room.Color = value;
                RaisePropertyChanged(() => Color);
            }
        }

        public float Height
        {
            get { return _room.Height; }
            set
            {
                if (_room.Height == value) return;
                _room.Height = value;

                RaisePropertyChanged(() => Height);

                RaisePropertyChanged(() => TotalCapacity);
                RaisePropertyChanged(() => TotalCapacityFt);
            }
        }

        public float Width
        {
            get { return _room.Width; }
            set
            {
                if (_room.Width == value) return;
                _room.Width = value;

                RaisePropertyChanged(() => Width);
                RaisePropertyChanged(() => FloorArea);
                RaisePropertyChanged(() => FloorAreaFt);
                RaisePropertyChanged(() => TotalCapacity);
                RaisePropertyChanged(() => TotalCapacityFt);
            }
        }

        public float Length
        {
            get { return _room.Length; }
            set
            {
                if (_room.Length == value) return;
                _room.Length = value;

                RaisePropertyChanged(() => Length);
                RaisePropertyChanged(() => FloorArea);
                RaisePropertyChanged(() => FloorAreaFt);
                RaisePropertyChanged(() => TotalCapacity);
                RaisePropertyChanged(() => TotalCapacityFt);
            }
        }

        public double FloorArea
        {
            get { return Width * Length; }
        }

        public double TotalCapacity
        {
            get { return FloorArea * Height; }
        }

        public double FloorAreaFt
        {
            get { return Width * Length * 3.2808 * 3.2808; }
        }

        public double TotalCapacityFt
        {
            get { return FloorAreaFt * Height * 3.2808; }
        }

        public string RoomFacilities
        {
            get { return _roomFacilities; }
            set
            {
                if (_roomFacilities == value) return;
                _roomFacilities = value;
                RaisePropertyChanged(() => RoomFacilities);
            }
        }


        public ObservableCollection<TimeSpan> ClockItems
        {
            get
            {
                return _clockItems;
            }
            set
            {
                if (_clockItems == value) return;
                _clockItems = value;
                RaisePropertyChanged(() => ClockItems);
            }
        }

        #endregion

        #region Constructors

        public RoomModel(Room room)
        {
            _room = room;
            RefreshRoomFacilities();
            CreateClockItems();

        }

        public void RefreshRoomFacilities()
        {
            if (_room.RoomFacilities.Any())
            {
                RoomFacilities = string.Join(", ", Room.RoomFacilities.Select(x => x.Facility.Name));
            }
        }


        #endregion

        #region Methods

        private void CreateClockItems()
        {
            ClockItems = new ObservableCollection<TimeSpan>();
            var startTime = Room.StartTime;
            var endTime = Room.EndTime;
            var timeInterval = Room.TimeInterval;
            var clockItem = new TimeSpan(startTime.Hours, startTime.Minutes, startTime.Seconds);
            ClockItems.Add(clockItem);

            if (startTime < endTime)
            {
                Int32 slots = Convert.ToInt32((endTime.Ticks - startTime.Ticks) / timeInterval.Ticks);
                for (int i = 0; i < slots; i++)
                {
                    clockItem = clockItem.Add(Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
            }
            else
            {
                var endOfTheDay = new TimeSpan(24, 0, 0);
                Int32 slotsBeforeEndOfDay = (timeInterval.Ticks != 0) ? Convert.ToInt32((endOfTheDay.Ticks - startTime.Ticks) / timeInterval.Ticks) : 0;
                for (int i = 0; i < slotsBeforeEndOfDay; i++)
                {
                    clockItem = clockItem.Add(Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
                Int32 slotsAfterEndOfDay = (timeInterval.Ticks != 0) ? Convert.ToInt32(endTime.Ticks / timeInterval.Ticks) : 0;
                clockItem = new TimeSpan();
                for (int i = 0; i < slotsAfterEndOfDay; i++)
                {
                    clockItem = clockItem.Add(Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
            }
        }
        #endregion Methods


        #region IDataError Implementation

        public bool HasErrors
        {
            get { return typeof(RoomModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Name")
                    if (string.IsNullOrWhiteSpace(Name))
                        Error = "Name can't be empty";

                if (columnName == "Color")
                    if (string.IsNullOrWhiteSpace(Color))
                        Error = "Color can't be empty";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion

    }
}