using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventGolfComparer : IEqualityComparer<EventGolfModel>
    {
        public int GetHashCode(EventGolfModel obj)
        {
            if (obj == null) return 0;

            return obj.EventGolf.ID.GetHashCode();
        }

        public bool Equals(EventGolfModel x, EventGolfModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventGolf.ID == y.EventGolf.ID;
        }
    }
}
