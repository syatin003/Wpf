using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EnquiryNoteComparer : IEqualityComparer<EnquiryNoteModel>
    {
        public bool Equals(EnquiryNoteModel x, EnquiryNoteModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.EnquiryNote.ID == y.EnquiryNote.ID;
        }

        public int GetHashCode(EnquiryNoteModel obj)
        {
            if (obj == null) return 0;

            return obj.EnquiryNote.ID.GetHashCode();
        }
    }
}
