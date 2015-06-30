using System.Collections.Generic;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Comparers
{
    public class GolfFollowResourcesComparer : IEqualityComparer<GolfModel>
    {
        public int GetHashCode(GolfModel obj)
        {
            if (obj == null) return 0;

            return obj.Golf.ID.GetHashCode();
        }

        public bool Equals(GolfModel x, GolfModel y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Golf.ID == y.Golf.ID;
        }
    }
}
