using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ICCContactsCorrespondenceRepository
    {
        Task<List<CCContactsCorrespondence>> GetAllAsync();
        Task<List<CCContactsCorrespondence>> GetAllAsync(Expression<Func<CCContactsCorrespondence, bool>> expression);

        void Add(CCContactsCorrespondence entity);
        void Delete(CCContactsCorrespondence entity);
    }
}
