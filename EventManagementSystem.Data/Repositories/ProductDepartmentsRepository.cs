using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class ProductDepartmentsRepository : EntitiesRepository<ProductDepartment>, IProductDepartmentsRepository
    {
        private readonly EmsEntities _objectContext;

        public ProductDepartmentsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<ProductDepartment>> GetAllAsync()
        {
            return await _objectContext.ProductDepartments.ToListAsync();
        }

        public async Task<List<ProductDepartment>> GetAllAsync(Expression<Func<ProductDepartment, bool>> expression)
        {
            return await _objectContext.ProductDepartments.Where(expression).ToListAsync();
        }

        public override void Add(ProductDepartment entity)
        {
            _objectContext.ProductDepartments.AddObject(entity);
        }

        public override void Delete(ProductDepartment entity)
        {
            _objectContext.ProductDepartments.DeleteObject(entity);
        }


        public void Refresh(System.Data.Entity.Core.Objects.RefreshMode refreshMode)
        {
            _objectContext.Refresh(refreshMode, _objectContext.ProductDepartments);
        }
    }
}
