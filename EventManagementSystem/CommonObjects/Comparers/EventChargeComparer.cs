using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventChargeComparer : IEqualityComparer<EventChargeModel>
    {
        public int GetHashCode(EventChargeModel obj)
        {
            if (obj == null) return 0;

            return obj.EventCharge.ID.GetHashCode();
        }

        public bool Equals(EventChargeModel x, EventChargeModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventCharge.ID == y.EventCharge.ID;
        }
    }
}
