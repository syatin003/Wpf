using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class TillDivisionsRepository : EntitiesRepository<TillDivision>, ITillDivisionsRepository
    {
        private readonly EmsEntities _objectContext;

        public TillDivisionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TillDivision>> GetAllAsync()
        {
            return await _objectContext.TillDivisions
                .Include("Tills")
                .Include("Tills.TillDivision")
                .ToListAsync();
        }

        public override void Add(TillDivision entity)
        {
            _objectContext.TillDivisions.AddObject(entity);
        }

        public override void Delete(TillDivision entity)
        {
            _objectContext.TillDivisions.DeleteObject(entity);
        }
    }
}
