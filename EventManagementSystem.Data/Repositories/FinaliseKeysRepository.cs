using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class FinaliseKeysRepository : EntitiesRepository<FinaliseKey>, IFinaliseKeysRepository
    {
        private readonly EmsEntities _objectContext;

        public FinaliseKeysRepository(EmsEntities objectContext) 
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<FinaliseKey>> GetAllAsync()
        {
            return await _objectContext.FinaliseKeys.ToListAsync();
        }

        public override void Add(FinaliseKey entity)
        {
            _objectContext.FinaliseKeys.AddObject(entity);
        }

        public override void Delete(FinaliseKey entity)
        {
            _objectContext.FinaliseKeys.DeleteObject(entity);
        }
    }
}
