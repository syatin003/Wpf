using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class FollowUpsRepository : EntitiesRepository<FollowUp>, IFollowUpsRepository
    {
        private readonly EmsEntities _objectContext;

        public FollowUpsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<FollowUp>> GetAllAsync()
        {
            return await _objectContext.FollowUps
                .Include("Enquiry")
                .Include("Enquiry.EventType")
                .Include("User")
                .ToListAsync();
        }

        public async Task<List<FollowUp>> GetAllAsync(Expression<Func<FollowUp, bool>> expression)
        {
            return await _objectContext.FollowUps
                .Where(expression)
                .Include("Enquiry")
                .Include("Enquiry.EventType")
                .Include("User")
                .ToListAsync();
        }

        public async Task<FollowUp> GetUpdatedFollowUp(Guid followUpId)
        {
            var desiredFollowUp = await _objectContext.FollowUps.FirstOrDefaultAsync(x => x.ID == followUpId);
            if (desiredFollowUp != null)
                _objectContext.Refresh(RefreshMode.StoreWins, desiredFollowUp);

            return desiredFollowUp;
        }

        public override void Add(FollowUp entity)
        {
            _objectContext.FollowUps.AddObject(entity);
        }

        public override void Delete(FollowUp entity)
        {
            _objectContext.FollowUps.DeleteObject(entity);
        }

        public void SetEntityModified(FollowUp entity)
        {
            _objectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.FollowUps);
        }
    }
}