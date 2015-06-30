using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class MemberNoteComparer : IEqualityComparer<MemberNoteModel>
    {
        public int GetHashCode(MemberNoteModel obj)
        {
            if (obj == null) return 0;

            return obj.MemberNote.ID.GetHashCode();
        }

        public bool Equals(MemberNoteModel x, MemberNoteModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.MemberNote.ID == y.MemberNote.ID;
        }
    }
}
