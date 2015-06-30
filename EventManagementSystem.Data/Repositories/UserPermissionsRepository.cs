using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class UserPermissionsRepository : EntitiesRepository<UserPermission>, IUserPermissionsRepository
    {
        private readonly EmsEntities _objectContext;

        public UserPermissionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<UserPermission>> GetAllAsync()
        {
            return await _objectContext.UserPermissions.ToListAsync();
        }

        public override void Add(UserPermission entity)
        {
            _objectContext.UserPermissions.AddObject(entity);
        }

        public override void Delete(UserPermission entity)
        {
            _objectContext.UserPermissions.DeleteObject(entity);
        }

        public void Delete(IEnumerable<UserPermission> entities)
        {
            entities.ForEach(Delete);
        }
    }
}
