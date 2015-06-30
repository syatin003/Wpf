using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IProductOptionsRepository
    {
        Task<List<ProductOption>> GetAllAsync();

        void Add(ProductOption entity);
        void Delete(ProductOption entity);
    }
}