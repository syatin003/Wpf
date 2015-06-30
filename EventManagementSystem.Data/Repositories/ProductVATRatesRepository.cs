using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class ProductVatRatesRepository : EntitiesRepository<ProductVATRate>, IProductVatRatesRepository
    {
        private readonly EmsEntities _objectContext;

        public ProductVatRatesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<ProductVATRate>> GetAllAsync()
        {
            return await _objectContext.ProductVATRates.ToListAsync();
        }

        public override void Add(ProductVATRate entity)
        {
            _objectContext.ProductVATRates.AddObject(entity);
        }

        public override void Delete(ProductVATRate entity)
        {
            _objectContext.ProductVATRates.DeleteObject(entity);
        }
        public void Refresh(System.Data.Entity.Core.Objects.RefreshMode refreshMode)
        {
            _objectContext.Refresh(refreshMode, _objectContext.ProductVATRates);
        }
    }
}
