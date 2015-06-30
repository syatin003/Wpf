using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace EventManagementSystem.Data.Repositories
{
    public class ActivitiesRepository : EntitiesRepository<Activity>, IActivitiesRepository
    {
        private readonly EmsEntities _objectContext;

        public ActivitiesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Activity>> GetAllAsync()
        {
            return await _objectContext.Activities
                .Include("Enquiry")
                .Include("Enquiry.User")
                .Include("ActivityType")
                .Include("Enquiry.EventType")
                .ToListAsync();
        }

        public async Task<Activity> GetUpdatedActivity(Guid activityId)
        {
            var desiredActivity = await _objectContext.Activities.Include("FollowUp").FirstOrDefaultAsync(x => x.ID == activityId);
            if (desiredActivity != null)
                _objectContext.Refresh(RefreshMode.StoreWins, desiredActivity);

            return desiredActivity;
        }
        public async Task<List<Activity>> GetAllAsync(Expression<Func<Activity, bool>> expression)
        {
            return await _objectContext.Activities
                .Where(expression)
                .Include("Enquiry")
                .Include("Enquiry.User")
                .Include("ActivityType")
                .Include("Enquiry.EventType")
                .ToListAsync();
        }
        public override void Add(Activity entity)
        {
            _objectContext.Activities.AddObject(entity);
        }

        public override void Delete(Activity entity)
        {
            _objectContext.Activities.DeleteObject(entity);
        }
        public void SetEntityModified(Activity entity)
        {
            _objectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Activities);
        }
      
    }
}