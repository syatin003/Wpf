using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventInvoicesRepository
    {
        Task<List<EventInvoice>> GetAllAsync();
        Task<List<EventInvoice>> GetAllAsync(Expression<Func<EventInvoice, bool>> expression);

        void Add(EventInvoice entity);
        void Delete(EventInvoice entity);
        void Delete(IEnumerable<EventInvoice> entities);
        void Refresh();
    }
}
