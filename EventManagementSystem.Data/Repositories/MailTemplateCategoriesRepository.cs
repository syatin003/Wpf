using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class MailTemplateCategoriesRepository : EntitiesRepository<MailTemplateCategory>, IMailTemplateCategoriesRepository
    {
        private readonly EmsEntities _objectContext;

        public MailTemplateCategoriesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MailTemplateCategory>> GetAllAsync()
        {
            return await _objectContext.MailTemplateCategories.ToListAsync();
        }

        public override void Add(MailTemplateCategory entity)
        {
            _objectContext.MailTemplateCategories.AddObject(entity);
        }

        public override void Delete(MailTemplateCategory entity)
        {
            _objectContext.MailTemplateCategories.DeleteObject(entity);
        }
    }
}
