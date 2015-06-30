using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;


namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface IGolfFollowResourcesRepository
    {
        Task<List<GolfFollowResource>> GetAllAsync();
        Task<List<GolfFollowResource>> GetAllAsync(Expression<Func<GolfFollowResource, bool>> expression);

        void Add(GolfFollowResource entity);
        void Delete(GolfFollowResource entity);
        void Delete(IEnumerable<GolfFollowResource> entities);
        void Refresh();
    }
}

