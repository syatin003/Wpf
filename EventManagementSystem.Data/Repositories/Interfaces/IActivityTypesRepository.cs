using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
   public interface IActivityTypesRepository
   {
       Task<List<ActivityType>> GetAllAsync();

        void Add(ActivityType entity);
        void Delete(ActivityType entity);
    }
}
