using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventCateringComparer : IEqualityComparer<EventCateringModel>
    {
        public int GetHashCode(EventCateringModel obj)
        {
            if (obj == null) return 0;

            return obj.EventCatering.ID.GetHashCode();
        }

        public bool Equals(EventCateringModel x, EventCateringModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventCatering.ID == y.EventCatering.ID;
        }
    }
}
