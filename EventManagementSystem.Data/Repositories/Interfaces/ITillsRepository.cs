using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillsRepository
    {
        Task<List<Till>> GetAllAsync();
        Task<List<Till>> GetAllAsync(Expression<Func<Till, bool>> expression);

        void Add(Till entity);
        void Delete(Till entity);
        void Refresh(Till entity);

    }
}
