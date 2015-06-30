using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class EventChargesRepository : EntitiesRepository<EventCharge>, IEventChargesRepository
    {
        private readonly EmsEntities _objectContext;

        public EventChargesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventCharge>> GetAllAsync()
        {
            return await _objectContext.EventCharges.ToListAsync();
        }

        public async Task<List<EventCharge>> GetAllAsync(Expression<Func<EventCharge, bool>> expression)
        {
            return await _objectContext.EventCharges
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(EventCharge entity)
        {
            _objectContext.EventCharges.AddObject(entity);
        }

        public override void Delete(EventCharge entity)
        {
            try
            {
                _objectContext.EventCharges.DeleteObject(entity);
            }
            catch (Exception)
            {
            }
        }

        public void Delete(IEnumerable<EventCharge> entity)
        {
            entity.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventCharges);
        }
    }
}
