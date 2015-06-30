using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EnquiryFollowUpComparer : IEqualityComparer<FollowUpModel>
    {
        public bool Equals(FollowUpModel x, FollowUpModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.FollowUp.ID == y.FollowUp.ID;
        }

        public int GetHashCode(FollowUpModel obj)
        {
            if (obj == null) return 0;

            return obj.FollowUp.ID.GetHashCode();
        }
    }
}
