using EventManagementSystem.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.Model;
using System.Data.Entity.Core.Objects;

namespace EventManagementSystem.Data.Repositories
{
    public class MailTemplateImagesRepository : EntitiesRepository<MailTemplateImage>, IMailTemplateImagesRepository
    {
         private readonly EmsEntities _objectContext;

         public MailTemplateImagesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

         //public async Task<List<MailTemplate>> GetAllAsync()
         //{
         //    return await _objectContext.MailTemplates.ToListAsync();
         //}

       
        //public async Task<MailTemplateImage> GetUpdatedMailTemplate(Guid mailTemplateId)
        //{
        //    var desiredMailTemplate = await _objectContext.MailTemplates.FirstOrDefaultAsync(x => x.ID == mailTemplateId);
        //    _objectContext.Refresh(RefreshMode.StoreWins, desiredMailTemplate);

        //    return desiredMailTemplate;
        //}

        public override void Add(MailTemplateImage entity)
        {
            _objectContext.MailTemplateImages.AddObject(entity);
        }

        public override void Delete(MailTemplateImage entity)
        {
            _objectContext.MailTemplateImages.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MailTemplates);
        }



    }
}
