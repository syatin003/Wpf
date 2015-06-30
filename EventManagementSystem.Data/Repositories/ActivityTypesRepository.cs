using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class ActivityTypesRepository : EntitiesRepository<ActivityType>, IActivityTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public ActivityTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<ActivityType>> GetAllAsync()
        {
            return await _objectContext.ActivityTypes.ToListAsync();
        }

        public override void Add(ActivityType entity)
        {
            _objectContext.ActivityTypes.AddObject(entity);
        }

        public override void Delete(ActivityType entity)
        {
            _objectContext.ActivityTypes.DeleteObject(entity);
        }
    }
}