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
    public class MembershipFullPaymentCostsRepository : EntitiesRepository<MembershipFullPaymentComponent>, IMembershipFullPaymentCostsRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipFullPaymentCostsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipFullPaymentComponent>> GetAllAsync()
        {
            return await _objectContext.MembershipFullPaymentComponents.ToListAsync();
        }

        public async Task<List<MembershipFullPaymentComponent>> GetAllAsync(Expression<Func<MembershipFullPaymentComponent, bool>> expression)
        {
            return await _objectContext.MembershipFullPaymentComponents
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipFullPaymentComponent entity)
        {
            _objectContext.MembershipFullPaymentComponents.AddObject(entity);
        }

        public override void Delete(MembershipFullPaymentComponent entity)
        {
            _objectContext.MembershipFullPaymentComponents.DeleteObject(entity);
        }

        public void Delete(IEnumerable<MembershipFullPaymentComponent> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipFullPaymentComponents);
        }

    }
}