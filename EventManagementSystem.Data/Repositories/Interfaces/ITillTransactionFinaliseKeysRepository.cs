using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillTransactionFinaliseKeysRepository
    {
        Task<List<TillTransactionFinaliseKey>> GetAllAsync();

        void Add(TillTransactionFinaliseKey entity);
        void Delete(TillTransactionFinaliseKey entity);
    }
}
