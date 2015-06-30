using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class EnquiryActivityComparer : IEqualityComparer<ActivityModel>
    {
        public bool Equals(ActivityModel x, ActivityModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Activity.ID == y.Activity.ID;
        }

        public int GetHashCode(ActivityModel obj)
        {
            if (obj == null) return 0;

            return obj.Activity.ID.GetHashCode();
        }
    }
}
