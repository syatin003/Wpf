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
    public class FollowUpStatusesRepository : EntitiesRepository<FollowUpStatus>, IFollowUpStatusesRepository
    {
        private readonly EmsEntities _objectContext;

        public FollowUpStatusesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<FollowUpStatus>> GetAllAsync()
        {
            return await _objectContext.FollowUpStatuses.ToListAsync();
        }


        public async Task<List<FollowUpStatus>> GetAllAsync(Expression<Func<FollowUpStatus, bool>> expression)
        {
            return await _objectContext.FollowUpStatuses
                .Where(expression)
                .ToListAsync();
        }

        public async Task<FollowUpStatus> GetUpdatedFollowUpStatus(Guid followUpStatusId)
        {
            var desiredFollowUpStatus = await _objectContext.FollowUpStatuses.FirstOrDefaultAsync(x => x.ID == followUpStatusId);
            _objectContext.Refresh(RefreshMode.StoreWins, desiredFollowUpStatus);

            return desiredFollowUpStatus;
        }

        public override void Add(FollowUpStatus entity)
        {
            _objectContext.FollowUpStatuses.AddObject(entity);
        }

        public override void Delete(FollowUpStatus entity)
        {
            _objectContext.FollowUpStatuses.DeleteObject(entity);
        }

        public void Refresh(FollowUpStatus entity)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, entity);
        }
    }
}