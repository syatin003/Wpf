using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class GolfHolesRepository : EntitiesRepository<GolfHole>, IGolfHolesRepository
    {
        private readonly EmsEntities _objectContext;

        public GolfHolesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<GolfHole>> GetAllAsync()
        {
            return await _objectContext.GolfHoles.ToListAsync();
        }

        public override void Add(GolfHole entity)
        {
            _objectContext.GolfHoles.AddObject(entity);
        }

        public override void Delete(GolfHole entity)
        {
            _objectContext.GolfHoles.DeleteObject(entity);
        }

        public void Refresh(RefreshMode mode)
        {
            _objectContext.DetectChanges();
            _objectContext.Refresh(mode, _objectContext.GolfHoles);
        }
    }
}
