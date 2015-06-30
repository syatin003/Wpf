using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEnquiryReceiveMethodsRepository
    {
        Task<List<EnquiryReceiveMethod>> GetAllAsync();

        void Add(EnquiryReceiveMethod entity);
        void Delete(EnquiryReceiveMethod entity);
    }
}
