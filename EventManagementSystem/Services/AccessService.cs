using System.Collections.Generic;
using System.Linq;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Core.Unity;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.Services
{
    public class AccessService
    {
        #region Fields


        private User _user;
        private List<Permission> _userPermissions;
        private readonly IPermissionsDataUnit _permissionsDataUnit;

        #endregion

        #region Properties

        public User User
        {
            get { return _user; }
            set
            {
                if (_user == value) return;
                _user = value;
                LoadUserPermissions(User);
            }
        }

        #endregion

        #region Singleton/Constructor

        private static AccessService _instance;

        private AccessService()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _permissionsDataUnit = dataUnitLocator.ResolveDataUnit<IPermissionsDataUnit>();
        }

        /// <summary>
        /// Returns current instence.
        /// </summary>
        public static AccessService Current
        {
            get { return _instance ?? (_instance = new AccessService()); }
        }

        #endregion

        #region Methods

        private void LoadUserPermissions(User user)
        {
            if (user != null)
            {
                _userPermissions = new List<Permission>(user.UserPermissions.Select(x => x.Permission).ToList());
            }
        }

        public bool UserHasPermissions(string permission)
        {
            //RefreshUserAndPermissions();
            return _userPermissions.Any(x => x.Name == permission);
        }

        private void RefreshUserAndPermissions()
        {
            _permissionsDataUnit.UsersRepository.Refresh(User);
             _permissionsDataUnit.UsersRepository.GetUsersAsync(user => user.ID == User.ID);
        }

        #endregion
    }
}