using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IUserPermissionsRepository
    {
        Task<List<UserPermission>> GetAllAsync();

        void Add(UserPermission entity);
        void Delete(UserPermission entity);
        void Delete(IEnumerable<UserPermission> entities);
    }
}
