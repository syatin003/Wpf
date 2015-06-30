using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventInvoiceComparer : IEqualityComparer<EventInvoiceModel>
    {
        public int GetHashCode(EventInvoiceModel obj)
        {
            if (obj == null) return 0;

            return obj.EventInvoice.ID.GetHashCode();
        }

        public bool Equals(EventInvoiceModel x, EventInvoiceModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventInvoice.ID == y.EventInvoice.ID;
        }
    }
}
