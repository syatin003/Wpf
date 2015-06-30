using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class EventOptionsRepository : EntitiesRepository<EventOption>, IEventOptionsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventOptionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventOption>> GetAllAsync()
        {
            return await _objectContext.EventOptions.ToListAsync();
        }

        public override void Add(EventOption entity)
        {
            _objectContext.EventOptions.AddObject(entity);
        }

        public override void Delete(EventOption entity)
        {
            _objectContext.EventOptions.DeleteObject(entity);
        }
    }
}
