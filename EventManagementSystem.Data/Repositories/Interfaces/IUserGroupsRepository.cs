using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IUserGroupsRepository
    {
        Task<List<UserGroup>> GetAllAsync();

        void Add(UserGroup entity);
        void Delete(UserGroup entity);
        void Refresh();
        void RefreshUserGroupsWithPermissions();
        void RevertAllChanges();
    }
}
