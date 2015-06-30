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
    public class TillTransactionsRepository : EntitiesRepository<TillTransaction>, ITillTransactionsRepository
    {
        private readonly EmsEntities _objectContext;

        public TillTransactionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TillTransaction>> GetAllAsync()
        {
            return await _objectContext.TillTransactions.ToListAsync();
        }

        public async Task<List<TillTransaction>> GetTransactionsWithRelatedData(DateTime startDate, DateTime endDate)
        {
            var query = _objectContext.TillTransactions
                .Include(tillTransaction => tillTransaction.Till)
                .Include(tillTransaction => tillTransaction.Clerk)
                .Include(tillTransaction => tillTransaction.TillTransactionProducts)
                .Include(tillTransaction => tillTransaction.TillTransactionProducts
                    .Select(tillTransactionProduct => tillTransactionProduct.TillProduct))
                .Include(tillTransaction => tillTransaction.TillTransactionFinaliseKeys)
                .Include(tillTransaction => tillTransaction.TillTransactionFinaliseKeys
                   .Select(transFinaliseKey => transFinaliseKey.FinaliseKey))
                .Where(tillTransaction => tillTransaction.Date >= startDate && tillTransaction.Date <= endDate);

            return await query.ToListAsync();
        }
        public override void Add(TillTransaction entity)
        {
            _objectContext.TillTransactions.AddObject(entity);
        }

        public override void Delete(TillTransaction entity)
        {
            _objectContext.TillTransactions.DeleteObject(entity);
        }
    }
}
