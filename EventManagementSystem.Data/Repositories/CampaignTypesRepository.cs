using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class CampaignTypesRepository : EntitiesRepository<CampaignType>, ICampaignTypesRepository
    {
         private readonly EmsEntities _objectContext;

         public CampaignTypesRepository(EmsEntities objectContext)
             : base(objectContext)
        {
            _objectContext = objectContext;
        }

         public async Task<List<CampaignType>> GetAllAsync()
        {
            return await _objectContext.CampaignTypes.ToListAsync();
        }

        public override void Add(CampaignType entity)
        {
            _objectContext.CampaignTypes.AddObject(entity);
        }

        public override void Delete(CampaignType entity)
        {
            _objectContext.CampaignTypes.DeleteObject(entity);
        }
    }
}
