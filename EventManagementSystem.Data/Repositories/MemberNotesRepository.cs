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
using Microsoft.Practices.ObjectBuilder2;

namespace EventManagementSystem.Data.Repositories
{
    public class MemberNotesRepository : EntitiesRepository<MemberNote>, IMemberNotesRepository
    {
        private readonly EmsEntities _objectContext;

        public MemberNotesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MemberNote>> GetAllAsync()
        {
            return await _objectContext.MemberNotes
                .Include("User")
                .Include("User1")
                .ToListAsync();
        }

        public async Task<List<MemberNote>> GetAllAsync(Expression<Func<MemberNote, bool>> expression)
        {
            return await _objectContext.MemberNotes
                .Where(expression)
                .Include("User")
                .Include("User1")
                .ToListAsync();
        }

        public override void Add(MemberNote entity)
        {
            _objectContext.MemberNotes.AddObject(entity);
        }

        public override void Delete(MemberNote entity)
        {
            _objectContext.MemberNotes.DeleteObject(entity);
        }

        public void Delete(IEnumerable<MemberNote> entities)
        {
            entities.ForEach(Delete);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MemberNotes);
        }
    }
}
