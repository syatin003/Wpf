using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class RoomsRepository : EntitiesRepository<Room>, IRoomsRepository
    {
        private readonly EmsEntities _objectContext;

        public RoomsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _objectContext.Rooms
                .Include("RoomFacilities")
                .Include("RoomFacilities.Facility")
                .ToListAsync();
        }

        public override void Add(Room entity)
        {
            _objectContext.Rooms.AddObject(entity);
        }

        public override void Delete(Room entity)
        {
            _objectContext.Rooms.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Rooms);
        }

        public void Refresh(RefreshMode mode)
        {
            _objectContext.DetectChanges();
            _objectContext.Refresh(mode, _objectContext.Rooms);
        }

        public void Refresh(Room entity)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, entity);
        }
    }
}
