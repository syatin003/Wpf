using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class EventNoteTypesRepository : EntitiesRepository<EventNoteType>, IEventNoteTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public EventNoteTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventNoteType>> GetAllAsync()
        {
            return await _objectContext.EventNoteTypes.ToListAsync();
        }

        public override void Add(EventNoteType entity)
        {
            _objectContext.EventNoteTypes.AddObject(entity);
        }

        public override void Delete(EventNoteType entity)
        {
            _objectContext.EventNoteTypes.DeleteObject(entity);
        }
    }
}