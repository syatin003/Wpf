using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventPaymentComparer : IEqualityComparer<EventPaymentModel>
    {
        public int GetHashCode(EventPaymentModel obj)
        {
            if (obj == null) return 0;

            return obj.EventPayment.ID.GetHashCode();
        }

        public bool Equals(EventPaymentModel x, EventPaymentModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventPayment.ID == y.EventPayment.ID;
        }
    }
}
