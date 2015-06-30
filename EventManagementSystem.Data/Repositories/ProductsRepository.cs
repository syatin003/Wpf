using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace EventManagementSystem.Data.Repositories
{
    public class ProductsRepository : EntitiesRepository<Product>, IProductsRepository
    {
        private readonly EmsEntities _objectContext;

        public ProductsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _objectContext.Products
                .Include("ProductEventTypes")
                .Include("EventBookedProducts")
                .Include("EventBookedProducts.Event")
                .Include("ProductType")
                .Include("ProductEventTypes.EventType")
                .Include("ProductDepartment")
                .Include("ProductVATRate")
                .Include("ProductGroup")
                .Include("ProductOption")
                .ToListAsync();
        }

        public async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> expression)
        {
            return await _objectContext.Products
                .Where(expression)
                .Include("ProductEventTypes")
                .Include("EventBookedProducts")
                .Include("EventBookedProducts.Event")
                .Include("ProductType")
                .Include("ProductEventTypes.EventType")
                .Include("ProductDepartment")
                .Include("ProductVATRate")
                .Include("ProductGroup")
                .Include("ProductOption")
                .ToListAsync();
        }


        public async Task<List<Product>> GetAllAsyncAsNoTracking()
        {
            return await _objectContext.Products
                .Include("ProductEventTypes")
                .Include("EventBookedProducts")
                .Include("EventBookedProducts.Event")
                .Include("ProductType")
                .Include("ProductEventTypes.EventType")
                .Include("ProductDepartment")
                .Include("ProductVATRate")
                .Include("ProductGroup")
                .Include("ProductOption")
                .ToListAsync();
        }

        public override void Add(Product entity)
        {
            _objectContext.Products.AddObject(entity);
        }

        public override void Delete(Product entity)
        {
            _objectContext.Products.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Products);
        }
    }
}
