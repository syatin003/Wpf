using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipGroupStylesRepository
    {
        Task<List<MembershipGroupStyle>> GetAllAsync();
        Task<List<MembershipGroupStyle>> GetAllAsync(Expression<Func<MembershipGroupStyle, bool>> expression);

        void Add(MembershipGroupStyle entity);
        void Delete(MembershipGroupStyle entity);

        void Refresh();
    }
}
