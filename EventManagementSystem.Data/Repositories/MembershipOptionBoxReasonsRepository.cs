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
    public class MembershipOptionBoxReasonsRepository : EntitiesRepository<MembershipOptionBoxReason>, IMembershipOptionBoxReasonsRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipOptionBoxReasonsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipOptionBoxReason>> GetAllAsync()
        {
            return await _objectContext.MembershipOptionBoxReasons.ToListAsync();
        }

        public async Task<List<MembershipOptionBoxReason>> GetAllAsync(Expression<Func<MembershipOptionBoxReason, bool>> expression)
        {
            return await _objectContext.MembershipOptionBoxReasons
                .Include("MembershipOptionBox")
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(MembershipOptionBoxReason entity)
        {
            _objectContext.MembershipOptionBoxReasons.AddObject(entity);
        }

        public override void Delete(MembershipOptionBoxReason entity)
        {
            _objectContext.MembershipOptionBoxReasons.DeleteObject(entity);
        }

        public void Delete(IEnumerable<MembershipOptionBoxReason> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipOptionBoxReasons);
        }
    }
}
