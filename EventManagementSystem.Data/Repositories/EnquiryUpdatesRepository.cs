using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class EnquiryUpdatesRepository : EntitiesRepository<EnquiryUpdate>, IEnquiryUpdatesRepository
    {
        private readonly EmsEntities _objectContext;

        public EnquiryUpdatesRepository(EmsEntities objectContext)
             : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EnquiryUpdate>> GetAllAsync()
        {
            return await _objectContext.EnquiryUpdates.ToListAsync();
        }

        public async Task<List<EnquiryUpdate>> GetAllAsync(Expression<Func<EnquiryUpdate, bool>> expression)
        {
            return await _objectContext.EnquiryUpdates
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(EnquiryUpdate entity)
        {
            _objectContext.EnquiryUpdates.AddObject(entity);
        }

        public override void Delete(EnquiryUpdate entity)
        {
            _objectContext.EnquiryUpdates.DeleteObject(entity);
        }
    }
}
