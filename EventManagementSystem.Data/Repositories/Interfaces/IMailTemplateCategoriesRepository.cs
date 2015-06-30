using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMailTemplateCategoriesRepository
    {
        Task<List<MailTemplateCategory>> GetAllAsync();

        void Add(MailTemplateCategory entity);
        void Delete(MailTemplateCategory entity);
    }
}
