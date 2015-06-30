using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class InvoicesRepository : EntitiesRepository<Invoice>, IInvoicesRepository
    {
        private readonly EmsEntities _objectContext;

        public InvoicesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Invoice>> GetAllAsync()
        {
            return await _objectContext.Invoices
                .Include("Event")
                .Include("Event.Contact")
                .Include("Event.EventContacts")
                .ToListAsync();
        }

        public async Task<List<Invoice>> GetAllAsync(Expression<Func<Invoice, bool>> expression)
        {
            return await _objectContext.Invoices
                .Include("Event")
                .Include("Event.Contact")
                .Include("Event.EventContacts")
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(Invoice entity)
        {
            _objectContext.Invoices.AddObject(entity);
        }

        public override void Delete(Invoice entity)
        {
            _objectContext.Invoices.DeleteObject(entity);
        }
    }
}
