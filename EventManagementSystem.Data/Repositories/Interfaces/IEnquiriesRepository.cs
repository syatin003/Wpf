using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEnquiriesRepository
    {
        Task<List<Enquiry>> GetLightEnquiriesAsync();
        Task<List<Enquiry>> GetLightEnquiriesAsync(Expression<Func<Enquiry, bool>> expression);

        Task<Enquiry> GetUpdatedEnquiry(Guid enquiryId);

        void Add(Enquiry entity);
        void Delete(Enquiry entity);
        void Refresh();
        void RevertAllChanges();
    }
}
