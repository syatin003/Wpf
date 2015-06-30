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
   public class MembershipTokensRepository : EntitiesRepository<MembershipToken>, IMembershipTokensRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipTokensRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipToken>> GetAllAsync()
        {
            return await _objectContext.MembershipTokens.ToListAsync();
        }

        public async Task<List<MembershipToken>> GetAllAsync(Expression<Func<MembershipToken, bool>> expression)
        {
            return await _objectContext.MembershipTokens
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipToken entity)
        {
            _objectContext.MembershipTokens.AddObject(entity);
        }

        public override void Delete(MembershipToken entity)
        {
            _objectContext.MembershipTokens.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipTokens);
        }
    }
}
