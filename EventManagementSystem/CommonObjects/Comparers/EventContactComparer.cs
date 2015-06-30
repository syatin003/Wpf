using System.Collections.Generic;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventContactComparer : IEqualityComparer<EventContact>
    {
        public int GetHashCode(EventContact obj)
        {
            if (obj == null) return 0;

            return obj.ID.GetHashCode();
        }

        public bool Equals(EventContact x, EventContact y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.ID == y.ID;
        }
    }
}
