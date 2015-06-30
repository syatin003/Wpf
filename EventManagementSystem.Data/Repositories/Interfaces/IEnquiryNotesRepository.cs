using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEnquiryNotesRepository
    {
        Task<List<EnquiryNote>> GetAllAsync();
        Task<List<EnquiryNote>> GetAllAsync(Expression<Func<EnquiryNote, bool>> expression);

        void Add(EnquiryNote entity);
        void Delete(EnquiryNote entity);
        void SetEntityModified(EnquiryNote entity);
    }
}
