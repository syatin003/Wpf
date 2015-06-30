using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IActivitiesRepository
    {
        Task<List<Activity>> GetAllAsync();
        Task<List<Activity>> GetAllAsync(Expression<Func<Activity, bool>> expression);
        Task<Activity> GetUpdatedActivity(Guid activityId);

        void Add(Activity entity);
        void Delete(Activity entity);
        void Refresh();
        void SetEntityModified(Activity entity);
    }
}
