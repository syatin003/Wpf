using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventChargesRepository
    {
        Task<List<EventCharge>> GetAllAsync();
        Task<List<EventCharge>> GetAllAsync(Expression<Func<EventCharge, bool>> expression);

        void Add(EventCharge entity);
        void Delete(EventCharge entity);
        void Delete(IEnumerable<EventCharge> entity);
        void Refresh();
    }
}
