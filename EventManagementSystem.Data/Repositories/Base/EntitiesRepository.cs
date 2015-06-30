using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using EventManagementSystem.Data.Extensions;

namespace EventManagementSystem.Data.Repositories.Base
{
    public abstract class EntitiesRepository<TEntity> : IDataRepository<TEntity> where TEntity : INotifyPropertyChanged
    {
        protected readonly ObjectContext _objectContext;

        protected EntitiesRepository(ObjectContext objectContext)
        {
            _objectContext = objectContext;
        }

        /// <summary>
        /// Refresh cached objects.
        /// </summary>
        /// <param name="overwriteChanges">Ovewrwrite objects changes.</param>
        public async Task Refresh(bool overwriteChanges = false)
        {
            RefreshMode refreshMode = overwriteChanges ? RefreshMode.StoreWins : RefreshMode.ClientWins;
            await _objectContext.RefreshAsync(refreshMode, _objectContext.GetModifiedObjects());
        }

        public abstract void Add(TEntity entity);

        public abstract void Delete(TEntity entity);
    }
}
