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
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class EventNotesRepository : EntitiesRepository<EventNote>, IEventNotesRepository
    {
        private readonly EmsEntities _objectContext;

        public EventNotesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventNote>> GetAllAsync()
        {
            return await _objectContext.EventNotes
                .Include("EventNoteType")
                .Include("User")
                .ToListAsync();
        }

        public async Task<List<EventNote>> GetAllAsync(Expression<Func<EventNote, bool>> expression)
        {
            return await _objectContext.EventNotes
                .Where(expression)
                .Include("EventNoteType")
                .Include("User")
                .ToListAsync();
        }

        public override void Add(EventNote entity)
        {
            _objectContext.EventNotes.AddObject(entity);
        }

        public override void Delete(EventNote entity)
        {
            _objectContext.EventNotes.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventNote> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventNotes);
        }
    }
}
