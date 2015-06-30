using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventPaymentsRepository
    {
        Task<List<EventPayment>> GetAllAsync();
        Task<List<EventPayment>> GetAllAsync(Expression<Func<EventPayment, bool>> expression);

        void Add(EventPayment entity);
        void Delete(EventPayment entity);
        void Delete(IEnumerable<EventPayment> entities);
        void Refresh();
    }
}
