using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IProductGroupsRepository
    {
        Task<List<ProductGroup>> GetAllAsync();
        Task<List<ProductGroup>> GetAllAsync(Expression<Func<ProductGroup, bool>> expression);

        void Add(ProductGroup entity);
        void Delete(ProductGroup entity);
         void Refresh(System.Data.Entity.Core.Objects.RefreshMode refreshMode);
    }
}
