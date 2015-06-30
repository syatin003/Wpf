using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Core;
using EventManagementSystem.Data.Repositories.Base;

namespace EventManagementSystem.Data.UnitOfWork.Base
{
    public class EntitiesUnitOfWork : DataUnitOfWork
    {
        #region Fields

        protected readonly ObjectContext _objectContext;
        private bool _isDisposed;

        #endregion

        #region Constructor

        public EntitiesUnitOfWork(ObjectContext objectContext)
        {
            Argument.IsNotNull("objectContext", objectContext);

            _objectContext = objectContext;
        }

        #endregion

        #region Properties

        public ObjectContext ObjectContext
        {
            get { return _objectContext; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get repository of a specified Entity type.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Repository for specified entity type.</returns>
        public override IDataRepository<TEntity> GetRepository<TEntity>()
        {
            if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);

            Type entityType = typeof (TEntity);

            if (!RegisteredRepositories.ContainsKey(entityType))
            {
                throw new KeyNotFoundException(string.Format(EX_REPOSITORY_DOES_NOT_EXISTS, entityType.Name));
            }

            Type repositoryType = RegisteredRepositories[entityType];
            IDataRepository<TEntity> instance;

            if (RegisteredInstances[repositoryType] != null)
            {
                instance = (IDataRepository<TEntity>) RegisteredInstances[repositoryType];
            }
            else
            {
                instance = (IDataRepository<TEntity>) Activator.CreateInstance(repositoryType, new object[] {ObjectContext});
                RegisteredInstances[repositoryType] = instance;
            }

            return instance;
        }

        /// <summary>
        /// Save current changes.
        /// </summary>
        public async override Task<int> SaveChanges()
        {
            return await ObjectContext.SaveChangesAsync();
        }

        /// <summary>
        /// Discard all changes, back to the initial state.
        /// </summary>
        public async override void RevertChanges()
        {
            if (_isDisposed) throw new ObjectDisposedException(EX_OBJECT_IS_DISPOSED);
            await RefreshData(true);
        }

        #endregion

        #region Disposing Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is object currently disposing.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!_isDisposed && disposing)
            {
                ObjectContext.Dispose();
            }
            _isDisposed = true;
        }

        /// <summary>
        /// Default destructor.
        /// </summary>
        ~EntitiesUnitOfWork()
        {
            Dispose(false);
        }

        #endregion
    }
}