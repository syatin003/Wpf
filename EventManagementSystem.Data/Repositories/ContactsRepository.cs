using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Data.Entity.Core.Objects;


namespace EventManagementSystem.Data.Repositories
{
    public class ContactsRepository : EntitiesRepository<Contact>, IContactsRepository
    {
        private readonly EmsEntities _objectContext;

        public ContactsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Contact>> GetAllAsync()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);
            return await _objectContext.Contacts
                .Include(contact => contact.ContactTitle)
                .Include(contact => contact.Member)
                .Include(contact => contact.Member.MembershipCategory).ToListAsync();
        }
        public async Task<List<Contact>> GetAllAsyncWithoutRefresh(Expression<Func<Contact, bool>> expression)
        {
            return await _objectContext.Contacts
                              .Include(contact => contact.ContactTitle)
                              .Include(contact => contact.Member)
                              .Include(contact => contact.Member.MembershipCategory)
                              .Where(expression)
                              .ToListAsync();
        }

        public async Task<List<Contact>> GetAllAsync(Expression<Func<Contact, bool>> expression)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);
            return await _objectContext.Contacts
                                .Include(contact => contact.ContactTitle)
                                .Include(contact => contact.Member)
                                .Include(contact => contact.Member.MembershipCategory)
                                .Where(expression)
                                .ToListAsync();
        }

        public override void Add(Contact entity)
        {
            _objectContext.Contacts.AddObject(entity);
        }

        public override void Delete(Contact entity)
        {
            _objectContext.Contacts.DeleteObject(entity);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.ClientWins, _objectContext.Activities);
        }

        public void Refresh(Contact entity)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, entity);
        }
        public void RefreshContact()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Contacts);
        }
    }
}
