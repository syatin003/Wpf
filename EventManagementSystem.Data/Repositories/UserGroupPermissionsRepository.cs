using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class UserGroupPermissionsRepository : EntitiesRepository<UserGroupPermission>, IUserGroupPermissionsRepository
    {
        private readonly EmsEntities _objectContext;

        public UserGroupPermissionsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<UserGroupPermission>> GetAllAsync()
        {
            return await _objectContext.UserGroupPermissions.ToListAsync();
        }

        public override void Add(UserGroupPermission entity)
        {
            _objectContext.UserGroupPermissions.AddObject(entity);
        }

        public override void Delete(UserGroupPermission entity)
        {
           _objectContext.UserGroupPermissions.DeleteObject(entity);
        }

        public void Delete(IEnumerable<UserGroupPermission> entities)
        {
            entities.ForEach(Delete);
        }
    }
}
