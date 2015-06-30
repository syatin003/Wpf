using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IPaymentMethodsRepository
    {
        Task<List<PaymentMethod>> GetAllAsync();

        void Add(PaymentMethod entity);
        void Delete(PaymentMethod entity);
    }
}
