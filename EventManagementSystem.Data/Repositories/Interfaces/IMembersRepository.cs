using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembersRepository
    {
        Task<List<Member>> GetAllAsync(Expression<Func<Member, bool>> expression);
        Task<List<Member>> GetAllAsync();

        void Add(Member entity);
        void Delete(Member entity);
        void Refresh();
    }
}
