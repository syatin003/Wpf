using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class EventBookedProductsRepository : EntitiesRepository<EventBookedProduct>, IEventBookedProductsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventBookedProductsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventBookedProduct>> GetAllAsync()
        {
            return await _objectContext
                .EventBookedProducts
                .Include("Event")
                .Include("Event.EventStatus")
                .Include("Event.EventType")
                .Include("EventCharge")
                .Include("Product.ProductDepartment")
                .Include("Product.ProductGroup")
                .Include("Product.ProductVATRate")
                .ToListAsync();
        }

        public async Task<List<EventBookedProduct>> GetAllAsync(Expression<Func<EventBookedProduct, bool>> expression)
        {
            return await _objectContext
                .EventBookedProducts
                .Where(expression)
                .Include("Event")
                .Include("Event.EventStatus")
                .Include("Event.EventType")
                .Include("EventCharge")
                .Include("Product.ProductDepartment")
                .Include("Product.ProductGroup")
                .Include("Product.ProductVATRate")
                .Include("Product.ProductOption")
                .ToListAsync();
        }
        public async Task<List<EventBookedProduct>> GetAllAsyncWithIncludeForwardBook(Expression<Func<EventBookedProduct, bool>> expression)
        {
            return await _objectContext
                .EventBookedProducts
                .Join(_objectContext.EventCaterings,
                bookedProduct => bookedProduct.EventBookingItemID,
                cateringItem => cateringItem.ID,
                (bookedProduct, cateringItem) => new { bookedProduct, cateringItem })
                .Where(p => p.cateringItem.IncludeInForwardBook)
                .Select(p => p.bookedProduct)
                .Union(_objectContext
                .EventBookedProducts
                .Join(_objectContext.EventGolfs,
                bookedProduct => bookedProduct.EventBookingItemID,
                golfItem => golfItem.ID,
                (bookedProduct, golfItem) => new { bookedProduct, golfItem })
                .Where(p => p.golfItem.IncludeInForwardBook)
                .Select(p => p.bookedProduct))
                .Union(_objectContext
                .EventBookedProducts
                .Join(_objectContext.EventRooms,
                bookedProduct => bookedProduct.EventBookingItemID,
                roomItem => roomItem.ID,
                (bookedProduct, roomItem) => new { bookedProduct, roomItem })
                .Where(p => p.roomItem.IncludeInForwardBook)
                .Select(p => p.bookedProduct))
                .Union(_objectContext
                .EventBookedProducts
                .Join(_objectContext.EventInvoices,
                bookedProduct => bookedProduct.EventBookingItemID,
                invoiceItem => invoiceItem.ID,
                (bookedProduct, invoiceItem) => new { bookedProduct, invoiceItem })
                .Where(p => p.invoiceItem.IncludeInForwardBook)
                .Select(p => p.bookedProduct))
                .Where(expression)
                .Include("Event")
                .Include("Event.EventStatus")
                .Include("Event.EventType")
                .Include("EventCharge")
                .Include("Product.ProductDepartment")
                .Include("Product.ProductGroup")
                .Include("Product.ProductVATRate")
                .Include("Product.ProductOption")
                .ToListAsync();
        }

        public override void Add(EventBookedProduct entity)
        {
            _objectContext.EventBookedProducts.AddObject(entity);
        }

        public override void Delete(EventBookedProduct entity)
        {
            try
            {
                _objectContext.EventBookedProducts.DeleteObject(entity);
            }
            catch (Exception)
            {
            }
        }

        public void Delete(IEnumerable<EventBookedProduct> entity)
        {
            entity.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventBookedProducts);
        }
    }
}
