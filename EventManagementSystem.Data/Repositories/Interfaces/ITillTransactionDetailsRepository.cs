using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillTransactionDetailsRepository
    {
        Task<List<TillTransactionDetail>> GetAllAsync();

        void Add(TillTransactionDetail entity);
        void Delete(TillTransactionDetail entity);
    }
}
