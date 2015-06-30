using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IReportsRepository
    {
        Task<List<Report>> GetAllAsync();
        Task<List<Report>> GetAllAsync(Expression<Func<Report, bool>> expression);

        void Add(Report entity);
        void Delete(Report entity);
        void Refresh();
    }
}
