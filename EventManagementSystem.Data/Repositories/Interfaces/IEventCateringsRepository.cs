using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Data.Entity.Core.Objects;


namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventCateringsRepository
    {
        Task<List<EventCatering>> GetAllAsync();
        Task<List<EventCatering>> GetAllAsync(Expression<Func<EventCatering, bool>> expression);

        void Add(EventCatering entity);
        void Delete(EventCatering entity);
        void Delete(IEnumerable<EventCatering> entities);
        void Refresh();
        void Refresh(RefreshMode mode);
        List<EventCatering> SetCateringsCurrentValues(List<EventCatering> eventCaterings);

    }
}
