using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class TillTransactionGroupRepository : EntitiesRepository<TillTransactionGroup>, ITillTransactionGroupsRepository
    {
        private readonly EmsEntities _objectContext;

        public TillTransactionGroupRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TillTransactionGroup>> GetAllAsync()
        {
            return await _objectContext.TillTransactionGroups.ToListAsync();
        }

        public override void Add(TillTransactionGroup entity)
        {
            _objectContext.TillTransactionGroups.AddObject(entity);
        }

        public override void Delete(TillTransactionGroup entity)
        {
            _objectContext.TillTransactionGroups.DeleteObject(entity);
        }
       
    }
}