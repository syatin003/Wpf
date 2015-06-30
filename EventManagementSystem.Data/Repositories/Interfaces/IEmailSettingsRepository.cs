using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEmailSettingsRepository
    {
        Task<List<EmailSetting>> GetAllAsync();

        void Add(EmailSetting entity);
        void Delete(EmailSetting entity);
    }
}
