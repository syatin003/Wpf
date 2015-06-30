using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class ClerksRepository : EntitiesRepository<Clerk>, IClerksRepository
    {
        private readonly EmsEntities _objectContext;

        public ClerksRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Clerk>> GetAllAsync()
        {
            return await _objectContext.Clerks.ToListAsync();
        }

        public override void Add(Clerk entity)
        {
           _objectContext.Clerks.AddObject(entity);
        }

        public override void Delete(Clerk entity)
        {
            _objectContext.Clerks.DeleteObject(entity);
        }
    }
}
