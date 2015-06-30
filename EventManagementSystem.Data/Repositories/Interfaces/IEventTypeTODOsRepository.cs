using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventTypeTODOsRepository
    {
        Task<List<EventTypeTODO>> GetAllAsync();
        Task<List<EventTypeTODO>> GetAllAsync(Expression<Func<EventTypeTODO, bool>> expression);

        void Add(EventTypeTODO entity);
        void Delete(EventTypeTODO entity);
    }
}
