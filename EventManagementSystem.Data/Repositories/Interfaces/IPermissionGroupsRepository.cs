using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IPermissionGroupsRepository
    {
        Task<List<PermissionGroup>> GetAllAsync();

        void Add(PermissionGroup entity);
        void Delete(PermissionGroup entity);

    }
}
