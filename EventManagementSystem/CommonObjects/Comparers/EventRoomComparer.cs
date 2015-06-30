using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventRoomComparer : IEqualityComparer<EventRoomModel>
    {
        public int GetHashCode(EventRoomModel obj)
        {
            if (obj == null) return 0;

            return obj.EventRoom.ID.GetHashCode();
        }

        public bool Equals(EventRoomModel x, EventRoomModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventRoom.ID == y.EventRoom.ID;
        }
    }
}
