using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class GolfsRepository : EntitiesRepository<Golf>, IGolfsRepository
    {
        private readonly EmsEntities _objectContext;

        public GolfsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Golf>> GetAllAsync()
        {
            return await _objectContext.Golfs
                .Include("GolfFollowResources")
                .ToListAsync();
        }

        public override void Add(Golf entity)
        {
            _objectContext.Golfs.AddObject(entity);
        }

        public override void Delete(Golf entity)
        {
            _objectContext.Golfs.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Golfs);
        }

        public void Refresh(RefreshMode mode)
        {
            _objectContext.DetectChanges();
            _objectContext.Refresh(mode, _objectContext.Golfs);
        }

        public void Refresh(Golf entity)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, entity);
        }

    }
}