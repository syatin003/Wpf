using System;
using System.Collections.Generic;
using System.Linq;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;
using System.Collections.ObjectModel;

namespace EventManagementSystem.Services
{
    public class BookingsService
    {
        #region Properties

        public List<EventRoomModel> BookedRooms;
        public List<EventGolfModel> BookedGolfs;
        public List<EventCateringModel> BookedCaterings;

        #endregion

        #region Methods

        public bool IsRoomAvailable(Room room, DateTime date)
        {
            var isEventRoomBooked = BookedRooms.Where(x => !x.EventRoom.Event.IsDeleted && x.Room == room).Any(x => IsDateBetweenAnotherTwo(x.StartTimeEx, x.EndTimeEx, date));
            var isEventCateringBooked = BookedCaterings.Where(x => !x.EventCatering.Event.IsDeleted && x.Room == room).Any(x => IsDateBetweenAnotherTwo(x.StartTimeEx, x.EndTimeEx, date));

            var isRoomBooked = isEventRoomBooked || isEventCateringBooked;

            return !isRoomBooked;
        }

        public bool IsRoomAvailable(Guid eventId, Room room, DateTime start, DateTime end)
        {
            if (room.EndTime < room.StartTime)
            {
                if (room.EndTime < room.StartTime)
                {
                    if ((new TimeSpan(start.Hour, start.Minute, 0)).Ticks < (new TimeSpan(room.StartTime.Hours, room.StartTime.Minutes, room.StartTime.Seconds)).Ticks)
                        start = start.AddDays(1);
                    if ((new TimeSpan(end.Hour, end.Minute, 0)).Ticks < (new TimeSpan(room.StartTime.Hours, room.StartTime.Minutes, room.StartTime.Seconds)).Ticks)
                        end = end.AddDays(1);
                }
            }

            var isEventRoomBooked = BookedRooms.Where(x => !x.EventRoom.Event.IsDeleted && x.EventRoom.EventID != eventId && x.EventRoom.RoomID == room.ID).Any(x => IsDateBetweenAnotherTwo(start, end, x.StartTimeEx)
                || IsDateBetweenAnotherTwo(x.StartTimeEx, x.EndTimeEx, start) || IsDateBetweenAnotherTwo(x.StartTimeEx, x.EndTimeEx, end.Add(new TimeSpan(0, 0, -1, 0)))
                            || IsDateBetweenAnotherTwo(start, end, x.EndTimeEx.Add(new TimeSpan(0, 0, -1, 0))));

            var isEventCateringBooked = BookedCaterings.Where(x => !x.EventCatering.Event.IsDeleted && x.EventCatering.EventID != eventId && x.EventCatering.RoomID == room.ID).Any(x => IsDateBetweenAnotherTwo(start, end, x.StartTimeEx)
                || IsDateBetweenAnotherTwo(x.StartTimeEx, x.EndTimeEx, start) || IsDateBetweenAnotherTwo(x.StartTimeEx, x.EndTimeEx, end.Add(new TimeSpan(0, 0, -1, 0)))
                || IsDateBetweenAnotherTwo(start, end, x.EndTimeEx.Add(new TimeSpan(0, 0, -1, 0))));

            var isRoomBooked = isEventRoomBooked || isEventCateringBooked;

            return !isRoomBooked;
        }

        public bool IsGolfAvailable(Golf golf, DateTime time)
        {
            var isGolfBooked = BookedGolfs.Where(x => !x.EventGolf.Event.IsDeleted && x.Golf == golf).Any(x => IsDateBetweenAnotherTwo(x.TimeEx, x.EndTimeEx, time));

            return !isGolfBooked;
        }

        public bool IsGolfAvailable(Golf golf, DateTime startTime, DateTime endTime)
        {
            var isMainGolfBooked = BookedGolfs.Where(x => !x.EventGolf.Event.IsDeleted && x.Golf == golf).Any(x => IsDateBetweenAnotherTwo(startTime, endTime, x.TimeEx)
                || IsDateBetweenAnotherTwo(x.TimeEx, x.EndTimeEx, startTime) || IsDateBetweenAnotherTwo(x.TimeEx, x.EndTimeEx, endTime.Add(new TimeSpan(0, 0, -1, 0)))
                || IsDateBetweenAnotherTwo(startTime, endTime, x.EndTimeEx.Add(new TimeSpan(0, 0, -1, 0))));

            return !isMainGolfBooked;
        }

        public List<Event> GetModelsByRoom(string name, DateTime time)
        {
            var events = new List<Event>();

            var roomBookings = BookedRooms.Where(x => x.EventRoom.Room.Name == name && IsDateBetweenAnotherTwo(x.StartTimeEx, x.EndTimeEx, time)).ToList();

            if (roomBookings.Any())
                events.AddRange(roomBookings.Select(x => x.EventRoom.Event));

            var cateringBookings = BookedCaterings.Where(x => x.EventCatering.Room.Name == name && IsDateBetweenAnotherTwo(x.StartTimeEx, x.EndTimeEx, time)).ToList();

            if (cateringBookings.Any())
                events.AddRange(cateringBookings.Select(x => x.EventCatering.Event));

            return events;
        }

        public Event GetModelByGolf(string name, DateTime time)
        {
            var events = new List<Event>();
            var golfBookings = BookedGolfs.Where(x => x.EventGolf.Golf.Name == name && IsDateBetweenAnotherTwo(x.TimeEx, x.EndTimeEx, time)).ToList();

            if (golfBookings.Any())
                events.AddRange(golfBookings.Select(x => x.EventGolf.Event));
            return events.FirstOrDefault(); //TODO  If current date has several events ?
        }

        private bool IsDateBetweenAnotherTwo(DateTime low, DateTime high, DateTime instance)
        {
            return (low.Ticks <= instance.Ticks && high.Ticks > instance.Ticks);
        }

        //private bool IsDateBetweenAnotherTwo(DateTime low, DateTime high, DateTime instance, EventGolfModel x, int a)
        //{
        //    //return (low.Ticks <= instance.Ticks && high.Ticks > instance.Ticks);
        //    var test = (low.Ticks <= instance.Ticks && high.Ticks > instance.Ticks);
        //    if (test == true)
        //    {
        //        var abc = "testin";
        //    }
        //    return test;
        //}
        #endregion


    }
}
