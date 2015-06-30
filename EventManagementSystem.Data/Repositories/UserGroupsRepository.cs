using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using System.Linq;

namespace EventManagementSystem.Data.Repositories
{
    public class UserGroupsRepository : EntitiesRepository<UserGroup>, IUserGroupsRepository
    {
        private readonly EmsEntities _objectContext;

        public UserGroupsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<UserGroup>> GetAllAsync()
        {
            return await _objectContext.UserGroups
                .Include("Users")
                .Include("Users.UserPermissions")
                .Include("UserGroupPermissions")
                .ToListAsync();
        }

        public override void Add(UserGroup entity)
        {
            _objectContext.UserGroups.AddObject(entity);
        }

        public override void Delete(UserGroup entity)
        {
            _objectContext.UserGroups.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.UserGroups);
        }

        public void RefreshUserGroupsWithPermissions()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.UserGroups);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.UserGroupPermissions);
        }

        public void RevertAllChanges()
        {
            _objectContext.DetectChanges();

            IEnumerable<object> modifiedandDeletedcollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Deleted).Select(x => x.Entity).ToList();
            _objectContext.Refresh(RefreshMode.StoreWins, modifiedandDeletedcollection);

            IEnumerable<object> addedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Select(x => x.Entity).ToList();

            addedCollection.ForEach(addedEntity => _objectContext.Detach(addedEntity));
        }
    }
}
