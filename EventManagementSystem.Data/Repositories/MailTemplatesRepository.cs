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
    public class MailTemplatesRepository : EntitiesRepository<MailTemplate>, IMailTemplatesRepository
    {
        private readonly EmsEntities _objectContext;

        public MailTemplatesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MailTemplate>> GetAllAsync()
        {
            return await _objectContext.MailTemplates
                .Include("MailTemplateCategory")
                .Include("MailTemplateType")
                .Include("User")
                .Include("EmailHeader")
                .ToListAsync();
        }

        public async Task<List<MailTemplate>> GetAllAsync(Expression<Func<MailTemplate, bool>> expression)
        {
            return await _objectContext.MailTemplates
                .Where(expression)
                .Include("MailTemplateCategory")
                .Include("MailTemplateType")
                .Include("User")
                .ToListAsync();
        }

        public async Task<MailTemplate> GetUpdatedMailTemplate(Guid mailTemplateId)
        {
            var desiredMailTemplate = await _objectContext.MailTemplates
                .Include("MailTemplateCategory")
                .Include("MailTemplateType")
                .Include("User")
                .FirstOrDefaultAsync(x => x.ID == mailTemplateId);
            _objectContext.Refresh(RefreshMode.StoreWins, desiredMailTemplate);

            return desiredMailTemplate;
        }

        public override void Add(MailTemplate entity)
        {
            _objectContext.MailTemplates.AddObject(entity);
        }

        public override void Delete(MailTemplate entity)
        {
            _objectContext.MailTemplates.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MailTemplates);
        }
    }
}
