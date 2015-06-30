using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Core.Security;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class UsersRepository : EntitiesRepository<User>, IUsersRepository
    {
        private readonly EmsEntities _objectContext;
        private readonly SaltedHash _saltedHash;

        public UsersRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
            _saltedHash = new SaltedHash();
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
            //string newSalt = Guid.NewGuid().ToString("N");
            //string newHash = _saltedHash.ComputeHash(password + newSalt);

            var candidate = await _objectContext.Users
                .Include("UserPermissions")
                .Include("UserPermissions.Permission")
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (candidate == null) return null;

            password = password + candidate.PasswordSalt;

            string hash = _saltedHash.ComputeHash(password);
            return candidate.PasswordHash.Equals(hash) ? candidate : null;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _objectContext.Users
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersAsync(Expression<Func<User, bool>> expression)
        {
            return await _objectContext.Users
                .Where(expression)
                .ToListAsync();
        }
        public async Task<User> GetUserAsync(Guid userID)
        {
            return await _objectContext.Users
                 .Include("UserPermissions")
                .Include("UserPermissions.Permission")
                .Where(user => user.ID == userID)
                .FirstOrDefaultAsync();
        }
        public override void Add(User entity)
        {
            _objectContext.Users.AddObject(entity);
        }

        public override void Delete(User entity)
        {
            _objectContext.Users.DeleteObject(entity);
        }

        public void Delete(IEnumerable<User> users)
        {
            users.ForEach(Delete);
        }

        public async Task Refresh()
        {
            await _objectContext.RefreshAsync(RefreshMode.StoreWins, _objectContext.Users);
            await _objectContext.RefreshAsync(RefreshMode.StoreWins, _objectContext.UserPermissions);
        }
        public void Refresh(RefreshMode mode)
        {
            _objectContext.Refresh(mode, _objectContext.Users);
            _objectContext.Refresh(mode, _objectContext.UserPermissions);
        }
        public async Task Refresh(User user)
        {
            await _objectContext.RefreshAsync(RefreshMode.StoreWins, user);
            await _objectContext.RefreshAsync(RefreshMode.StoreWins, user.UserPermissions);
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
