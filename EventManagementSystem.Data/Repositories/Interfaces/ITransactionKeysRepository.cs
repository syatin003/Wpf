using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITransactionKeysRepository
    {
        Task<List<TransactionKey>> GetAllAsync();

        void Add(TransactionKey entity);
        void Delete(TransactionKey entity);
    }
}
