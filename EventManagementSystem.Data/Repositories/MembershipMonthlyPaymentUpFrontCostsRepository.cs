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
    public class MembershipMonthlyPaymentUpFrontCostsRepository : EntitiesRepository<MembershipMonthlyPaymentUpFrontCost>, IMembershipMonthlyPaymentUpFrontCostsRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipMonthlyPaymentUpFrontCostsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipMonthlyPaymentUpFrontCost>> GetAllAsync()
        {
            return await _objectContext.MembershipMonthlyPaymentUpFrontCosts.ToListAsync();
        }

        public async Task<List<MembershipMonthlyPaymentUpFrontCost>> GetAllAsync(Expression<Func<MembershipMonthlyPaymentUpFrontCost, bool>> expression)
        {
            return await _objectContext.MembershipMonthlyPaymentUpFrontCosts
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipMonthlyPaymentUpFrontCost entity)
        {
            _objectContext.MembershipMonthlyPaymentUpFrontCosts.AddObject(entity);
        }

        public override void Delete(MembershipMonthlyPaymentUpFrontCost entity)
        {
            _objectContext.MembershipMonthlyPaymentUpFrontCosts.DeleteObject(entity);
        }

        public void Delete(IEnumerable<MembershipMonthlyPaymentUpFrontCost> entities)
        {
            entities.ForEach(Delete);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipMonthlyPaymentUpFrontCosts);
        }
    }
}