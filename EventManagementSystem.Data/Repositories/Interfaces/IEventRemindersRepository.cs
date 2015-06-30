using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventRemindersRepository
    {
        Task<List<EventReminder>> GetAllAsync();
        Task<List<EventReminder>> GetAllAsync(Expression<Func<EventReminder, bool>> expression);

        Task<EventReminder> GetUpdatedEventReminder(Guid eventReminderID);

        void Add(EventReminder entity);
        void Delete(EventReminder entity);
        void SetEntityModified(EventReminder entity);
        void Refresh();
    }
}