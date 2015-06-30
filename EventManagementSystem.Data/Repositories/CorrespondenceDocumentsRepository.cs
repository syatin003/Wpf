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
    class CorrespondenceDocumentsRepository : EntitiesRepository<CorrespondenceDocument>, ICorrespondenceDocumentsRepository
    {
        private readonly EmsEntities _objectContext;

        public CorrespondenceDocumentsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<CorrespondenceDocument>> GetAllAsync()
        {
            return await _objectContext.CorrespondenceDocuments.ToListAsync();
        }

        public async Task<List<CorrespondenceDocument>> GetAllAsync(Expression<Func<CorrespondenceDocument, bool>> expression)
        {
            return await _objectContext.CorrespondenceDocuments
               .Where(expression)
               .ToListAsync();
        }

        public override void Add(CorrespondenceDocument entity)
        {
            _objectContext.CorrespondenceDocuments.AddObject(entity);
        }

        public override void Delete(CorrespondenceDocument entity)
        {
            _objectContext.CorrespondenceDocuments.DeleteObject(entity);
        }
    }
}
