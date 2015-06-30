using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEnquiryUpdatesRepository
    {
        Task<List<EnquiryUpdate>> GetAllAsync();
        Task<List<EnquiryUpdate>> GetAllAsync(Expression<Func<EnquiryUpdate, bool>> expression);

        void Add(EnquiryUpdate entity);
        void Delete(EnquiryUpdate entity);
    }
}
