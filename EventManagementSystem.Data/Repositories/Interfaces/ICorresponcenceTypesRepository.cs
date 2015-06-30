using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ICorresponcenceTypesRepository
    {
        Task<List<CorresponcenceType>> GetAllAsync();
        Task<List<CorresponcenceType>> GetAllAsync(Expression<Func<CorresponcenceType, bool>> expression);

        void Add(CorresponcenceType entity);
        void Delete(CorresponcenceType entity);
    }
}
