using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IProductEventTypesRepository
    {
        Task<List<ProductEventType>> GetAllAsync();
        Task<List<ProductEventType>> GetAllAsync(Expression<Func<ProductEventType, bool>> expression);

        void Add(ProductEventType entity);
        void Delete(ProductEventType entity);
        void Delete(IEnumerable<ProductEventType> entities);
    }
}
