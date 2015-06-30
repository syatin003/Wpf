using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;

namespace EventManagementSystem.Data.UnitOfWork.Interfaces
{
    public interface IWorkspaceDataUnit : IDataUnitOfWork
    {
        IUsersRepository UsersRepository { get; }
        IEventsRepository EventsRepository { get; }
        IEventRemindersRepository EventRemindersRepository { get; }
    }
}
