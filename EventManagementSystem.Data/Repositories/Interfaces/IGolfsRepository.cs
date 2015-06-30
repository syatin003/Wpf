using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IGolfsRepository
    {
        Task<List<Golf>> GetAllAsync();

        void Add(Golf entity);
        void Delete(Golf entity);
        void Refresh();
        void Refresh(RefreshMode mode);
        void Refresh(Golf entity);
    }
}
