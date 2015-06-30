using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class CampaignsRepository : EntitiesRepository<Campaign>, ICampaignsRepository
    {
        private readonly EmsEntities _objectContext;

        public CampaignsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Campaign>> GetAllAsync()
        {
            return await _objectContext.Campaigns.ToListAsync();
        }

        public async Task<List<Campaign>> GetAllAsync(Expression<Func<Campaign, bool>> expression)
        {
            return await _objectContext.Campaigns
                .Where(expression)
                .ToListAsync();
        }

        public async Task<Campaign> GetUpdatedCampaign(Guid campaignId)
        {
            var desiredCampaign = await _objectContext.Campaigns.FirstOrDefaultAsync(x => x.ID == campaignId);
            _objectContext.Refresh(RefreshMode.StoreWins, desiredCampaign);

            return desiredCampaign;
        }

        public override void Add(Campaign entity)
        {
            _objectContext.Campaigns.AddObject(entity);
        }

        public override void Delete(Campaign entity)
        {
            _objectContext.Campaigns.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Campaigns);
        }
    }
}