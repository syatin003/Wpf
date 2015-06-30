using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventTypesRepository
    {
        Task<List<EventType>> GetAllAsync();
        Task<List<EventType>> GetAllAsyncWithDefaultToDos();
        Task<List<EventType>> GetAllAsync(Expression<Func<EventType, bool>> expression);

        void Add(EventType entity);
        void Delete(EventType entity);
        void Refresh();
        void Refresh(EventType entity);
    }
}
