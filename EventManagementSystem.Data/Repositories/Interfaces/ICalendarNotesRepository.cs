using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ICalendarNotesRepository
    {
        Task<List<CalendarNote>> GetAllAsync();

        void Add(CalendarNote entity);
        void Delete(CalendarNote entity);
        void Refresh();
    }
}
