using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class TillTransactionFinaliseKeysRepository : EntitiesRepository<TillTransactionFinaliseKey>, ITillTransactionFinaliseKeysRepository
    {
        private readonly EmsEntities _objectContext;

        public TillTransactionFinaliseKeysRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TillTransactionFinaliseKey>> GetAllAsync()
        {
            return await _objectContext.TillTransactionFinaliseKeys.ToListAsync();
        }

        public override void Add(TillTransactionFinaliseKey entity)
        {
            _objectContext.TillTransactionFinaliseKeys.AddObject(entity);
        }

        public override void Delete(TillTransactionFinaliseKey entity)
        {
            _objectContext.TillTransactionFinaliseKeys.DeleteObject(entity);
        }
    }
}
