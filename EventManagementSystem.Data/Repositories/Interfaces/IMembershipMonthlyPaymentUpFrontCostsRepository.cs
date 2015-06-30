using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipMonthlyPaymentUpFrontCostsRepository
    {
        Task<List<MembershipMonthlyPaymentUpFrontCost>> GetAllAsync();
        Task<List<MembershipMonthlyPaymentUpFrontCost>> GetAllAsync(Expression<Func<MembershipMonthlyPaymentUpFrontCost, bool>> expression);

        void Add(MembershipMonthlyPaymentUpFrontCost entity);
        void Delete(MembershipMonthlyPaymentUpFrontCost entity);

        void Delete(IEnumerable<MembershipMonthlyPaymentUpFrontCost> entities);

        void Refresh();
    }
}
