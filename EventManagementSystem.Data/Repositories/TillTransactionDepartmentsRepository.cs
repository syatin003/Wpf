using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class TillTransactionDepartmentsRepository : EntitiesRepository<TillTransactionDepartment>,
        ITillTransactionDepartmentsRepository
    {
        private readonly EmsEntities _objectContext;

        public TillTransactionDepartmentsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<TillTransactionDepartment>> GetAllAsync()
        {
            return await _objectContext.TillTransactionDepartments.ToListAsync();
        }

        public override void Add(TillTransactionDepartment entity)
        {
            _objectContext.TillTransactionDepartments.AddObject(entity);
        }

        public override void Delete(TillTransactionDepartment entity)
        {
            _objectContext.TillTransactionDepartments.DeleteObject(entity);
        }
    }
}