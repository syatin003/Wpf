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

namespace EventManagementSystem.Data.Repositories
{
    public class MembershipGroupAgesRepository : EntitiesRepository<MembershipGroupAge>, IMembershipGroupAgesRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipGroupAgesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipGroupAge>> GetAllAsync()
        {
            return await _objectContext.MembershipGroupAges.ToListAsync();
        }

        public async Task<List<MembershipGroupAge>> GetAllAsync(Expression<Func<MembershipGroupAge, bool>> expression)
        {
            return await _objectContext.MembershipGroupAges
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipGroupAge entity)
        {
            _objectContext.MembershipGroupAges.AddObject(entity);
        }

        public override void Delete(MembershipGroupAge entity)
        {
            _objectContext.MembershipGroupAges.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipGroupAges);
        }
    }
}