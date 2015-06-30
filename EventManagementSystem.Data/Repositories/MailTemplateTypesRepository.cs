using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class MailTemplateTypesRepository : EntitiesRepository<MailTemplateType>, IMailTemplateTypesRepository
    {
        private readonly EmsEntities _objectContext;

        public MailTemplateTypesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MailTemplateType>> GetAllAsync()
        {
            return await _objectContext.MailTemplateTypes.ToListAsync();
        }

        public override void Add(MailTemplateType entity)
        {
            _objectContext.MailTemplateTypes.AddObject(entity);
        }

        public override void Delete(MailTemplateType entity)
        {
            _objectContext.MailTemplateTypes.DeleteObject(entity);
        }
    }
}
