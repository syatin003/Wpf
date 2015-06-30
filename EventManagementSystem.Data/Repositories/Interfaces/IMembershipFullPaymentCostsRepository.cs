using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipFullPaymentCostsRepository
    {
        Task<List<MembershipFullPaymentComponent>> GetAllAsync();
        Task<List<MembershipFullPaymentComponent>> GetAllAsync(Expression<Func<MembershipFullPaymentComponent, bool>> expression);

        void Add(MembershipFullPaymentComponent entity);
        void Delete(MembershipFullPaymentComponent entity);

        void Delete(IEnumerable<MembershipFullPaymentComponent> entities);

        void Refresh();
    }
}
