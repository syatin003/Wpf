using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IProductTypesRepository
    {
        Task<List<ProductType>> GetAllAsync();

        void Add(ProductType entity);
        void Delete(ProductType entity);
    }
}
