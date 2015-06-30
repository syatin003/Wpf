using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class UserDepartmentsRepository : EntitiesRepository<UserDepartment>, IUserDepartmentsRepository
    {
        private readonly EmsEntities _objectContext;

        public UserDepartmentsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<UserDepartment>> GetAllAsync()
        {
            return await _objectContext.UserDepartments.ToListAsync();
        }

        public override void Add(UserDepartment entity)
        {
            _objectContext.UserDepartments.AddObject(entity);
        }

        public override void Delete(UserDepartment entity)
        {
            _objectContext.UserDepartments.DeleteObject(entity);
        }
    }
}
