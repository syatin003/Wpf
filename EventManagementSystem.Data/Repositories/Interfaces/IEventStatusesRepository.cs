using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventStatusesRepository
    {
        Task<List<EventStatus>> GetAllAsync();

        void Add(EventStatus entity);
        void Delete(EventStatus entity);
        void Refresh();
        void Refresh(EventStatus entity);
    }
}
