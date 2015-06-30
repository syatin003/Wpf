using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillProductsRepository
    {
        Task<List<TillProduct>> GetAllAsync();

        void Add(TillProduct entity);
        void Delete(TillProduct entity);
    }
}
