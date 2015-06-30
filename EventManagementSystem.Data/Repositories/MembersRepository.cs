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
    public class MembersRepository : EntitiesRepository<Member>, IMembersRepository
    {
        private readonly EmsEntities _objectContext;

        public MembersRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _objectContext.Members
                .Include("Contact")
                .Include("Contact.ContactTitle")
                .Include("MembershipCategory")
                .Include("MembershipCategory.MembershipGroupAge")
                .Include("MembershipCategory.MembershipGroup")
                .Include("MembershipCategory.Members")
                .Include("MemberNotes")
                .Include("MemberNotes.User")
                .Include("MemberNotes.User1")
                .ToListAsync();
        }

        public async Task<List<Member>> GetAllAsync(Expression<Func<Member, bool>> expression)
        {
            return await _objectContext.Members
                         .Where(expression)
                         .ToListAsync();
        }

        public override void Add(Member entity)
        {
            _objectContext.Members.AddObject(entity);
        }

        public override void Delete(Member entity)
        {
            _objectContext.Members.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.Members);
        }
    }
}
