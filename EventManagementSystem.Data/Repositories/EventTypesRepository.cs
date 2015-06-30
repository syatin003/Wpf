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

namespace EventManagementSystem.Data.Repositories
{
    public class EventTypesRepository : EntitiesRepository<EventType>, IEventTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public EventTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventType>> GetAllAsync()
        {
            return await _objectContext.EventTypes
                .Include("EventTypeOptions")
                 .Include("EventTypeTODOs")
                .Include("EventTypeTODOs.User")
                .Include("EventTypeTODOs.User1")
                .ToListAsync();
        }
        public async Task<List<EventType>> GetAllAsyncWithDefaultToDos()
        {
            return await _objectContext.EventTypes
                .Include("EventTypeOptions")
                .Include("EventTypeTODOs")
                .Include("EventTypeTODOs.User")
                .Include("EventTypeTODOs.User1")
                .ToListAsync();
        }
        public async Task<List<EventType>> GetAllAsync(Expression<Func<EventType, bool>> expression)
        {
            return await _objectContext.EventTypes
                .Where(expression)
                .Include("EventTypeOptions")
                .ToListAsync();
        }

        public override void Add(EventType entity)
        {
            _objectContext.EventTypes.AddObject(entity);
        }

        public override void Delete(EventType entity)
        {
            _objectContext.EventTypes.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventTypes);
        }
        public void Refresh(EventType entity)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, entity);

        }
    }
}
