using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class ProductEventTypesRepository : EntitiesRepository<ProductEventType>, IProductEventTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public ProductEventTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<ProductEventType>> GetAllAsync()
        {
            return await _objectContext.ProductEventTypes.ToListAsync();
        }

        public async Task<List<ProductEventType>> GetAllAsync(Expression<Func<ProductEventType, bool>> expression)
        {
            return await _objectContext.ProductEventTypes.Where(expression).ToListAsync();
        }

        public override void Add(ProductEventType entity)
        {
            _objectContext.ProductEventTypes.AddObject(entity);
        }

        public override void Delete(ProductEventType entity)
        {
            _objectContext.ProductEventTypes.DeleteObject(entity);
        }

        public void Delete(IEnumerable<ProductEventType> entities)
        {
            entities.ForEach(Delete);
        }
    }
}
