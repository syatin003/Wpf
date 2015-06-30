using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EventNoteComparer : IEqualityComparer<EventNoteModel>
    {
        public int GetHashCode(EventNoteModel obj)
        {
            if (obj == null) return 0;

            return obj.EventNote.ID.GetHashCode();
        }

        public bool Equals(EventNoteModel x, EventNoteModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EventNote.ID == y.EventNote.ID;
        }
    }
}
