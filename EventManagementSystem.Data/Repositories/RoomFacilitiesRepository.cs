using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class RoomFacilitiesRepository : EntitiesRepository<RoomFacility>, IRoomFacilitiesRepository
    {
        private readonly EmsEntities _objectContext;

        public RoomFacilitiesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<RoomFacility>> GetAllAsync()
        {
            return await _objectContext.RoomFacilities.ToListAsync();
        }

        public override void Add(RoomFacility entity)
        {
            _objectContext.RoomFacilities.AddObject(entity);
        }

        public override void Delete(RoomFacility entity)
        {
            _objectContext.RoomFacilities.DeleteObject(entity);
        }

        public void Delete(IEnumerable<RoomFacility> roomFacilities)
        {
            roomFacilities.ForEach(Delete);
        }
    }
}
