using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMailTemplatesRepository
    {
        Task<List<MailTemplate>> GetAllAsync();
        Task<List<MailTemplate>> GetAllAsync(Expression<Func<MailTemplate, bool>> expression);

        Task<MailTemplate> GetUpdatedMailTemplate(Guid mailTemplateId);

        void Add(MailTemplate entity);
        void Delete(MailTemplate entity);
        void Refresh();
    }
}
