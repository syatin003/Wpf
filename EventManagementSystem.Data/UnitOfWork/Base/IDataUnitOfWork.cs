using System;
using System.ComponentModel;
using EventManagementSystem.Data.Repositories.Base;

namespace EventManagementSystem.Data.UnitOfWork.Base
{
    public interface IDataUnitOfWork : IDisposable, IUnitOfWork
    {
        /// <summary>
        /// Register repository type.
        /// </summary>
        /// <typeparam name="TEntity">Entity type or interface.</typeparam>
        /// <typeparam name="TRepository">Repository for specified Entities type.</typeparam>
        void RegisterRepository<TEntity, TRepository>()
            where TEntity : INotifyPropertyChanged
            where TRepository : class, IDataRepository<TEntity>;

        /// <summary>
        /// Register repository instance.
        /// </summary>
        /// <typeparam name="TEntity">Entity type or interface.</typeparam>
        /// <typeparam name="TRepository">Repository for specified Entities type.</typeparam>
        /// <param name="repository">Instance</param>
        void RegisterRepository<TEntity, TRepository>(TRepository repository)
            where TEntity : INotifyPropertyChanged
            where TRepository : class, IDataRepository<TEntity>;

        /// <summary>
        /// Get repository of a specified Entity type.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Repository for specified entity type.</returns>
        IDataRepository<TEntity> GetRepository<TEntity>() where TEntity : INotifyPropertyChanged;
    }
}
