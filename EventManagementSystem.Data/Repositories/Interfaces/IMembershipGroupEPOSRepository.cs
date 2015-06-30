using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipGroupEPOSRepository
    {
        Task<List<MembershipGroupEPOS>> GetAllAsync();
        Task<List<MembershipGroupEPOS>> GetAllAsync(Expression<Func<MembershipGroupEPOS, bool>> expression);

        void Add(MembershipGroupEPOS entity);
        void Delete(MembershipGroupEPOS entity);

        void Refresh();
    }
}
