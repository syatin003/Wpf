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
using Microsoft.Practices.ObjectBuilder2;


namespace EventManagementSystem.Data.Repositories
{
    public class EventsRepository : EntitiesRepository<Event>, IEventsRepository
    {
        private readonly EmsEntities _objectContext;

        public EventsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Event>> GetLightEventsAsync(Expression<Func<Event, bool>> expression)
        {
            return await _objectContext.Events
                .Include("EventStatus")
                .Include("EventType")
                .Include("Contact")
                .Include("EventContacts")
                .Where(expression)
                .ToListAsync();
        }

        public async Task<List<Event>> GetEventsForReportsAsync(Expression<Func<Event, bool>> expression)
        {
            return await _objectContext.Events
                .Include("EventPayments")
                .Include("EventCaterings")
                .Include("EventGolfs")
                .Include("EventGolfs.Golf")
                .Include("EventGolfs.GolfHole")
                .Include("EventRooms")
                .Include("EventRooms.Room")
                .Include("EventInvoices")
                .Include("EventBookedProducts")
                .Include("EventBookedProducts.EventCharge")
                .Include("EventBookedProducts.Product")
                .Include("EventNotes")
                .Where(expression)
                .ToListAsync();
        }
        public async Task<List<Event>> GetLightEventsAsync()
        {
            return await _objectContext.Events
                .Include("EventStatus")
                .Include("EventType")
                .Include("Contact")
                .Include("EventContacts")
                .ToListAsync();
        }

        public async Task<Event> GetUpdatedEvent(Guid eventId)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);

            var desiredEvent = await _objectContext.Events.FirstOrDefaultAsync(x => x.ID == eventId);
            _objectContext.Refresh(RefreshMode.StoreWins, desiredEvent);

            return desiredEvent;
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Events);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);
        }



        public override void Add(Event entity)
        {
            _objectContext.Events.AddObject(entity);
        }

        public override void Delete(Event entity)
        {
            _objectContext.Events.DeleteObject(entity);
        }

        public int GetNextInvoiceNumber()
        {
            var invoiceNumber = 1;

            if (_objectContext.Events.Any() && _objectContext.Events.Any(x => x.InvoiceNumber != null))
                invoiceNumber = (int)(_objectContext.Events.Max(x => x.InvoiceNumber) + 1);

            return invoiceNumber;
        }
        public void RevertAllChanges()
        {
            _objectContext.DetectChanges();

            IEnumerable<object> ModifiedandDeletedcollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Deleted).Select(x => x.Entity).ToList();
            _objectContext.Refresh(RefreshMode.StoreWins, ModifiedandDeletedcollection);


            IEnumerable<object> AddedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Select(x => x.Entity).ToList();

            AddedCollection.ForEach(addedEntity => _objectContext.Detach(addedEntity));

        }

        public void RefreshSynopsisReportsData()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Events);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventCaterings);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventRooms);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventGolfs);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventNotes);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventBookedProducts);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EventReminders);

        }
        public void DetectChanges()
        {
            _objectContext.DetectChanges();
        }
    }
}
