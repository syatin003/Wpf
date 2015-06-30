using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IFollowUpStatusesRepository
    {
        Task<List<FollowUpStatus>> GetAllAsync();
        Task<List<FollowUpStatus>> GetAllAsync(Expression<Func<FollowUpStatus, bool>> expression);
        Task<FollowUpStatus> GetUpdatedFollowUpStatus(Guid followUpStatusId);

        void Add(FollowUpStatus entity);
        void Delete(FollowUpStatus entity);
        void Refresh(FollowUpStatus entity);
    }
}
