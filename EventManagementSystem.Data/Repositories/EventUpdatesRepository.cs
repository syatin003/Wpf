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
    public class EventUpdatesRepository : EntitiesRepository<EventUpdate>, IEventUpdatesRepository
    {
        private readonly EmsEntities _objectContext;

        public EventUpdatesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventUpdate>> GetAllAsync()
        {
            return await _objectContext.EventUpdates
                .Include("User")
                .Include("Event")
                .ToListAsync();
        }

        public async Task<List<EventUpdate>> GetAllAsync(Expression<Func<EventUpdate, bool>> expression)
        {
            return await _objectContext.EventUpdates
                .Where(expression)
                .Include("User")
                .Include("Event")
                .ToListAsync();
        }
        public async Task<List<EventUpdate>> GetEventUpdatesByDate(DateTime startDate, DateTime endDate)
        {
            var updatesWithoutNotes = _objectContext.EventUpdates.Where(update => update.Date >= startDate && update.Date <= endDate && update.Field != "Notes");

            var notesUpdates = _objectContext.EventUpdates.Where(update => update.Date >= startDate && update.Date <= endDate && update.Field == "Notes");

            var notesUpdatesHistory = _objectContext.EventUpdates.Where(x => notesUpdates.Select(y => y.ItemId).Contains(x.ItemId));

            return await updatesWithoutNotes.Union(notesUpdatesHistory)
                        .Include("User")
                        .Include("Event")
                        .ToListAsync();
        }
        public override void Add(EventUpdate entity)
        {
            _objectContext.EventUpdates.AddObject(entity);
        }

        public override void Delete(EventUpdate entity)
        {
            _objectContext.EventUpdates.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventUpdate> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventUpdates);
        }
    }
}
