using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventBookedProductComparer : IEqualityComparer<EventBookedProductModel>
    {
        public int GetHashCode(EventBookedProductModel obj)
        {
            if (obj == null) return 0;

            return obj.EventBookedProduct.ID.GetHashCode();
        }

        public bool Equals(EventBookedProductModel x, EventBookedProductModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventBookedProduct.ID == y.EventBookedProduct.ID;
        }
    }
}
