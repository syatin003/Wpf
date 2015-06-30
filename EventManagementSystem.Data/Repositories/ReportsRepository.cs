using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class ReportsRepository : EntitiesRepository<Report>, IReportsRepository
    {
        private readonly EmsEntities _objectContext;

        public ReportsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Report>> GetAllAsync()
        {
            return await _objectContext.Reports.ToListAsync();
        }

        public async Task<List<Report>> GetAllAsync(Expression<Func<Report, bool>> expression)
        {
            return await _objectContext.Reports
                .Where(expression)
                .ToListAsync();
        }

        public override void Add(Report entity)
        {
            _objectContext.Reports.AddObject(entity);
        }

        public override void Delete(Report entity)
        {
            _objectContext.Reports.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Reports);
        }
    }
}
