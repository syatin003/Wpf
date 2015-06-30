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
    public class MembershipCategoryGroupDefaultEPOSRepository : EntitiesRepository<MembershipCategoryGroupDefaultEPOS>, IMembershipCategoryGroupDefaultEPOSRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipCategoryGroupDefaultEPOSRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipCategoryGroupDefaultEPOS>> GetAllAsync()
        {
            return await _objectContext.MembershipCategoryGroupDefaultEPOS.ToListAsync();
        }

        public async Task<List<MembershipCategoryGroupDefaultEPOS>> GetAllAsync(Expression<Func<MembershipCategoryGroupDefaultEPOS, bool>> expression)
        {
            return await _objectContext.MembershipCategoryGroupDefaultEPOS
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipCategoryGroupDefaultEPOS entity)
        {
            _objectContext.MembershipCategoryGroupDefaultEPOS.AddObject(entity);
        }

        public override void Delete(MembershipCategoryGroupDefaultEPOS entity)
        {
            _objectContext.MembershipCategoryGroupDefaultEPOS.DeleteObject(entity);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipCategoryGroupDefaultEPOS);
        }
    }
}
