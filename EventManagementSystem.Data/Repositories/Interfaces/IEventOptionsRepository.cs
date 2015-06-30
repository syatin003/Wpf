using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventOptionsRepository
    {
        Task<List<EventOption>> GetAllAsync();

        void Add(EventOption entity);
        void Delete(EventOption entity);
    }
}
