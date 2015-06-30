using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class PermissionGroupsRepository : EntitiesRepository<PermissionGroup>, IPermissionGroupsRepository
    {
        private readonly EmsEntities _objectContext;

        public PermissionGroupsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<PermissionGroup>> GetAllAsync()
        {
            return await _objectContext.PermissionGroups.ToListAsync();
        }

        public override void Add(PermissionGroup entity)
        {
            _objectContext.PermissionGroups.AddObject(entity);
        }

        public override void Delete(PermissionGroup entity)
        {
            _objectContext.PermissionGroups.DeleteObject(entity);
        }
    }
}
