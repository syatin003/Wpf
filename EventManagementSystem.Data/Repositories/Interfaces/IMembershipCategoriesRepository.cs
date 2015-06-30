using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipCategoriesRepository
    {
        Task<List<MembershipCategory>> GetAllAsync();
        Task<List<MembershipCategory>> GetAllAsync(Expression<Func<MembershipCategory, bool>> expression);
        Task<List<MembershipCategory>> GetAllCategoriesWithItsTabsDataAsync();

        void Add(MembershipCategory entity);
        void Delete(MembershipCategory entity);

        void Refresh();
        void RefreshCategoryGroupsWithItsTabs();
    }
}
