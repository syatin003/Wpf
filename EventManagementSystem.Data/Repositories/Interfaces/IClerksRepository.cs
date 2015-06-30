using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IClerksRepository
    {
        Task<List<Clerk>> GetAllAsync();

        void Add(Clerk entity);
        void Delete(Clerk entity);
    }
}
