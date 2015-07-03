using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ICorresponcencesRepository
    {
        Task<List<Corresponcence>> GetAllAsync();
        Task<List<Corresponcence>> GetAllAsync(Expression<Func<Corresponcence, bool>> expression);

        void Add(Corresponcence entity);
        void Delete(Corresponcence entity);
        void Delete(IEnumerable<Corresponcence> entities);
        void Refresh();
        void RevertChanges(bool isSaveOnClientRecord);
    }
}
