using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Data.Repositories
{
    public class MembershipUpdatesRepository : EntitiesRepository<MembershipUpdate>, IMembershipUpdatesRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipUpdatesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipUpdate>> GetAllAsync()
        {
            return await _objectContext.MembershipUpdates
                .Include("User")
                .Include("Member")
                .Include("Member.Contact")
                .ToListAsync();
        }

        public async Task<List<MembershipUpdate>> GetAllAsync(Expression<Func<MembershipUpdate, bool>> expression)
        {
            return await _objectContext.MembershipUpdates
                .Where(expression)
                .Include("User")
                .Include("Member")
                .Include("Member.Contact")
                .ToListAsync();
        }
        public async Task<List<MembershipUpdate>> GetUpdatesByDate(DateTime startDate, DateTime endDate)
        {
            var updatesWithoutNotes = _objectContext.MembershipUpdates.Where(update => update.Date >= startDate && update.Date <= endDate && update.Field != "Notes");

            var notesUpdates = _objectContext.MembershipUpdates.Where(update => update.Date >= startDate && update.Date <= endDate && update.Field == "Notes");

            var notesUpdatesHistory = _objectContext.MembershipUpdates.Where(x => notesUpdates.Select(y => y.ItemId).Contains(x.ItemId));

            return await updatesWithoutNotes.Union(notesUpdatesHistory)
                        .Include("User")
                        .Include("Member")
                        .Include("Member.Contact")
                        .ToListAsync();
        }
        public override void Add(MembershipUpdate entity)
        {
            _objectContext.MembershipUpdates.AddObject(entity);
        }

        public override void Delete(MembershipUpdate entity)
        {
            _objectContext.MembershipUpdates.DeleteObject(entity);
        }

        public void Delete(IEnumerable<MembershipUpdate> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipUpdates);
        }

        
    }
}
