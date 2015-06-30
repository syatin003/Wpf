using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventBookedProductsRepository
    {
        Task<List<EventBookedProduct>> GetAllAsync();
        Task<List<EventBookedProduct>> GetAllAsync(Expression<Func<EventBookedProduct, bool>> expression);
        Task<List<EventBookedProduct>> GetAllAsyncWithIncludeForwardBook(Expression<Func<EventBookedProduct, bool>> expression);

        void Add(EventBookedProduct entity);
        void Delete(EventBookedProduct entity);
        void Delete(IEnumerable<EventBookedProduct> entities);
        void Refresh();
    }
}
