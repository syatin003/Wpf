using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;

namespace EventManagementSystem.Data.UnitOfWork.Interfaces
{
    public interface IPermissionsDataUnit : IDataUnitOfWork
    {
        IUsersRepository UsersRepository { get; }
    }
}
