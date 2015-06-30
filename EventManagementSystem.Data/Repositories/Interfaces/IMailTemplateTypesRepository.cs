using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMailTemplateTypesRepository
    {
        Task<List<MailTemplateType>> GetAllAsync();

        void Add(MailTemplateType entity);
        void Delete(MailTemplateType entity);
    }
}
