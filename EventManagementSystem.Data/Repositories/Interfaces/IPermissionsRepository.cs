using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IPermissionsRepository
    {
        Task<List<Permission>> GetAllAsync();

        void Add(Permission entity);
        void Delete(Permission entity);
        void Refresh();
    }
}
