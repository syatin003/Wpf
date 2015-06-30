using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Security;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Users
{
    public class AddUserViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private ObservableCollection<UserGroupModel> _userGroups;
        private ObservableCollection<UserJobType> _userJobTypes;
        private ObservableCollection<UserDepartment> _userDepartments;

        private UserModel _user;
        private readonly SaltedHash _saltedHash;
        private bool _isBusy;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public UserModel User
        {
            get { return _user; }
            set
            {
                if (_user == value) return;
                _user = value;
                RaisePropertyChanged(() => User);
            }
        }

        public ObservableCollection<UserGroupModel> UserGroups
        {
            get { return _userGroups; }
            set
            {
                if (_userGroups == value) return;
                _userGroups = value;
                RaisePropertyChanged(() => UserGroups);
            }
        }

        public ObservableCollection<UserJobType> UserJobTypes
        {
            get { return _userJobTypes; }
            set
            {
                if (_userJobTypes == value) return;
                _userJobTypes = value;
                RaisePropertyChanged(() => UserJobTypes);
            }
        }

        public ObservableCollection<UserDepartment> UserDepartments
        {
            get { return _userDepartments; }
            set
            {
                if (_userDepartments == value) return;
                _userDepartments = value;
                RaisePropertyChanged(() => UserDepartments);
            }
        }

        public RelayCommand OkCommand { get; private set; }

        #endregion

        #region Constructors

        public AddUserViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            _saltedHash = new SaltedHash();

            OkCommand = new RelayCommand(OkCommandExecuted, OkCommandCanExecute);

            AddUser();
        }

        #endregion

        #region Methods

        private void AddUser()
        {
            var userModel = new UserModel(new User()
            {
                ID = Guid.NewGuid(),
                Colour = "#808080",
                IdNumber = ""
            });

            userModel.PropertyChanged += OnUserPropertyChanged;
            User = userModel;
        }

        public async void LoadData()
        {
            IsBusy = true;

            var types = await _adminDataUnit.UserJobTypesRepository.GetAllAsync();
            UserJobTypes = new ObservableCollection<UserJobType>(types);

            var departments = await _adminDataUnit.UserDepartmentsRepository.GetAllAsync();
            UserDepartments = new ObservableCollection<UserDepartment>(departments);

            var groups = await _adminDataUnit.UserGroupsRepository.GetAllAsync();
            UserGroups =
                new ObservableCollection<UserGroupModel>(groups.OrderBy(x => x.Name).Select(x => new UserGroupModel(x)));

            var users = await _adminDataUnit.UsersRepository.GetUsersAsync();

            User.RegisteredLogins = new List<string>(users.Select(x => x.UserName));

            IsBusy = false;
        }

        private void OnUserPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OkCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void OkCommandExecuted()
        {
            User.User.PasswordSalt = Guid.NewGuid().ToString("N");
            User.User.PasswordHash = _saltedHash.ComputeHash(User.Password + User.User.PasswordSalt);

            _adminDataUnit.UsersRepository.Add(User.User);
            _adminDataUnit.SaveChanges();
        }

        private bool OkCommandCanExecute()
        {
            return !User.HasErrors;
        }

        #endregion
    }
}