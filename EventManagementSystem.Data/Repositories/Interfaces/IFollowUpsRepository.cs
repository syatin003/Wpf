using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IFollowUpsRepository
    {
        Task<List<FollowUp>> GetAllAsync();
        Task<List<FollowUp>> GetAllAsync(Expression<Func<FollowUp, bool>> expression);

        Task<FollowUp> GetUpdatedFollowUp(Guid followUpId);

        void Add(FollowUp entity);
        void Delete(FollowUp entity);
        void SetEntityModified(FollowUp entity);
        void Refresh();
    }
}