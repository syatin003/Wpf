using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillTransactionProductsRepository
    {
        Task<List<TillTransactionProduct>> GetAllAsync();
        Task<List<TillTransactionProduct>> GetTransactionsWithRelatedData(DateTime startDate, DateTime endDate);

        void Add(TillTransactionProduct entity);
        void Delete(TillTransactionProduct entity);
    }
}
