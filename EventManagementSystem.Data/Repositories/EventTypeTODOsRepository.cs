using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Data.Entity.Core.Objects;

namespace EventManagementSystem.Data.Repositories
{
    public class EventTypeTODOsRepository : EntitiesRepository<EventTypeTODO>, IEventTypeTODOsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventTypeTODOsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventTypeTODO>> GetAllAsync()
        {
            return await _objectContext.EventTypeTODOs.ToListAsync();
        }

        public async Task<List<EventTypeTODO>> GetAllAsync(Expression<Func<EventTypeTODO, bool>> expression)
        {
            return await _objectContext.EventTypeTODOs
                .Where(expression).ToListAsync();
        }

        public override void Add(EventTypeTODO entity)
        {
            _objectContext.EventTypeTODOs.AddObject(entity);
        }

        public override void Delete(EventTypeTODO entity)
        {
            _objectContext.EventTypeTODOs.DeleteObject(entity);
        }
    }
}