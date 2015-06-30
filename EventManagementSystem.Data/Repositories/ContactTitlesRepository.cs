using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class ContactTitlesRepository : EntitiesRepository<ContactTitle>, IContactTitlesRepository
    {
        private readonly EmsEntities _objectContext;

        public ContactTitlesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<ContactTitle>> GetAllAsync()
        {
            return await _objectContext.ContactTitles.ToListAsync();
        }

        public override void Add(ContactTitle entity)
        {
            _objectContext.ContactTitles.AddObject(entity);
        }

        public override void Delete(ContactTitle entity)
        {
            _objectContext.ContactTitles.DeleteObject(entity);
        }

    }
}
