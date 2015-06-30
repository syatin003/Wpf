using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> expression);

        void Add(Product entity);
        void Delete(Product entity);
        void Refresh();
    }
}
