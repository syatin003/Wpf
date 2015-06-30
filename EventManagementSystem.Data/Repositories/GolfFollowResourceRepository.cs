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
    public class GolfFollowResourceRepository : EntitiesRepository<GolfFollowResource>, IGolfFollowResourcesRepository
    {
        private readonly EmsEntities _objectContext;

        public GolfFollowResourceRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<GolfFollowResource>> GetAllAsync()
        {
            return await _objectContext.GolfFollowResources
                .ToListAsync();
        }

        public async Task<List<GolfFollowResource>> GetAllAsync(Expression<Func<GolfFollowResource, bool>> expression)
        {
            return await _objectContext.GolfFollowResources
                 .Where(expression)
                 .ToListAsync();
        }

        public override void Add(GolfFollowResource entity)
        {
            _objectContext.GolfFollowResources.AddObject(entity);
        }

        public override void Delete(GolfFollowResource entity)
        {
            _objectContext.GolfFollowResources.DeleteObject(entity);

        }

        public void Delete(IEnumerable<GolfFollowResource> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.GolfFollowResources);
        }
    }
}
