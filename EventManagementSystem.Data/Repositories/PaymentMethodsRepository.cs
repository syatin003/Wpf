using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;

namespace EventManagementSystem.Data.Repositories
{
    public class PaymentMethodsRepository : EntitiesRepository<PaymentMethod>, IPaymentMethodsRepository
    {
        private readonly EmsEntities _objectContext;

        public PaymentMethodsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<PaymentMethod>> GetAllAsync()
        {
            return await _objectContext.PaymentMethods.ToListAsync();
        }

        public override void Add(PaymentMethod entity)
        {
            _objectContext.PaymentMethods.AddObject(entity);
        }

        public override void Delete(PaymentMethod entity)
        {
            _objectContext.PaymentMethods.DeleteObject(entity);
        }
    }
}
