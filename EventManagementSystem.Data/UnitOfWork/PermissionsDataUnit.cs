using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;
using EventManagementSystem.Data.UnitOfWork.Interfaces;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class PermissionsDataUnit : EntitiesUnitOfWork, IPermissionsDataUnit
    {
        public PermissionsDataUnit()
            : base(new EmsEntities())
        {
            RegisterRepository<User, UsersRepository>();
        }

        public IUsersRepository UsersRepository
        {
            get { return (IUsersRepository)GetRepository<User>(); }
        }
    }
}
