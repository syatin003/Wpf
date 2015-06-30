using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IGolfHolesRepository
    {
        Task<List<GolfHole>> GetAllAsync();

        void Add(GolfHole entity);
        void Delete(GolfHole entity);
        void Refresh(RefreshMode mode);
    }
}
