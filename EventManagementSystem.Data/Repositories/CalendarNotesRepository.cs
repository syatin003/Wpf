using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class CalendarNotesRepository : EntitiesRepository<CalendarNote>, ICalendarNotesRepository
    {
        private readonly EmsEntities _objectContext;

        public CalendarNotesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<CalendarNote>> GetAllAsync()
        {
            return await _objectContext.CalendarNotes.ToListAsync();
        }

        public override void Add(CalendarNote entity)
        {
            _objectContext.CalendarNotes.AddObject(entity);
        }

        public override void Delete(CalendarNote entity)
        {
            _objectContext.CalendarNotes.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.CalendarNotes);
        }
    }
}
