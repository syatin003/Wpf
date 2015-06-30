using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IUserGroupPermissionsRepository
    {
        Task<List<UserGroupPermission>> GetAllAsync();

        void Add(UserGroupPermission entity);
        void Delete(UserGroupPermission entity);
        void Delete(IEnumerable<UserGroupPermission> entities);
    }
}
