using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventUpdatesRepository
    {
        Task<List<EventUpdate>> GetAllAsync();
        Task<List<EventUpdate>> GetAllAsync(Expression<Func<EventUpdate, bool>> expression);

        Task<List<EventUpdate>> GetEventUpdatesByDate(DateTime startDate, DateTime endDate);

        void Add(EventUpdate entity);
        void Delete(EventUpdate entity);
        void Delete(IEnumerable<EventUpdate> entities);
        void Refresh();
    }
}
