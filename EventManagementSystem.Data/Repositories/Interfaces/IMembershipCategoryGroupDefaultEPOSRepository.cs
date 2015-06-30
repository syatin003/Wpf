using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipCategoryGroupDefaultEPOSRepository
    {
        Task<List<MembershipCategoryGroupDefaultEPOS>> GetAllAsync();
        Task<List<MembershipCategoryGroupDefaultEPOS>> GetAllAsync(Expression<Func<MembershipCategoryGroupDefaultEPOS, bool>> expression);

        void Add(MembershipCategoryGroupDefaultEPOS entity);
        void Delete(MembershipCategoryGroupDefaultEPOS entity);

        void Refresh();
    }
}
