using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IInvoicesRepository
    {
        Task<List<Invoice>> GetAllAsync();
        Task<List<Invoice>> GetAllAsync(Expression<Func<Invoice, bool>> expression);

        void Add(Invoice entity);
        void Delete(Invoice entity);
    }
}
