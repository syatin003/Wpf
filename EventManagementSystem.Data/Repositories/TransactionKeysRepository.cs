using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class TransactionKeysRepository : EntitiesRepository<TransactionKey>, ITransactionKeysRepository
    {
        private readonly EmsEntities _objectContext;

        public TransactionKeysRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TransactionKey>> GetAllAsync()
        {
            return await _objectContext.TransactionKeys.ToListAsync();
        }

        public override void Add(TransactionKey entity)
        {
           _objectContext.TransactionKeys.AddObject(entity);
        }

        public override void Delete(TransactionKey entity)
        {
           _objectContext.TransactionKeys.DeleteObject(entity);
        }
    }
}
