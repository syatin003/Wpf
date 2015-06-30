using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class EventStatusesRepository : EntitiesRepository<EventStatus>, IEventStatusesRepository
    {
        private readonly EmsEntities _objectContext;

        public EventStatusesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventStatus>> GetAllAsync()
        {
            return await _objectContext.EventStatuses.ToListAsync();
        }

        public override void Add(EventStatus entity)
        {
            _objectContext.EventStatuses.AddObject(entity);
        }

        public override void Delete(EventStatus entity)
        {
            _objectContext.EventStatuses.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventStatuses);
        }
        public void Refresh(EventStatus entity)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, entity);
        }
    }
}
