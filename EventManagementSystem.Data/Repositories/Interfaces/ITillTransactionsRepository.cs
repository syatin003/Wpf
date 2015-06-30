using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ITillTransactionsRepository
    {
        Task<List<TillTransaction>> GetAllAsync();
        Task<List<TillTransaction>> GetTransactionsWithRelatedData(DateTime startDate, DateTime endDate);

        void Add(TillTransaction entity);
        void Delete(TillTransaction entity);
    }
}
