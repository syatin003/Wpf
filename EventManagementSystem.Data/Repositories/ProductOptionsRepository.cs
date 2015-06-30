using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class ProductOptionsRepository : EntitiesRepository<ProductOption>, IProductOptionsRepository
    {
        private readonly EmsEntities _objectContext;

        public ProductOptionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<ProductOption>> GetAllAsync()
        {
            return await _objectContext.ProductOptions.ToListAsync();
        }

        public override void Add(ProductOption entity)
        {
            _objectContext.ProductOptions.AddObject(entity);
        }

        public override void Delete(ProductOption entity)
        {
            _objectContext.ProductOptions.DeleteObject(entity);
        }
    }
}