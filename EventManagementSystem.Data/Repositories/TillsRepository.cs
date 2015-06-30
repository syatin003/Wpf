using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Data.Entity.Core.Objects;

namespace EventManagementSystem.Data.Repositories
{
    public class TillsRepository : EntitiesRepository<Till>, ITillsRepository
    {
        private readonly EmsEntities _objectContext;

        public TillsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Till>> GetAllAsync()
        {
            return await _objectContext.Tills.ToListAsync();
        }
        public async Task<List<Till>> GetAllAsync(Expression<Func<Till, bool>> expression)
        {
            return await _objectContext.Tills.Where(expression).ToListAsync();
        }
        public override void Add(Till entity)
        {
            _objectContext.Tills.AddObject(entity);
        }

        public override void Delete(Till entity)
        {
            _objectContext.Tills.DeleteObject(entity);
        }

        public void Refresh(Till entity)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, entity);
            _objectContext.Refresh(RefreshMode.StoreWins, entity.TillDivision);
        }

    }
}
