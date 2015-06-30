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
    public class MembershipLinkTypesRepository : EntitiesRepository<MembershipLinkType>, IMembershipLinkTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipLinkTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipLinkType>> GetAllAsync()
        {
            return await _objectContext.MembershipLinkTypes
                .Include("MembershipCategory")
                .Include("MembershipCategory1")
                .ToListAsync();
        }

        public async Task<List<MembershipLinkType>> GetAllAsync(Expression<Func<MembershipLinkType, bool>> expression)
        {
            return await _objectContext.MembershipLinkTypes
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipLinkType entity)
        {
            _objectContext.MembershipLinkTypes.AddObject(entity);
        }

        public override void Delete(MembershipLinkType entity)
        {
            _objectContext.MembershipLinkTypes.DeleteObject(entity);
        }

        public void Delete(IEnumerable<MembershipLinkType> entities)
        {
            entities.ForEach(Delete);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipLinkTypes);
        }
    }
}
