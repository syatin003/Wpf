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
    public class MembershipCategoriesRepository : EntitiesRepository<MembershipCategory>, IMembershipCategoriesRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipCategoriesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipCategory>> GetAllAsync()
        {
            return await _objectContext.MembershipCategories.ToListAsync();

        }

        public async Task<List<MembershipCategory>> GetAllCategoriesWithItsTabsDataAsync()
        {
            return await _objectContext.MembershipCategories
                 .Include("MembershipGroupStyle")
                 .Include("MembershipGroupAge")
                 .Include("MembershipGroup")
                 .Include("MembershipGroup.MembershipGroupEPOS")
                 .Include("MembershipCategoryGroupDefault")
                 .Include("MembershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS")
                 .Include("MembershipFullPaymentComponent")
                 .Include("MembershipMonthlyPaymentUpFrontCost")
                 .Include("MembershipMonthlyPaymentOngoingCost")
                 .Include("MembershipFullPaymentComponent.Product")
                 .Include("MembershipFullPaymentComponent.Product1")
                 .Include("MembershipFullPaymentComponent.Product2")
                 .Include("MembershipFullPaymentComponent.Product3")
                 .Include("MembershipFullPaymentComponent.Product4")
                 .Include("MembershipFullPaymentComponent.Product5")
                 .Include("MembershipFullPaymentComponent.Product6")
                 .Include("MembershipFullPaymentComponent.Product7")
                 .Include("MembershipMonthlyPaymentUpFrontCost.Product")
                 .Include("MembershipMonthlyPaymentUpFrontCost.Product1")
                 .Include("MembershipMonthlyPaymentUpFrontCost.Product2")
                 .Include("MembershipMonthlyPaymentUpFrontCost.Product3")
                 .Include("MembershipMonthlyPaymentUpFrontCost.Product4")
                 .Include("MembershipMonthlyPaymentUpFrontCost.Product5")
                 .Include("MembershipMonthlyPaymentUpFrontCost.Product6")
                 .Include("MembershipMonthlyPaymentUpFrontCost.Product7")
                 .Include("MembershipMonthlyPaymentOngoingCost.Product")
                 .Include("MembershipMonthlyPaymentOngoingCost.Product1")
                 .Include("MembershipMonthlyPaymentOngoingCost.Product2")
                 .Include("MembershipMonthlyPaymentOngoingCost.Product3")
                 .Include("MembershipLinkTypes")
                 .Include("MembershipLinkTypes1")
                 .ToListAsync();
        }

        public async Task<List<MembershipCategory>> GetAllAsync(Expression<Func<MembershipCategory, bool>> expression)
        {
            return await _objectContext.MembershipCategories
                .Where(expression).ToListAsync();
        }

        public override void Add(MembershipCategory entity)
        {
            _objectContext.MembershipCategories.AddObject(entity);
        }

        public override void Delete(MembershipCategory entity)
        {
            _objectContext.MembershipCategories.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipCategories);
        }

        public void RefreshCategoryGroupsWithItsTabs()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipCategories);

            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipCategoryGroupDefaults);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipCategoryGroupDefaultEPOS);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipFullPaymentComponents);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipMonthlyPaymentUpFrontCosts);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipMonthlyPaymentOngoingCosts);
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipLinkTypes);

        }
    }
}