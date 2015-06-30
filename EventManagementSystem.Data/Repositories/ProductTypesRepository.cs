using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class ProductTypesRepository : EntitiesRepository<ProductType>, IProductTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public ProductTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<ProductType>> GetAllAsync()
        {
            return await _objectContext.ProductTypes.ToListAsync();
        }

        public override void Add(ProductType entity)
        {
            _objectContext.ProductTypes.AddObject(entity);
        }

        public override void Delete(ProductType entity)
        {
            _objectContext.ProductTypes.DeleteObject(entity);
        }
    }
}
