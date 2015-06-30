using System.ComponentModel;

namespace EventManagementSystem.Data.Repositories.Base
{
    public interface IDataRepository<in T> : IRepository where T : INotifyPropertyChanged
    {
        /// <summary>
        /// Add new entity to repository.
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Delete entity from repository.
        /// </summary>
        /// <param name="entity">Entity which will be deleted.</param>
        void Delete(T entity);

    }
}
