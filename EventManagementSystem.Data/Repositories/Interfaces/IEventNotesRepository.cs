using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventNotesRepository
    {
        Task<List<EventNote>> GetAllAsync();
        Task<List<EventNote>> GetAllAsync(Expression<Func<EventNote, bool>> expression);

        void Add(EventNote entity);
        void Delete(EventNote entity);
        void Delete(IEnumerable<EventNote> entities);
        void Refresh();
    }
}
