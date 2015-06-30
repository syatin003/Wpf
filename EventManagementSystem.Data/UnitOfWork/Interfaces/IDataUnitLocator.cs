namespace EventManagementSystem.Data.UnitOfWork.Interfaces
{
    public interface IDataUnitLocator
    {
        /// <summary>
        /// Resolves Data Unit instance.
        /// </summary>
        /// <typeparam name="TInterface">Data Unit interface</typeparam>
        /// <returns>Data Unit instance.</returns>
        TInterface ResolveDataUnit<TInterface>();
    }
}
