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
    public class EnquiryStatusesRepository : EntitiesRepository<EnquiryStatus>, IEnquiryStatusesRepository
    {
        private readonly EmsEntities _objectContext;

        public EnquiryStatusesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EnquiryStatus>> GetAllAsync()
        {
            return await _objectContext.EnquiryStatuses.ToListAsync();
        }

        public async Task<List<EnquiryStatus>> GetAllAsync(Expression<Func<EnquiryStatus, bool>> expression)
        {
            return await _objectContext.EnquiryStatuses
                .Where(expression)
                .ToListAsync();
        }

        public async Task<EnquiryStatus> GetUpdatedEnquiryStatus(Guid enquiryStatusId)
        {
            var desiredEnquiryStatus = await _objectContext.EnquiryStatuses.FirstOrDefaultAsync(x => x.ID == enquiryStatusId);
            _objectContext.Refresh(RefreshMode.StoreWins, desiredEnquiryStatus);

            return desiredEnquiryStatus;
        }

        public override void Add(EnquiryStatus entity)
        {
            _objectContext.EnquiryStatuses.AddObject(entity);
        }

        public override void Delete(EnquiryStatus entity)
        {
            _objectContext.EnquiryStatuses.DeleteObject(entity);
        }

        public void Refresh(EnquiryStatus entity)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, entity);
        }
    }
}
