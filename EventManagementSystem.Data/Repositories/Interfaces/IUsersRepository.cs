using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUserAsync(string username, string password);

        Task<List<User>> GetUsersAsync();
        Task<List<User>> GetUsersAsync(Expression<Func<User, bool>> expression);
        Task<User> GetUserAsync(Guid userID);
        void Add(User entity);
        void Delete(User entity);
        void Delete(IEnumerable<User> entities);
        Task Refresh();
        void Refresh(System.Data.Entity.Core.Objects.RefreshMode mode);
        Task Refresh(User user);
        void RevertAllChanges();

    }
}
