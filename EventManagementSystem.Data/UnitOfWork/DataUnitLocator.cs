using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class DataUnitLocator : IDataUnitLocator
    {
        #region Fields

        private readonly IUnityContainer _unityContainer;

        #endregion

        public DataUnitLocator()
        {
            _unityContainer = ContainerAccessor.Instance.GetContainer();

            _unityContainer.RegisterType<IAdminDataUnit, AdminDataUnit>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<ICrmDataUnit, CrmDataUnit>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IEventDataUnit, EventDataUnit>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IReportsDataUnit, ReportsDataUnit>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IContactsDataUnit, ContactsDataUnit>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IWorkspaceDataUnit, WorkspaceDataUnit>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IWebEnquiryDataUnit, WebEnquiryDataUnit>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IPermissionsDataUnit, PermissionsDataUnit>(new ContainerControlledLifetimeManager());
            _unityContainer.RegisterType<IMembershipDataUnit, MembershipDataUnit>(new ContainerControlledLifetimeManager());

        }

        #region Methods

        /// <summary>
        /// Resolves Data Unit instance.
        /// </summary>
        /// <typeparam name="TInterface">Data Unit interface</typeparam>
        /// <returns>Data Unit instance.</returns>
        public TInterface ResolveDataUnit<TInterface>()
        {
            return _unityContainer.Resolve<TInterface>();
        }

        #endregion
    }
}
