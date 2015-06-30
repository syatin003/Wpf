using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventContactsRepository
    {
        Task<List<EventContact>> GetAllAsync();
        Task<List<EventContact>> GetAllAsync(Expression<Func<EventContact, bool>> expression);

        void Add(EventContact entity);
        void Delete(EventContact entity);
        void Delete(IEnumerable<EventContact> entities);
        void Refresh();
    }
}
