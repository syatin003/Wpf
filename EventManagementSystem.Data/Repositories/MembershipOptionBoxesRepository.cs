using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class MembershipOptionBoxesRepository : EntitiesRepository<MembershipOptionBox>, IMembershipOptionBoxesRepository
    {
        private readonly EmsEntities _objectContext;

        public MembershipOptionBoxesRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<MembershipOptionBox>> GetAllAsync()
        {
            return await _objectContext.MembershipOptionBoxes.ToListAsync();
        }

        public override void Add(MembershipOptionBox entity)
        {
            _objectContext.MembershipOptionBoxes.AddObject(entity);
        }

        public override void Delete(MembershipOptionBox entity)
        {
            _objectContext.MembershipOptionBoxes.DeleteObject(entity);
        }
        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.MembershipOptionBoxes);
        }
    }
}
