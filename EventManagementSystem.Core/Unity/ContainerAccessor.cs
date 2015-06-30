using Microsoft.Practices.Unity;

namespace EventManagementSystem.Core.Unity
{
    public class ContainerAccessor
    {
        #region Fields

        private readonly UnityContainer _container;

        #endregion

        #region Singleton/Constructor

        public static ContainerAccessor Instance { get; private set; }

        private ContainerAccessor()
        {
            _container = new UnityContainer();
        }

        static ContainerAccessor()
        {
            Instance = new ContainerAccessor();
        }

        #endregion

        #region Methods

        public IUnityContainer GetContainer()
        {
            return _container;
        }

        #endregion
    }
}
