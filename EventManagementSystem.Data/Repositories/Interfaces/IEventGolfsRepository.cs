using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Data.Entity.Core.Objects;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventGolfsRepository
    {
        Task<List<EventGolf>> GetAllAsync();
        Task<List<EventGolf>> GetAllAsync(Expression<Func<EventGolf, bool>> expression);
        Task<List<EventGolf>> GetAllAsyncGolfResources(Expression<Func<EventGolf, bool>> expression);

        void Add(EventGolf entity);
        void Delete(EventGolf entity);

        void Delete(IEnumerable<EventGolf> entities);
        void Refresh();
        void Refresh(RefreshMode mode);
        void DetachGolfEvent(Guid golfEventId);
        List<EventGolf> SetGolfCurrentValues(List<EventGolf> eventGolfs);
        void ApplyRelationalChanges();
    }
}
