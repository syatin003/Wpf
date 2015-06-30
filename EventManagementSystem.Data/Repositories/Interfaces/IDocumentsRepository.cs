using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IDocumentsRepository
    {
        Task<List<Document>> GetAllAsync();
        Task<List<Document>> GetAllAsync(Expression<Func<Document, bool>> expression);

        void Add(Document entity);
        void Delete(Document entity);
        void Refresh();
    }
}
