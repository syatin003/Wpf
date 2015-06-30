using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using EventManagementSystem.Data.Repositories.Base;

namespace EventManagementSystem.Data.UnitOfWork.Base
{
    public abstract class DataUnitOfWork : IDataUnitOfWork
    {
        #region Fields

        private const string EX_ENTITY_TYPE_ALREADY_REGISTERED = "Repository for entity type:{0}, already registered.";
        protected const string EX_REPOSITORY_DOES_NOT_EXISTS = "Repository for entity type:{0}, doesn't exists.";
        protected const string EX_OBJECT_IS_DISPOSED = "Object is disposed.";

        private IDictionary<Type, IRepository> _registeredInstances = new Dictionary<Type, IRepository>();
        private IDictionary<Type, Type> _registeredRepositories = new Dictionary<Type, Type>();
        private bool _isDisposed;

        #endregion

        #region Properties

        /// <summary>
        /// Registered repository Instances.
        /// </summary>
        protected IDictionary<Type, IRepository> RegisteredInstances
        {
            get
            {
                if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);
                return _registeredInstances;
            }
            private set
            {
                if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);
                _registeredInstances = value;
            }
        }

        /// <summary>
        /// Registered repository Types.
        /// </summary>
        protected IDictionary<Type, Type> RegisteredRepositories
        {
            get
            {
                if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);
                return _registeredRepositories;
            }
            private set
            {
                if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);
                _registeredRepositories = value;
            }
        }

        /// <summary>
        /// Returns all registered repositories instances.
        /// </summary>
        protected IEnumerable<IRepository> Repositories
        {
            get
            {
                if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);
                return RegisteredInstances.Values.Where(repository => repository != null);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save current changes.
        /// </summary>
        public abstract Task<int> SaveChanges();

        /// <summary>
        /// Discard all changes, back to the initial state.
        /// </summary>
        public abstract void RevertChanges();

        /// <summary>
        /// Get repository of a specified Entity type.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Repository for specified entity type.</returns>
        public abstract IDataRepository<TEntity> GetRepository<TEntity>() where TEntity : INotifyPropertyChanged;

        /// <summary>
        /// Register repostiry type.
        /// </summary>
        /// <typeparam name="TEntity">Entity type or interface.</typeparam>
        /// <typeparam name="TRepository">Repository for specified Entities type.</typeparam>
        public virtual void RegisterRepository<TEntity, TRepository>()
            where TEntity : INotifyPropertyChanged
            where TRepository : class, IDataRepository<TEntity>
        {
            if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);

            RegisterRepository<TEntity, TRepository>(null);
        }

        /// <summary>
        /// Register repository instance.
        /// </summary>
        /// <typeparam name="TEntity">Entity type or interface.</typeparam>
        /// <typeparam name="TRepository">Repository for specified Entities type.</typeparam>
        /// <param name="repository">Instance</param>
        public void RegisterRepository<TEntity, TRepository>(TRepository repository)
            where TEntity : INotifyPropertyChanged
            where TRepository : class, IDataRepository<TEntity>
        {
            if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);

            Type entityType = typeof(TEntity);
            Type repositoryType = typeof(TRepository);

            if (_registeredRepositories.ContainsKey(entityType))
            {
                throw new ArgumentException(string.Format(EX_ENTITY_TYPE_ALREADY_REGISTERED, entityType.Name));
            }

            _registeredRepositories.Add(entityType, repositoryType);
            _registeredInstances.Add(repositoryType, repository);
        }

        /// <summary>
        /// Refresh repositories data.
        /// </summary>
        /// <param name="overwriteChanges">Specifies if the changes will be overwiten by source data.</param>
        public async Task RefreshData(bool overwriteChanges = false)
        {
            if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);

            foreach (IRepository repository in Repositories)
            {
                await repository.Refresh(overwriteChanges);
            }
        }

        #endregion

        #region Disposing Methods

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is object currently disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                RegisteredRepositories = null;
                RegisteredInstances = null;
            }
            _isDisposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Default destructor.
        /// </summary>
        ~DataUnitOfWork()
        {
            Dispose(false);
        }

        #endregion
    }
}
