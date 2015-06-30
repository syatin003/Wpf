using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipCategoryGroupDefaultsRepository
    {
        Task<List<MembershipCategoryGroupDefault>> GetAllAsync();
        Task<List<MembershipCategoryGroupDefault>> GetAllAsync(Expression<Func<MembershipCategoryGroupDefault, bool>> expression);

        void Add(MembershipCategoryGroupDefault entity);
        void Delete(MembershipCategoryGroupDefault entity);

        void Delete(IEnumerable<MembershipCategoryGroupDefault> entities);

        void Refresh();
    }
}
