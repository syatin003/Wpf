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
    public class EventInvoicesRepository : EntitiesRepository<EventInvoice>, IEventInvoicesRepository
    {
        private readonly EmsEntities _objectContext;

        public EventInvoicesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventInvoice>> GetAllAsync()
        {
            return await _objectContext.EventInvoices.ToListAsync();
        }

        public async Task<List<EventInvoice>> GetAllAsync(Expression<Func<EventInvoice, bool>> expression)
        {
            return await _objectContext.EventInvoices
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(EventInvoice entity)
        {
            _objectContext.EventInvoices.AddObject(entity);
        }

        public override void Delete(EventInvoice entity)
        {
            _objectContext.EventInvoices.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventInvoice> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventInvoices);
        }
    }
}
