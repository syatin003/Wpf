using EventManagementSystem.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMemberNotesRepository
    {
        Task<List<MemberNote>> GetAllAsync();
        Task<List<MemberNote>> GetAllAsync(Expression<Func<MemberNote, bool>> expression);

        void Add(MemberNote entity);
        void Delete(MemberNote entity);
        void Delete(IEnumerable<MemberNote> entities);
        void Refresh();
    }
}
