using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IRoomFacilitiesRepository
    {
        Task<List<RoomFacility>> GetAllAsync();

        void Add(RoomFacility entity);
        void Delete(RoomFacility entity);
        void Delete(IEnumerable<RoomFacility> roomFacilities);
    }
}
