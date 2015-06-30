using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class CorresponcenceTypesRepository : EntitiesRepository<CorresponcenceType>, ICorresponcenceTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public CorresponcenceTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<CorresponcenceType>> GetAllAsync()
        {
            return await _objectContext.CorresponcenceTypes.ToListAsync();
        }

        public async Task<List<CorresponcenceType>> GetAllAsync(Expression<Func<CorresponcenceType, bool>> expression)
        {
            return await _objectContext.CorresponcenceTypes
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(CorresponcenceType entity)
        {
            _objectContext.CorresponcenceTypes.AddObject(entity);
        }

        public override void Delete(CorresponcenceType entity)
        {
            _objectContext.CorresponcenceTypes.DeleteObject(entity);
        }
    }
}
