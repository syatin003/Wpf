using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IProductVatRatesRepository
    {
        Task<List<ProductVATRate>> GetAllAsync();

        void Add(ProductVATRate entity);
        void Delete(ProductVATRate entity);
         void Refresh(System.Data.Entity.Core.Objects.RefreshMode refreshMode);
    }
}
