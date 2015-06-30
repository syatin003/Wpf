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
    public class EventGolfsRepository : EntitiesRepository<EventGolf>, IEventGolfsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventGolfsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventGolf>> GetAllAsync()
        {
            return await _objectContext.EventGolfs
                .Include("Golf")
                .Include("GolfHole")
                .Include("Event")
                .ToListAsync();
        }

        public async Task<List<EventGolf>> GetAllAsyncGolfResources(Expression<Func<EventGolf, bool>> expression)
        {
            return await _objectContext.EventGolfs
                .Where(expression)
                .Include("Golf")
                .Include("GolfHole")
                .Include("Event")
                .ToListAsync();
        }

        public async Task<List<EventGolf>> GetAllAsync(Expression<Func<EventGolf, bool>> expression)
        {
            return await _objectContext.EventGolfs
                .Where(expression)
                .Include("Golf")
                .Include("GolfHole")
                .Include("Event")
                .Include("EventGolf1")
                .ToListAsync();
        }

        public override void Add(EventGolf entity)
        {
            _objectContext.EventGolfs.AddObject(entity);
        }

        public override void Delete(EventGolf entity)
        {
            _objectContext.EventGolfs.DeleteObject(entity);
        }
        public void Delete(IEnumerable<EventGolf> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventGolfs);
        }
        public void Refresh(RefreshMode mode)
        {
            _objectContext.DetectChanges();
            _objectContext.Refresh(mode, _objectContext.EventGolfs);
        }

        public List<EventGolf> SetGolfCurrentValues(List<EventGolf> eventGolfs)
        {

            _objectContext.DetectChanges();
            IEnumerable<EventGolf> DeletedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).Where(x => x.EntitySet.Name == _objectContext.EventGolfs.EntitySet.Name).Select(x => (EventGolf)x.Entity).ToList();
            IEnumerable<EventGolf> ModifiedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).Where(x => x.EntitySet.Name == _objectContext.EventGolfs.EntitySet.Name).Select(x => (EventGolf)x.Entity).ToList();
            IEnumerable<EventGolf> AddedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(x => x.EntitySet.Name == _objectContext.EventGolfs.EntitySet.Name).Select(x => (EventGolf)x.Entity).ToList();

            DeletedCollection.ForEach(deletedGolf =>
            {
                eventGolfs.Remove(deletedGolf);
            });
            eventGolfs.AddRange(AddedCollection);
            ModifiedCollection.ForEach(modifiedEntity =>
            {
                eventGolfs.RemoveAll(x => x.ID == modifiedEntity.ID);
            });
            eventGolfs.AddRange(ModifiedCollection);

            return eventGolfs;
        }
        public void DetachGolfEvent(Guid golfEventId)
        {
            var AddedGolfEvent = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(x => x.EntitySet.Name == _objectContext.EventGolfs.EntitySet.Name && ((EventGolf)x.Entity).ID == golfEventId).Select(x => (EventGolf)x.Entity).FirstOrDefault();
            if (AddedGolfEvent != null)
            {
                _objectContext.Detach(AddedGolfEvent);
            }
        }



        public void ApplyRelationalChanges()
        {
            _objectContext.DetectChanges();
        }
    }
}
