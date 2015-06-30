using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillTransactionGroupsRepository
    {
        Task<List<TillTransactionGroup>> GetAllAsync();

        void Add(TillTransactionGroup entity);
        void Delete(TillTransactionGroup entity);
    }
}