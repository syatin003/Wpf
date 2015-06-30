using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEnquiryStatusesRepository
    {
        Task<List<EnquiryStatus>> GetAllAsync();
        Task<List<EnquiryStatus>> GetAllAsync(Expression<Func<EnquiryStatus, bool>> expression);

        Task<EnquiryStatus> GetUpdatedEnquiryStatus(Guid enquiryStatusId);

        void Add(EnquiryStatus entity);
        void Delete(EnquiryStatus entity);
        void Refresh(EnquiryStatus entity);
    }
}
