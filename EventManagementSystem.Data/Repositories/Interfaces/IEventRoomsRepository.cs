using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Data.Entity.Core.Objects;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventRoomsRepository
    {
        Task<List<EventRoom>> GetAllAsync();
        Task<List<EventRoom>> GetAllAsync(Expression<Func<EventRoom, bool>> expression);

        void Add(EventRoom entity);
        void Delete(EventRoom entity);
        void Delete(IEnumerable<EventRoom> entities);
        void Refresh();
        void Refresh(RefreshMode mode);
        List<EventRoom> SetRoomsCurrentValues(List<EventRoom> eventRooms);
    }
}
