using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEventsRepository
    {
        Task<List<Event>> GetLightEventsAsync(Expression<Func<Event, bool>> expression);
        Task<List<Event>> GetEventsForReportsAsync(Expression<Func<Event, bool>> expression);
        Task<List<Event>> GetLightEventsAsync();

        Task<Event> GetUpdatedEvent(Guid eventId);
        void Refresh();
        void RefreshSynopsisReportsData();

        void Add(Event entity);
        void Delete(Event entity);

        int GetNextInvoiceNumber();
        void RevertAllChanges();


        void DetectChanges();
    }
}
