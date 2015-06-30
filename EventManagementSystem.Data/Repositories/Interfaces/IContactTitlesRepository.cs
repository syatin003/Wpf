using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IContactTitlesRepository
    {
        Task<List<ContactTitle>> GetAllAsync();

        void Add(ContactTitle entity);
        void Delete(ContactTitle entity);
    }
}
