using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMailTemplateImagesRepository
    {
        //Task<List<MailTemplateImagesRepository>> GetAllAsync();
        //Task<MailTemplateImagesRepository> GetUpdatedMailTemplate(Guid mailTemplateId);
        void Add(MailTemplateImage entity);
        void Delete(MailTemplateImage entity);
        void Refresh();
    }
}
