using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IFacilitiesRepository
    {
        Task<List<Facility>> GetAllAsync();

        void Add(Facility entity);
        void Delete(Facility entity);
    }
}
