using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace EventManagementSystem.Data.Repositories
{
    public class EmailHeadersRepository : EntitiesRepository<EmailHeader>, IEmailHeadersRepository
    {
        private readonly EmsEntities _objectContext;

        public EmailHeadersRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<EmailHeader>> GetAllAsync()
        {
            return await _objectContext.EmailHeaders
                .Include("MailTemplates")
                .ToListAsync();
        }

        public async Task<List<EmailHeader>> GetAllAsync(Expression<Func<EmailHeader, bool>> expression)
        {
            return await _objectContext.EmailHeaders
                         .Where(expression)
                         .ToListAsync();
        }

        public override void Add(EmailHeader entity)
        {
            _objectContext.EmailHeaders.AddObject(entity);
        }

        public override void Delete(EmailHeader entity)
        {
            _objectContext.EmailHeaders.DeleteObject(entity);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.EmailHeaders);
        }
    }
}
