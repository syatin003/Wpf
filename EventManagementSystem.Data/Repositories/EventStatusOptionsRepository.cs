using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class EventStatusOptionsRepository : EntitiesRepository<EventStatusOption>, IEventStatusOptionsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventStatusOptionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventStatusOption>> GetAllAsync()
        {
            return await _objectContext.EventStatusOptions.ToListAsync();
        }

        public override void Add(EventStatusOption entity)
        {
            _objectContext.EventStatusOptions.AddObject(entity);
        }

        public override void Delete(EventStatusOption entity)
        {
            _objectContext.EventStatusOptions.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventStatusOption> entities)
        {
            entities.ForEach(Delete);
        }
    }
}
