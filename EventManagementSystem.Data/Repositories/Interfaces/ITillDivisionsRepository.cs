using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillDivisionsRepository
    {

        Task<List<TillDivision>> GetAllAsync();

        void Add(TillDivision entity);
        void Delete(TillDivision entity);
    }
}
