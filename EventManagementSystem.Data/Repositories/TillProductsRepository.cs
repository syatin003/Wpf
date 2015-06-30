using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class TillProductsRepository: EntitiesRepository<TillProduct>, ITillProductsRepository
    {
        private readonly EmsEntities _objectContext;

        public TillProductsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TillProduct>> GetAllAsync()
        {
            return await _objectContext.TillProducts.ToListAsync();
        }

        public override void Add(TillProduct entity)
        {
            _objectContext.TillProducts.AddObject(entity);
        }

        public override void Delete(TillProduct entity)
        {
            _objectContext.TillProducts.DeleteObject(entity);
        }
    }
}
