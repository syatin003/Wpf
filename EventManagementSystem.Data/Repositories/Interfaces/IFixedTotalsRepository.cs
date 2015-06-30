using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IFixedTotalsRepository
    {
        Task<List<FixedTotal>> GetAllAsync();

        void Add(FixedTotal entity);
        void Delete(FixedTotal entity);
    }
}
