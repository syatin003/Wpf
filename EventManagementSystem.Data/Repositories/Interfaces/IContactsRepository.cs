using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IContactsRepository
    {
        Task<List<Contact>> GetAllAsync(Expression<Func<Contact, bool>> expression);
        Task<List<Contact>> GetAllAsyncWithoutRefresh(Expression<Func<Contact, bool>> expression);
        Task<List<Contact>> GetAllAsync();

        void Add(Contact entity);
        void Delete(Contact entity);
        void Refresh();
        void Refresh(Contact entity);
        void RefreshContact();

    }
}
