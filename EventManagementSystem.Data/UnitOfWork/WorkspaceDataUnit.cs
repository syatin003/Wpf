using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;
using EventManagementSystem.Data.UnitOfWork.Interfaces;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class WorkspaceDataUnit : EntitiesUnitOfWork, IWorkspaceDataUnit
    {
        public WorkspaceDataUnit()
            : base(new EmsEntities())
        {
            RegisterRepository<User, UsersRepository>();
            RegisterRepository<Event, EventsRepository>();
            RegisterRepository<EventReminder, EventRemindersRepository>();
        }

        public IEventsRepository EventsRepository
        {
            get { return (IEventsRepository)GetRepository<Event>(); }
        }

        public IUsersRepository UsersRepository
        {
            get { return (IUsersRepository)GetRepository<User>(); }
        }
        public IEventRemindersRepository EventRemindersRepository
        {
            get { return (IEventRemindersRepository)GetRepository<EventReminder>(); }
        }
    }
}
