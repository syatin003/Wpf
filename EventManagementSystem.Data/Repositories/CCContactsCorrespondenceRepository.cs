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
    public class CCContactsCorrespondenceRepository : EntitiesRepository<CCContactsCorrespondence>, ICCContactsCorrespondenceRepository
    {
        private readonly EmsEntities _objectContext;

        public CCContactsCorrespondenceRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<CCContactsCorrespondence>> GetAllAsync()
        {
            return await _objectContext.CCContactsCorrespondences
               .ToListAsync();
        }

        public async Task<List<CCContactsCorrespondence>> GetAllAsync(Expression<Func<CCContactsCorrespondence, bool>> expression)
        {
            return await _objectContext.CCContactsCorrespondences
                  .Where(expression)
              .ToListAsync();
        }

        public override void Add(CCContactsCorrespondence entity)
        {
            _objectContext.CCContactsCorrespondences.AddObject(entity);
        }

        public override void Delete(CCContactsCorrespondence entity)
        {
            _objectContext.CCContactsCorrespondences.DeleteObject(entity);
        }
    }
}
