using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventNoteTypesRepository
    {
        Task<List<EventNoteType>> GetAllAsync();

        void Add(EventNoteType entity);
        void Delete(EventNoteType entity);
    }
}
