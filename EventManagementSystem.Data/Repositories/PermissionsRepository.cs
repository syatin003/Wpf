using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class PermissionsRepository : EntitiesRepository<Permission>, IPermissionsRepository
    {
        private readonly EmsEntities _objectContext;

        public PermissionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _objectContext.Permissions.ToListAsync();
        }

        public override void Add(Permission entity)
        {
            _objectContext.Permissions.AddObject(entity);
        }

        public override void Delete(Permission entity)
        {
            _objectContext.Permissions.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Permissions);
        }
    }
}
