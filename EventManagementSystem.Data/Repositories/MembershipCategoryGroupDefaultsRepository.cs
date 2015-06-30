using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Data.Entity.Core.Objects;
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class MembershipCategoryGroupDefaultsRepository : EntitiesRepository<MembershipCategoryGroupDefault>, IMembershipCategoryGroupDefaultsRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipCategoryGroupDefaultsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipCategoryGroupDefault>> GetAllAsync()
        {
            return await _objectContext.MembershipCategoryGroupDefaults
                .Include("MembershipCategoryGroupDefaultEPOS")
                .ToListAsync();
        }

        public async Task<List<MembershipCategoryGroupDefault>> GetAllAsync(Expression<Func<MembershipCategoryGroupDefault, bool>> expression)
        {
            return await _objectContext.MembershipCategoryGroupDefaults
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipCategoryGroupDefault entity)
        {
            _objectContext.MembershipCategoryGroupDefaults.AddObject(entity);
        }

        public override void Delete(MembershipCategoryGroupDefault entity)
        {
            _objectContext.MembershipCategoryGroupDefaults.DeleteObject(entity);
        }

        public void Delete(IEnumerable<MembershipCategoryGroupDefault> entities)
        {
            entities.ForEach(Delete);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipCategoryGroupDefaults);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipCategoryGroupDefaultEPOS);
        }
    }
}