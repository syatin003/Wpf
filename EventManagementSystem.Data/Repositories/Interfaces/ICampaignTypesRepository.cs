using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ICampaignTypesRepository
    {
        Task<List<CampaignType>> GetAllAsync();

        void Add(CampaignType entity);
        void Delete(CampaignType entity);
    }
}
