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
    public class ProductGroupsRepository : EntitiesRepository<ProductGroup>, IProductGroupsRepository
    {
        private readonly EmsEntities _objectContext;

        public ProductGroupsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<ProductGroup>> GetAllAsync()
        {
            return await _objectContext.ProductGroups.ToListAsync();
        }

        public async Task<List<ProductGroup>> GetAllAsync(Expression<Func<ProductGroup, bool>> expression)
        {
            return await _objectContext.ProductGroups.Where(expression).ToListAsync();
        }

        public override void Add(ProductGroup entity)
        {
            _objectContext.ProductGroups.AddObject(entity);
        }

        public override void Delete(ProductGroup entity)
        {
            _objectContext.ProductGroups.DeleteObject(entity);
        }
        public void Refresh(System.Data.Entity.Core.Objects.RefreshMode refreshMode)
        {
            _objectContext.Refresh(refreshMode, _objectContext.ProductGroups);
        }
    }
}
