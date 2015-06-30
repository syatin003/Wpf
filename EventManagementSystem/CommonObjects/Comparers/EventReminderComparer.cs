using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventReminderComparer : IEqualityComparer<EventReminderModel>
    {
        public bool Equals(EventReminderModel x, EventReminderModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventReminder.ID == y.EventReminder.ID;
        }

        public int GetHashCode(EventReminderModel obj)
        {
            if (obj == null) return 0;

            return obj.EventReminder.ID.GetHashCode();
        }
    }
}
