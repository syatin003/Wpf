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
    public class EventContactsRepository : EntitiesRepository<EventContact>, IEventContactsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventContactsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventContact>> GetAllAsync()
        {
            return await _objectContext.EventContacts
                .Include("Contact")
                .ToListAsync();
        }

        public async Task<List<EventContact>> GetAllAsync(Expression<Func<EventContact, bool>> expression)
        {
            return await _objectContext.EventContacts
                .Where(expression)
                .Include("Contact")
                .ToListAsync();
        }

        public override void Add(EventContact entity)
        {
            _objectContext.EventContacts.AddObject(entity);
        }

        public override void Delete(EventContact entity)
        {
            _objectContext.EventContacts.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventContact> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventContacts);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);
        }
    }
}
