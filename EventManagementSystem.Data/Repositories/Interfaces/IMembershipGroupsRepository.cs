using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipGroupsRepository
    {
        Task<List<MembershipGroup>> GetAllAsync();
        Task<List<MembershipGroup>> GetAllAsync(Expression<Func<MembershipGroup, bool>> expression);

        void Add(MembershipGroup entity);
        void Delete(MembershipGroup entity);

        void Refresh();
    }
}
