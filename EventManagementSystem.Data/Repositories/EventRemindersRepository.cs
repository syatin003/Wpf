using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class EventRemindersRepository : EntitiesRepository<EventReminder>, IEventRemindersRepository
    {
        private readonly EmsEntities _objectContext;

        public EventRemindersRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EventReminder>> GetAllAsync()
        {
            return await _objectContext.EventReminders
                .Include("Event")
                .Include("Event.EventType")
                .Include("User")
                .Include("User1")
                .ToListAsync();
        }

        public async Task<List<EventReminder>> GetAllAsync(Expression<Func<EventReminder, bool>> expression)
        {
            return await _objectContext.EventReminders
                .Where(expression)
                .Include("Event")
                .Include("Event.EventType")
                .Include("User")
                .Include("User1")
                .ToListAsync();
        }

        public async Task<EventReminder> GetUpdatedEventReminder(Guid eventReminderID)
        {
            var desiredEventToDo = await _objectContext.EventReminders.FirstOrDefaultAsync(x => x.ID == eventReminderID);
           
            //if (desiredEventToDo != null)
            //    _objectContext.Refresh(RefreshMode.ClientWins, desiredEventToDo);

            return desiredEventToDo;
        }

        public override void Add(EventReminder entity)
        {
            _objectContext.EventReminders.AddObject(entity);
        }

        public override void Delete(EventReminder entity)
        {
            _objectContext.EventReminders.DeleteObject(entity);
        }

        public void SetEntityModified(EventReminder entity)
        {
            _objectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.ClientWins, _objectContext.EventReminders);
        }
    }
}