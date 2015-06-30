using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillTransactionDepartmentsRepository
    {
        Task<List<TillTransactionDepartment>> GetAllAsync();

        void Add(TillTransactionDepartment entity);
        void Delete(TillTransactionDepartment entity);
    }
}