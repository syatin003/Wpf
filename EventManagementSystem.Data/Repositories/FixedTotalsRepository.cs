using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class FixedTotalsRepository : EntitiesRepository<FixedTotal>, IFixedTotalsRepository
    {
        private readonly EmsEntities _objectContext;

        public FixedTotalsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<FixedTotal>> GetAllAsync()
        {
            return await _objectContext.FixedTotals.ToListAsync();
        }

        public override void Add(FixedTotal entity)
        {
            _objectContext.FixedTotals.AddObject(entity);
        }

        public override void Delete(FixedTotal entity)
        {
            _objectContext.FixedTotals.DeleteObject(entity);
        }
    }
}
