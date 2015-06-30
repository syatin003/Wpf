using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IUserJobTypesRepository
    {
        Task<List<UserJobType>> GetAllAsync();

        void Add(UserJobType entity);
        void Delete(UserJobType entity);
    }
}
