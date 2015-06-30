using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipUpdatesRepository
    {
        Task<List<MembershipUpdate>> GetAllAsync();
        Task<List<MembershipUpdate>> GetAllAsync(Expression<Func<MembershipUpdate, bool>> expression);

        Task<List<MembershipUpdate>> GetUpdatesByDate(DateTime startDate, DateTime endDate);

        void Add(MembershipUpdate entity);
        void Delete(MembershipUpdate entity);
        void Delete(IEnumerable<MembershipUpdate> entities);
        void Refresh();
    }
}
