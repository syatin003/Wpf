using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipTokensRepository
    {
        Task<List<MembershipToken>> GetAllAsync();
        Task<List<MembershipToken>> GetAllAsync(Expression<Func<MembershipToken, bool>> expression);

        void Add(MembershipToken entity);
        void Delete(MembershipToken entity);

        void Refresh();
    }
}
