using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class FacilitiesRepository : EntitiesRepository<Facility>, IFacilitiesRepository
    {
        private readonly EmsEntities _objectContext;

        public FacilitiesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Facility>> GetAllAsync()
        {
            return await _objectContext.Facilities.ToListAsync();
        }

        public override void Add(Facility entity)
        {
            _objectContext.Facilities.AddObject(entity);
        }

        public override void Delete(Facility entity)
        {
            _objectContext.Facilities.DeleteObject(entity);
        }
    }
}
