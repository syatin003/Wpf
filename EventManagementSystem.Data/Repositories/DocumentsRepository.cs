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
    public class DocumentsRepository : EntitiesRepository<Document>, IDocumentsRepository
    {
        private readonly EmsEntities _objectContext;

        public DocumentsRepository(EmsEntities objectContext) : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Document>> GetAllAsync()
        {
            return await _objectContext.Documents
                .Include("CorrespondenceDocuments")
                .ToListAsync();
        }

        public async Task<List<Document>> GetAllAsync(Expression<Func<Document, bool>> expression)
        {
            return await _objectContext.Documents
                .Include("CorrespondenceDocuments")
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(Document entity)
        {
            _objectContext.Documents.AddObject(entity);
        }

        public override void Delete(Document entity)
        {
            _objectContext.Documents.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Documents.Include("CorrespondenceDocuments"));
        }
    }
}