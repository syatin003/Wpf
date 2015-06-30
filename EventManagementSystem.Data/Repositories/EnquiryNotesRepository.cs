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
    public class EnquiryNotesRepository : EntitiesRepository<EnquiryNote>, IEnquiryNotesRepository
    {
        private readonly EmsEntities _objectContext;

        public EnquiryNotesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EnquiryNote>> GetAllAsync()
        {
            return await _objectContext.EnquiryNotes.ToListAsync();
        }

        public async Task<List<EnquiryNote>> GetAllAsync(Expression<Func<EnquiryNote, bool>> expression)
        {
            return await _objectContext.EnquiryNotes
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(EnquiryNote entity)
        {
            _objectContext.EnquiryNotes.AddObject(entity);
        }

        public override void Delete(EnquiryNote entity)
        {
            _objectContext.EnquiryNotes.DeleteObject(entity);
        }
        public void SetEntityModified(EnquiryNote entity)
        {
            _objectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

        }
    }
}