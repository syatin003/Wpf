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
    public class EventPaymentsRepository : EntitiesRepository<EventPayment>, IEventPaymentsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventPaymentsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventPayment>> GetAllAsync()
        {
            return await _objectContext.EventPayments.ToListAsync();
        }

        public async Task<List<EventPayment>> GetAllAsync(Expression<Func<EventPayment, bool>> expression)
        {
            return await _objectContext.EventPayments
                .Include("Event")
                .Include("Event.Contact") // TODO: why?
                .Include("Event.EventContacts") // TODO: why?
                .Include("PaymentMethod")
                .Include("User")
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(EventPayment entity)
        {
            _objectContext.EventPayments.AddObject(entity);
        }

        public override void Delete(EventPayment entity)
        {
            _objectContext.EventPayments.DeleteObject(entity);
        }

        public void Delete(IEnumerable<EventPayment> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventPayments);
        }
    }
}
