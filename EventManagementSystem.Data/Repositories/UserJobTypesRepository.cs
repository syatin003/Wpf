using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class UserJobTypesRepository : EntitiesRepository<UserJobType>, IUserJobTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public UserJobTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<UserJobType>> GetAllAsync()
        {
            return await _objectContext.UserJobTypes.ToListAsync();
        }

        public override void Add(UserJobType entity)
        {
            _objectContext.UserJobTypes.AddObject(entity);
        }

        public override void Delete(UserJobType entity)
        {
            _objectContext.UserJobTypes.DeleteObject(entity);
        }
    }
}
