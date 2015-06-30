using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class EnquiryReceiveMethodsRepository : EntitiesRepository<EnquiryReceiveMethod>, IEnquiryReceiveMethodsRepository
    {
        private readonly EmsEntities _objectContext;

        public EnquiryReceiveMethodsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EnquiryReceiveMethod>> GetAllAsync()
        {
            return await _objectContext.EnquiryReceiveMethods.ToListAsync();
        }

        public override void Add(EnquiryReceiveMethod entity)
        {
            _objectContext.EnquiryReceiveMethods.AddObject(entity);
        }

        public override void Delete(EnquiryReceiveMethod entity)
        {
            _objectContext.EnquiryReceiveMethods.DeleteObject(entity);
        }
    }
}
