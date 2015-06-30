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
    public class MembershipGroupsRepository : EntitiesRepository<MembershipGroup>, IMembershipGroupsRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipGroupsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipGroup>> GetAllAsync()
        {
            return await _objectContext.MembershipGroups
                .Include("MembershipGroupEPOS")
                .ToListAsync();
        }

        public async Task<List<MembershipGroup>> GetAllAsync(Expression<Func<MembershipGroup, bool>> expression)
        {
            return await _objectContext.MembershipGroups
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipGroup entity)
        {
            _objectContext.MembershipGroups.AddObject(entity);
        }

        public override void Delete(MembershipGroup entity)
        {
            _objectContext.MembershipGroups.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipGroups);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipGroupEPOS);
        }
    }
}