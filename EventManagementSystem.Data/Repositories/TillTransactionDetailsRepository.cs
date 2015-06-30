using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class TillTransactionDetailsRepository : EntitiesRepository<TillTransactionDetail>, ITillTransactionDetailsRepository
    {
        private readonly EmsEntities _objectContext;

        public TillTransactionDetailsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TillTransactionDetail>> GetAllAsync()
        {
            return await _objectContext.TillTransactionDetails.ToListAsync();
        }

        public override void Add(TillTransactionDetail entity)
        {
            _objectContext.TillTransactionDetails.AddObject(entity);
        }

        public override void Delete(TillTransactionDetail entity)
        {
            _objectContext.TillTransactionDetails.DeleteObject(entity);
        }
    }
}
