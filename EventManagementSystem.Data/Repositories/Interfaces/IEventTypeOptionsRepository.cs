using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventTypeOptionsRepository
    {
        Task<List<EventTypeOption>> GetAllAsync();

        void Add(EventTypeOption entity);
        void Delete(EventTypeOption entity);
        void Delete(IEnumerable<EventTypeOption> entities);
    }
}
