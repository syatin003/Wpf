using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class EventTypeOptionsRepository : EntitiesRepository<EventTypeOption>, IEventTypeOptionsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventTypeOptionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventTypeOption>> GetAllAsync()
        {
            return await _objectContext.EventTypeOptions.ToListAsync();
        }

        public override void Add(EventTypeOption entity)
        {
            _objectContext.EventTypeOptions.AddObject(entity);
        }

        public override void Delete(EventTypeOption entity)
        {
            _objectContext.EventTypeOptions.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventTypeOption> entities)
        {
            entities.ForEach(Delete);
        }
    }
}
