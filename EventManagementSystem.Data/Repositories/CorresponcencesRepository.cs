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
    public class CorresponcencesRepository : EntitiesRepository<Corresponcence>, ICorresponcencesRepository
    {
        private readonly EmsEntities _objectContext;

        public CorresponcencesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Corresponcence>> GetAllAsync()
        {
            return await _objectContext.Corresponcences
                .Include("CorresponcenceType")
                .Include("User")
                .Include("Contact")
                .Include("CorrespondenceDocuments")
                .ToListAsync();
        }

        public async Task<List<Corresponcence>> GetAllAsync(Expression<Func<Corresponcence, bool>> expression)
        {
            return await _objectContext.Corresponcences
                .Where(expression)
                .Include("CorresponcenceType")
                .Include("User")
                .Include("Contact")
                .Include("CorrespondenceDocuments")
                .ToListAsync();
        }

        public override void Add(Corresponcence entity)
        {
            _objectContext.Corresponcences.AddObject(entity);
        }

        public override void Delete(Corresponcence entity)
        {
            _objectContext.Corresponcences.DeleteObject(entity);
        }

        public void Delete(IEnumerable<Corresponcence> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Corresponcences);
        }

        public void RevertChanges(bool isSaveOnClientRecord)
        {
            _objectContext.DetectChanges();

            IEnumerable<object> ModifiedandDeletedcollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Deleted).Select(x => x.Entity).ToList();
            _objectContext.Refresh(RefreshMode.StoreWins, ModifiedandDeletedcollection);


            IEnumerable<object> AddedCollection = _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Select(x => x.Entity).ToList();

           // AddedCollection.ForEach(addedEntity => _objectContext.Detach(addedEntity));
   
            foreach(var addedEntity in AddedCollection)
            {
               if(addedEntity.GetType().Name == "Corresponcence")
               {
                   Corresponcence entity = (Corresponcence)addedEntity;
                   if(entity.OwnerID == Guid.Empty)
                   {
                     _objectContext.Detach(addedEntity);
                   }
                   else if (!isSaveOnClientRecord)
                   {
                       _objectContext.Detach(addedEntity);
                   }
               }
            }
        }
    }
}