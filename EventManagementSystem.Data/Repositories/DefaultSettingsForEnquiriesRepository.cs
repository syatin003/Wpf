using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class DefaultSettingsForEnquiriesRepository : EntitiesRepository<DefaultSettingsForEnquiry>, IDefaultSettingsForEnquiriesRepository
    {
        private readonly EmsEntities _objectContext;

        public DefaultSettingsForEnquiriesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<DefaultSettingsForEnquiry>> GetAllAsync()
        {
            return await _objectContext.DefaultSettingsForEnquiries.ToListAsync();
        }

        public override void Add(DefaultSettingsForEnquiry entity)
        {
            _objectContext.DefaultSettingsForEnquiries.AddObject(entity);
        }

        public override void Delete(DefaultSettingsForEnquiry entity)
        {
            _objectContext.DefaultSettingsForEnquiries.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.DefaultSettingsForEnquiries);
        }
    }
}
