using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IProductDepartmentsRepository
    {
        Task<List<ProductDepartment>> GetAllAsync();
        Task<List<ProductDepartment>> GetAllAsync(Expression<Func<ProductDepartment, bool>> expression);

        void Add(ProductDepartment entity);
        void Delete(ProductDepartment entity);
        void Refresh(System.Data.Entity.Core.Objects.RefreshMode refreshMode);
    }
}
