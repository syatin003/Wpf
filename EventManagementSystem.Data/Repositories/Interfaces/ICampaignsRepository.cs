using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ICampaignsRepository
    {
        Task<List<Campaign>> GetAllAsync();
        Task<List<Campaign>> GetAllAsync(Expression<Func<Campaign, bool>> expression);

        Task<Campaign> GetUpdatedCampaign(Guid campaignId);

        void Add(Campaign entity);
        void Delete(Campaign entity);
        void Refresh();
    }
}
