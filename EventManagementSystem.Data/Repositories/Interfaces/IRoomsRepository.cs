using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IRoomsRepository
    {
        Task<List<Room>> GetAllAsync();

        void Add(Room entity);
        void Delete(Room entity);
        void Refresh();
        void Refresh(RefreshMode mode);
        void Refresh(Room entity);

    }
}
