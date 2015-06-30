using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IUserDepartmentsRepository
    {
        Task<List<UserDepartment>> GetAllAsync();

        void Add(UserDepartment entity);
        void Delete(UserDepartment entity);
    }
}
