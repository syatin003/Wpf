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
    public class MembershipGroupEPOSRepository : EntitiesRepository<MembershipGroupEPOS>, IMembershipGroupEPOSRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipGroupEPOSRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipGroupEPOS>> GetAllAsync()
        {
            return await _objectContext.MembershipGroupEPOS.ToListAsync();
        }

        public async Task<List<MembershipGroupEPOS>> GetAllAsync(Expression<Func<MembershipGroupEPOS, bool>> expression)
        {
            return await _objectContext.MembershipGroupEPOS
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipGroupEPOS entity)
        {
            _objectContext.MembershipGroupEPOS.AddObject(entity);
        }

        public override void Delete(MembershipGroupEPOS entity)
        {
            _objectContext.MembershipGroupEPOS.DeleteObject(entity);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipGroupEPOS);
        }
    }
}
