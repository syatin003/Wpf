using EventManagementSystem.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IEmailHeadersRepository
    {
        Task<List<EmailHeader>> GetAllAsync();
        Task<List<EmailHeader>> GetAllAsync(Expression<Func<EmailHeader, bool>> expression);

        void Add(EmailHeader entity);
        void Delete(EmailHeader entity);
        void Refresh();
    }
}
