using System.Threading.Tasks;

namespace EventManagementSystem.Data.UnitOfWork.Base
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Save current changes.
        /// </summary>
        Task<int> SaveChanges();

        /// <summary>
        /// Discard all changes, back to the initial state.
        /// </summary>
        void RevertChanges();
    }
}
