using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IMembershipOptionBoxesRepository
    {
        Task<List<MembershipOptionBox>> GetAllAsync();

        void Add(MembershipOptionBox entity);
        void Delete(MembershipOptionBox entity);
        void Refresh();
    }
}
