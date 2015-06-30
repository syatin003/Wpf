using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IDefaultSettingsForEnquiriesRepository
    {
        Task<List<DefaultSettingsForEnquiry>> GetAllAsync();

        void Add(DefaultSettingsForEnquiry entity);
        void Delete(DefaultSettingsForEnquiry entity);
        void Refresh();
    }
}
