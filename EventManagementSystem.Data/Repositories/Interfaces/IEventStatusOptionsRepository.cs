using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventStatusOptionsRepository
    {
        Task<List<EventStatusOption>> GetAllAsync();

        void Add(EventStatusOption entity);
        void Delete(EventStatusOption entity);
        void Delete(IEnumerable<EventStatusOption> entities);
    }
}
