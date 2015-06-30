using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IFinaliseKeysRepository
    {
        Task<List<FinaliseKey>> GetAllAsync();

        void Add(FinaliseKey entity);
        void Delete(FinaliseKey entity);
    }
}
