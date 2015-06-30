using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Linq;
using System;

namespace EventManagementSystem.Data.Repositories
{
    public class TillTransactionProductsRepository : EntitiesRepository<TillTransactionProduct>, ITillTransactionProductsRepository
    {
        private readonly EmsEntities _objectContext;

        public TillTransactionProductsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TillTransactionProduct>> GetAllAsync()
        {
            return await _objectContext.TillTransactionProducts.ToListAsync();
        }

        public async Task<List<TillTransactionProduct>> GetTransactionsWithRelatedData(DateTime startDate, DateTime endDate)
        {
            var query = _objectContext.TillTransactionProducts
                 .Include(tillTransactionProduct => tillTransactionProduct.TillProduct)
                 .Include(tillTransactionProduct => tillTransactionProduct.TillProduct.Till)
                 .Include(tillTransactionProduct => tillTransactionProduct.TillProduct.ProductDepartment)
                 .Include(tillTransactionProduct => tillTransactionProduct.TillProduct.ProductGroup)
                 .Include(tillTransactionProduct => tillTransactionProduct.TillTransaction)
                 .Include(tillTransactionProduct => tillTransactionProduct.TillTransaction.TillTransactionFinaliseKeys)
                 .Include(tillTransactionProduct => tillTransactionProduct.TillTransaction.Clerk)
                 .Include(tillTransactionProduct => tillTransactionProduct.TillTransaction.Till)
                 .Include(tillTransactionProduct => tillTransactionProduct.TillTransaction.TillTransactionFinaliseKeys
                .Select(transFinaliseKey => transFinaliseKey.FinaliseKey))
                .Where(tillTransactionProduct => tillTransactionProduct.TillTransaction.Date >= startDate && tillTransactionProduct.TillTransaction.Date <= endDate);

            return await query.ToListAsync();
        }

        public override void Add(TillTransactionProduct entity)
        {
            _objectContext.TillTransactionProducts.AddObject(entity);
        }

        public override void Delete(TillTransactionProduct entity)
        {
            _objectContext.TillTransactionProducts.DeleteObject(entity);
        }
    }
}
