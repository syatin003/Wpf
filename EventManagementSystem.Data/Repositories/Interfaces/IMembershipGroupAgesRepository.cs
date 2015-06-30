using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipGroupAgesRepository
    {
        Task<List<MembershipGroupAge>> GetAllAsync();
        Task<List<MembershipGroupAge>> GetAllAsync(Expression<Func<MembershipGroupAge, bool>> expression);

        void Add(MembershipGroupAge entity);
        void Delete(MembershipGroupAge entity);

        void Refresh();
    }
}
