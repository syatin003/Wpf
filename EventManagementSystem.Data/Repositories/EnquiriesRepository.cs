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
    public class EnquiriesRepository : EntitiesRepository<Enquiry>, IEnquiriesRepository
    {
        private readonly EmsEntities _objectContext;

        public EnquiriesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Enquiry>> GetLightEnquiriesAsync()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);
            return await _objectContext.Enquiries
                .Include("Contact")
                .Include("EventType")
                .Include("EventStatus")
                .Include("EnquiryStatus")
                .Include("EnquiryReceiveMethod")
                .Include("User")
                .Include("FollowUps")
                .Include("Activities")
                .Include("Events")
                .ToListAsync();
        }

        public async Task<List<Enquiry>> GetLightEnquiriesAsync(Expression<Func<Enquiry, bool>> expression)
        {
            return await _objectContext.Enquiries
                .Include("Contact")
                .Include("EventType")
                .Include("EnquiryStatus")
                .Include("EventStatus")
                .Include("EnquiryReceiveMethod")
                .Include("User")
                .Include("FollowUps")
                .Include("Activities")
                .Include("Events")
                .Where(expression)
                .ToListAsync();
        }

        public async Task<List<Enquiry>> GetFullEnquiriesAsync()
        {
            return await _objectContext.Enquiries
                .Include("User")
                .Include("EventStatus")
                .Include("Contact")
                .Include("EnquiryNotes")
                .Include("EnquiryStatus")
                .Include("EnquiryReceiveMethod")
                .Include("EnquiryUpdates")
                .Include("Campaign")
                .Include("Campaign.CampaignType")
                .Include("FollowUps")
                .Include("Activities")
                .Include("Activities.ActivityType")
                .Include("EventType")
                .Include("Events")
                .ToListAsync();
        }

        public async Task<Enquiry> GetUpdatedEnquiry(Guid enquiryId)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.FollowUps);

            var desiredEnquiry = await _objectContext.Enquiries.Include("FollowUps")
                .Include("Activities").FirstOrDefaultAsync(x => x.ID == enquiryId);
            _objectContext.Refresh(RefreshMode.StoreWins, desiredEnquiry);

            return desiredEnquiry;
        }


        public override void Add(Enquiry entity)
        {
            _objectContext.Enquiries.AddObject(entity);
        }

        public override void Delete(Enquiry entity)
        {
            _objectContext.Enquiries.DeleteObject(entity);
        }
        public void RevertAllChanges()
        {
            _objectContext.DetectChanges();

            IEnumerable<object> ModifiedandDeletedcollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Deleted).Select(x => x.Entity).ToList();
            _objectContext.Refresh(RefreshMode.StoreWins, ModifiedandDeletedcollection);


            IEnumerable<object> AddedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Select(x => x.Entity).ToList();

            AddedCollection.ForEach(addedEntity =>
            {
                _objectContext.Detach(addedEntity);
            });

        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Enquiries);
        }
    }
}