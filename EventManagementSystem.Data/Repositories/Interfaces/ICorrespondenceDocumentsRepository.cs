using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ICorrespondenceDocumentsRepository
    {
        Task<List<CorrespondenceDocument>> GetAllAsync();
        Task<List<CorrespondenceDocument>> GetAllAsync(Expression<Func<CorrespondenceDocument, bool>> expression);

        void Add(CorrespondenceDocument entity);
        void Delete(CorrespondenceDocument entity);
    }
}
