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
    public class MembershipGroupStylesRepository : EntitiesRepository<MembershipGroupStyle>, IMembershipGroupStylesRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipGroupStylesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipGroupStyle>> GetAllAsync()
        {
            return await _objectContext.MembershipGroupStyles.ToListAsync();
        }

        public async Task<List<MembershipGroupStyle>> GetAllAsync(Expression<Func<MembershipGroupStyle, bool>> expression)
        {
            return await _objectContext.MembershipGroupStyles
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipGroupStyle entity)
        {
            _objectContext.MembershipGroupStyles.AddObject(entity);
        }

        public override void Delete(MembershipGroupStyle entity)
        {
            _objectContext.MembershipGroupStyles.DeleteObject(entity);
        }


        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipGroupStyles);
        }
    }
}