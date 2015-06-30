using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace EventManagementSystem.Data.Extensions
{
    /// <summary>
    /// Entity framework object context extensions.
    /// </summary>
    public static class ObjectContextExtensions
    {
        /// <summary>
        /// Get all added entities.
        /// </summary>
        /// <param name="objectContext">ObjectContext to which entites was added.</param>
        /// <returns>Added entites</returns>
        public static IEnumerable<object> GetAddedObjects(this ObjectContext objectContext)
        {
            return
                objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Select(entry => entry.Entity);
        }

        /// <summary>
        /// Get all added entities of specified type.
        /// </summary>
        /// <param name="objectContext">ObjectContext to which entites was added.</param>
        /// <returns>Added entites</returns>
        public static IEnumerable<T> GetAddedObjects<T>(this ObjectContext objectContext)
        {
            return objectContext.GetAddedObjects().OfType<T>();
        }

        /// <summary>
        /// Get all deleted entities.
        /// </summary>
        /// <param name="objectContext">ObjectContext from which items was deleted.</param>
        /// <returns>Deleted items</returns>
        public static IEnumerable<object> GetDeletedObjects(this ObjectContext objectContext)
        {
            return
                objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).Select(entry => entry.Entity);
        }

        /// <summary>
        /// Get all deleted entities of specified type.
        /// </summary>
        /// <param name="objectContext">ObjectContext from which items was deleted.</param>
        /// <returns>Deleted items</returns>
        public static IEnumerable<T> GetDeletedObjects<T>(this ObjectContext objectContext)
        {
            return objectContext.GetDeletedObjects().OfType<T>();
        }

        /// <summary>
        /// Get all modified entites.
        /// </summary>
        /// <param name="objectContext">ObjectContext in which items was modified.</param>
        /// <returns>Modified items.</returns>
        public static IEnumerable<object> GetModifiedObjects(this ObjectContext objectContext)
        {
            return
                objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).Select(
                    entry => entry.Entity);
        }

        /// <summary>
        /// Get all modified entites of specified type.
        /// </summary>
        /// <param name="objectContext">ObjectContext in which items was modified.</param>
        /// <returns>Modified items.</returns>
        public static IEnumerable<T> GetModifiedObjects<T>(this ObjectContext objectContext)
        {
            return objectContext.GetModifiedObjects().OfType<T>();
        }
    }
}
