using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipLinkTypesRepository
    {
        Task<List<MembershipLinkType>> GetAllAsync();
        Task<List<MembershipLinkType>> GetAllAsync(Expression<Func<MembershipLinkType, bool>> expression);

        void Add(MembershipLinkType entity);
        void Delete(MembershipLinkType entity);
        void Delete(IEnumerable<MembershipLinkType> entities);

        void Refresh();
    }
}
