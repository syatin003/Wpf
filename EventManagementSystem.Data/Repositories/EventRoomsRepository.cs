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
    public class EventRoomsRepository : EntitiesRepository<EventRoom>, IEventRoomsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventRoomsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventRoom>> GetAllAsync()
        {
            return await _objectContext.EventRooms
                .Include("Room")
                .Include("Event")
                .ToListAsync();
        }

        public async Task<List<EventRoom>> GetAllAsync(Expression<Func<EventRoom, bool>> expression)
        {
            return await _objectContext.EventRooms
                .Where(expression)
                .Include("Room")
                .Include("Event")
                .ToListAsync();
        }

        public override void Add(EventRoom entity)
        {
            _objectContext.EventRooms.AddObject(entity);
        }

        public override void Delete(EventRoom entity)
        {
            _objectContext.EventRooms.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventRoom> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventRooms);
        }

        public void Refresh(RefreshMode mode)
        {
            _objectContext.DetectChanges();
            _objectContext.Refresh(mode, _objectContext.EventRooms);
        }

        public List<EventRoom> SetRoomsCurrentValues(List<EventRoom> eventRooms)
        {
            _objectContext.DetectChanges();
            IEnumerable<EventRoom> modifiedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).Where(x => x.EntitySet.Name == _objectContext.EventRooms.EntitySet.Name).Select(x => (EventRoom)x.Entity).ToList();
            IEnumerable<EventRoom> deletedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).Where(x => x.EntitySet.Name == _objectContext.EventRooms.EntitySet.Name).Select(x => (EventRoom)x.Entity).ToList();
            IEnumerable<EventRoom> addedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(x => x.EntitySet.Name == _objectContext.EventRooms.EntitySet.Name).Select(x => (EventRoom)x.Entity).ToList();

            deletedCollection.ForEach(deletedRoom => eventRooms.Remove(deletedRoom));
            eventRooms.AddRange(addedCollection);
            modifiedCollection.ForEach(modifiedEntity => eventRooms.RemoveAll(x => x.ID == modifiedEntity.ID));
            eventRooms.AddRange(modifiedCollection);
            return eventRooms;

        }
    }
}
