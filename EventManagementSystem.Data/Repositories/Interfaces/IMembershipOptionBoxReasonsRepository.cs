using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipOptionBoxReasonsRepository
    {
        Task<List<MembershipOptionBoxReason>> GetAllAsync();
        Task<List<MembershipOptionBoxReason>> GetAllAsync(Expression<Func<MembershipOptionBoxReason, bool>> expression);

        void Add(MembershipOptionBoxReason entity);
        void Delete(MembershipOptionBoxReason entity);
        void Delete(IEnumerable<MembershipOptionBoxReason> entities);
        void Refresh();
    }
}
