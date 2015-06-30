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
    public class EventCateringsRepository : EntitiesRepository<EventCatering>, IEventCateringsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventCateringsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventCatering>> GetAllAsync()
        {
            return await _objectContext.EventCaterings
                .Include("Room")
                .Include("Event")
                .ToListAsync();
        }

        public async Task<List<EventCatering>> GetAllAsync(Expression<Func<EventCatering, bool>> expression)
        {
            return await _objectContext.EventCaterings
                .Include("Room")
                .Include("Event")
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(EventCatering entity)
        {
            _objectContext.EventCaterings.AddObject(entity);
        }

        public override void Delete(EventCatering entity)
        {
            _objectContext.EventCaterings.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventCatering> entity)
        {
            entity.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventCaterings);
        }
        public void Refresh(RefreshMode mode)
        {
            _objectContext.DetectChanges();
            _objectContext.Refresh(mode, _objectContext.EventCaterings);
        }

        public List<EventCatering> SetCateringsCurrentValues(List<EventCatering> eventCaterings)
        {
            _objectContext.DetectChanges();
            IEnumerable<EventCatering> DeletedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).Where(x => x.EntitySet.Name == _objectContext.EventCaterings.EntitySet.Name).Select(x => (EventCatering)x.Entity).ToList();
            IEnumerable<EventCatering> ModifiedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).Where(x => x.EntitySet.Name == _objectContext.EventCaterings.EntitySet.Name).Select(x => (EventCatering)x.Entity).ToList();
            IEnumerable<EventCatering> AddedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(x => x.EntitySet.Name == _objectContext.EventCaterings.EntitySet.Name).Select(x => (EventCatering)x.Entity).ToList();

            DeletedCollection.ForEach(deletedCatering =>
            {
                eventCaterings.Remove(deletedCatering);
            });
            eventCaterings.AddRange(AddedCollection);
            ModifiedCollection.ForEach(modifiedEntity =>
            {
                eventCaterings.RemoveAll(x => x.ID == modifiedEntity.ID);
            });
            eventCaterings.AddRange(ModifiedCollection);
            return eventCaterings;
        }
    }
}
