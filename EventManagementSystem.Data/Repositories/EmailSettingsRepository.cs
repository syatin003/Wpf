using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class EmailSettingsRepository : EntitiesRepository<EmailSetting>, IEmailSettingsRepository
    {
        private readonly EmsEntities _objectContext;

        public EmailSettingsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EmailSetting>> GetAllAsync()
        {
            return await _objectContext.EmailSettings.ToListAsync();
        }

        public override void Add(EmailSetting entity)
        {
            _objectContext.EmailSettings.AddObject(entity);
        }

        public override void Delete(EmailSetting entity)
        {
            _objectContext.EmailSettings.DeleteObject(entity);
        }
    }
}
