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
    public class MembershipMonthlyPaymentOngoingCostsRepository : EntitiesRepository<MembershipMonthlyPaymentOngoingCost>, IMembershipMonthlyPaymentOngoingCostsRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipMonthlyPaymentOngoingCostsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipMonthlyPaymentOngoingCost>> GetAllAsync()
        {
            return await _objectContext.MembershipMonthlyPaymentOngoingCosts.ToListAsync();
        }

        public async Task<List<MembershipMonthlyPaymentOngoingCost>> GetAllAsync(Expression<Func<MembershipMonthlyPaymentOngoingCost, bool>> expression)
        {
            return await _objectContext.MembershipMonthlyPaymentOngoingCosts
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipMonthlyPaymentOngoingCost entity)
        {
            _objectContext.MembershipMonthlyPaymentOngoingCosts.AddObject(entity);
        }

        public override void Delete(MembershipMonthlyPaymentOngoingCost entity)
        {
            _objectContext.MembershipMonthlyPaymentOngoingCosts.DeleteObject(entity);
        }
        public void Delete(IEnumerable<MembershipMonthlyPaymentOngoingCost> entities)
        {
            entities.ForEach(Delete);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipMonthlyPaymentOngoingCosts);
        }
    }
}