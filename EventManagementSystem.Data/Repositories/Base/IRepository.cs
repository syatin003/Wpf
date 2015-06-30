using System.Threading.Tasks;

namespace EventManagementSystem.Data.Repositories.Base
{
    public interface IRepository
    {
        /// <summary>
        /// Refresh cached objects.
        /// </summary>
        /// <param name="overwriteChanges">Ovewrwrite objects changes.</param>
        Task Refresh(bool overwriteChanges = false);
    }
}
