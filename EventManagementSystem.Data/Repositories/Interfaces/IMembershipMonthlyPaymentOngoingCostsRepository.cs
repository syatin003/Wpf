using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipMonthlyPaymentOngoingCostsRepository
    {
        Task<List<MembershipMonthlyPaymentOngoingCost>> GetAllAsync();
        Task<List<MembershipMonthlyPaymentOngoingCost>> GetAllAsync(Expression<Func<MembershipMonthlyPaymentOngoingCost, bool>> expression);

        void Add(MembershipMonthlyPaymentOngoingCost entity);
        void Delete(MembershipMonthlyPaymentOngoingCost entity);

        void Delete(IEnumerable<MembershipMonthlyPaymentOngoingCost> entities);

        void Refresh();
    }
}
