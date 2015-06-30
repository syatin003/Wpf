using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Security;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Admin.Users;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Users
{
    public class UsersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private ObservableCollection<UserGroupModel> _userGroups;
        private object _selectedItem;
        private SafeUserModel _selectedUser;
        private ObservableCollection<UserJobType> _userJobTypes;
        private ObservableCollection<UserDepartment> _userDepartments;
        private bool _isUserGroupChanged;
        private readonly SaltedHash _saltedHash;
        private bool _isDirty;


        #endregion

        #region Properties

        public List<Permission> Permissions { set; get; }
        public List<PermissionGroup> PermissionGroups { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
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

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);

                SelectedUser = value as SafeUserModel;
            }
        }

        public SafeUserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser == value) return;
                RefreshUserAndRevertAllChanges();
                _selectedUser = value;
                IsDirty = false;
                RaisePropertyChanged(() => SelectedUser);
                DeleteUserCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                RaisePropertyChanged(() => IsDirty);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand SaveChangesCommand { get; private set; }
        public RelayCommand DeleteUserCommand { get; private set; }
        public RelayCommand AddUserCommand { get; private set; }
        public RelayCommand EditUserPasswordCommand { get; private set; }

        #endregion

        #region Constructor

        public UsersViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            _saltedHash = new SaltedHash();

            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);
            DeleteUserCommand = new RelayCommand(DeleteUserCommandExecuted, DeleteUserCommandCanExecuted);
            AddUserCommand = new RelayCommand(AddUserCommandExecuted);
            EditUserPasswordCommand = new RelayCommand(EditUserPasswordCommandExecuted, EditUserPasswordCommandCanExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var groups = await _adminDataUnit.PermissionGroupsRepository.GetAllAsync();
            PermissionGroups = groups.OrderBy(x => x.GroupName).ToList();

            var types = await _adminDataUnit.UserJobTypesRepository.GetAllAsync();
            UserJobTypes = new ObservableCollection<UserJobType>(types);

            var departments = await _adminDataUnit.UserDepartmentsRepository.GetAllAsync();
            UserDepartments = new ObservableCollection<UserDepartment>(departments);

            _adminDataUnit.PermissionsRepository.Refresh();

            var permissions = await _adminDataUnit.PermissionsRepository.GetAllAsync();
            Permissions = permissions.OrderBy(x => x.Name).ToList();

            LoadUserGroups();
            SelectedUser = null;
        }

        private async void LoadUserGroups()
        {
            _adminDataUnit.UserGroupsRepository.Refresh();
            _adminDataUnit.UsersRepository.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins);

            var userGroups = await _adminDataUnit.UserGroupsRepository.GetAllAsync();
            UserGroups = new ObservableCollection<UserGroupModel>(userGroups.Select(x => new UserGroupModel(x)));

            foreach (var userGroup in UserGroups)
            {
                LoadUsers(userGroup);
            }

            IsBusy = false;
        }

        private void LoadUsers(UserGroupModel groupModel)
        {
            groupModel.Users =
                new ObservableCollection<SafeUserModel>(groupModel.UserGroup.Users.Select(x => new SafeUserModel(x)));

            foreach (var userModel in groupModel.Users)
            {
                userModel.UserGroup = groupModel;
                userModel.UserJobType = userModel.User.UserJobType;
                userModel.UserDepartment = userModel.User.UserDepartment;

                LoadUserPermissions(userModel);
                LoadAuthorisationPermissions(userModel);

                userModel.PropertyChanged += OnUserModelPropertyChanged;
                userModel.User.PropertyChanged += OnPropertyChanged;
            }
        }

        private void LoadUserPermissions(SafeUserModel userModel)
        {
            userModel.Permissions = new ObservableCollection<PermissionModel>();

            var userPermissions = userModel.User.UserPermissions.Select(x => x.Permission);

            foreach (var permission in Permissions)
            {
                var permissionModel = new PermissionModel(permission)
                {
                    IsChecked = userPermissions.Contains(permission)
                };

                permissionModel.PropertyChanged += PermissionModelOnPropertyChanged;
                permissionModel.Permission.PropertyChanged += OnPropertyChanged;
                userModel.Permissions.Add(permissionModel);
            }
        }

        private void LoadAuthorisationPermissions(SafeUserModel userModel)
        {
            userModel.PermissionGroups = new ObservableCollection<PermissionGroupModel>();

            var userPermissions = userModel.User.UserPermissions.Select(x => x.Permission);

            foreach (var permissionGroup in PermissionGroups)
            {
                var premissionGroupModel = new PermissionGroupModel(permissionGroup);
                premissionGroupModel.Permissions = new ObservableCollection<PermissionModel>();

                foreach (Permission permission in permissionGroup.Permissions.OrderBy(x => x.Name))
                {
                    var permissionModel = new PermissionModel(permission)
                    {
                        IsChecked = userPermissions.Contains(permission)
                    };

                    permissionModel.PropertyChanged += PermissionModelOnPropertyChanged;
                    permissionModel.Permission.PropertyChanged += OnPropertyChanged;
                    premissionGroupModel.Permissions.Add(permissionModel);
                }

                premissionGroupModel.IsChecked = premissionGroupModel.Permissions.Any() &&
                                                 premissionGroupModel.Permissions.All((x) => x.IsChecked);

                premissionGroupModel.PropertyChanged += PremissionGroupModelOnPropertyChanged;

                premissionGroupModel.PermissionGroup.PropertyChanged += OnPropertyChanged;

                userModel.PermissionGroups.Add(premissionGroupModel);
            }
        }

        private void PremissionGroupModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var permissionGroupModel = sender as PermissionGroupModel;

            if (args.PropertyName == "IsChecked")
            {
                IsDirty = true;
                permissionGroupModel.Permissions.ForEach(x => x.IsChecked = permissionGroupModel.IsChecked);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        private void CopyPermissionsFromGroup(UserGroup userGroup, User user)
        {
            var groupPermissions = userGroup.UserGroupPermissions.Select(x => x.Permission);

            foreach (var permission in groupPermissions)
            {
                var userPermission = new UserPermission()
                {
                    ID = Guid.NewGuid(),
                    PermissionID = permission.ID,
                    UserID = user.ID
                };

                _adminDataUnit.UserPermissionsRepository.Add(userPermission);
            }

            _adminDataUnit.SaveChanges();
        }

        private void PermissionModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {

            var permission = sender as PermissionModel;

            if (args.PropertyName == "IsChecked")
            {
                IsDirty = true;
                if (permission.IsChecked)
                {
                    var userPermission = new UserPermission()
                    {
                        ID = Guid.NewGuid(),
                        UserID = SelectedUser.User.ID,
                        PermissionID = permission.Permission.ID,
                    };

                    _adminDataUnit.UserPermissionsRepository.Add(userPermission);
                }
                else
                {
                    var userPermission =
                        SelectedUser.User.UserPermissions.FirstOrDefault(x => x.PermissionID == permission.Permission.ID);
                    _adminDataUnit.UserPermissionsRepository.Delete(userPermission);
                }

                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        private void OnUserModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            IsDirty = true;
            if (args.PropertyName == "UserGroup")
                _isUserGroupChanged = true;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        public void RefreshUserAndRevertAllChanges()
        {
            if (IsDirty)
            {
                _adminDataUnit.UsersRepository.RevertAllChanges();
                if (SelectedUser != null)
                {
                    LoadUserPermissions(SelectedUser);
                    LoadAuthorisationPermissions(SelectedUser);
                }
            }
        }

        #endregion

        #region Commands

        private void SaveChangesCommandExecuted()
        {
            IsDirty = false;
            _adminDataUnit.SaveChanges();

            // if user group was changed we should refresh treeview
            if (_isUserGroupChanged)
            {
                LoadUserGroups();
                _isUserGroupChanged = false;
            }
        }

        private bool SaveChangesCommandCanExecute()
        {
            return SelectedUser != null && !SelectedUser.HasErrors && IsDirty;
        }

        private void DeleteUserCommandExecuted()
        {
            if (SelectedUser == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            if (dialogResult != true) return;

            IsBusy = true;

            // delete all user permissions
            var userPermissions = SelectedUser.User.UserPermissions.ToList();
            _adminDataUnit.UserPermissionsRepository.Delete(userPermissions);

            _adminDataUnit.UsersRepository.Delete(SelectedUser.User);
            _adminDataUnit.SaveChanges();

            SelectedUser.PropertyChanged -= OnUserModelPropertyChanged;

            SelectedUser.UserGroup.UserGroup.Users.Remove(SelectedUser.User);

            SelectedUser = null;

            LoadUserGroups(); // refresh data

            IsBusy = false;
        }

        private bool DeleteUserCommandCanExecuted()
        {
            return SelectedUser != null;
        }

        private void AddUserCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddUserView();
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.ViewModel.User.HasErrors) return;

            IsBusy = true;

            var user = view.ViewModel.User;

            CopyPermissionsFromGroup(user.UserGroup.UserGroup, user.User);
            LoadUserPermissions(user);

            LoadAuthorisationPermissions(user);

            user.PropertyChanged += OnUserModelPropertyChanged;

            var firstOrDefault = UserGroups.FirstOrDefault(x => x.UserGroup.ID == user.User.UserGroupID);
            if (firstOrDefault != null)
                firstOrDefault.Users.Add(user);

            SelectedUser = user;

            IsBusy = false;
        }

        public void EditUserPasswordCommandExecuted()
        {
            string password = string.Empty;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Prompt(new DialogParameters()
            {
                Content = "Enter New Password:",
                Closed = (sender, args) => { password = args.PromptResult; }
            });

            RaisePropertyChanged("EnableParentWindow");

            if (!string.IsNullOrWhiteSpace(password))
            {
                SelectedUser.User.PasswordSalt = Guid.NewGuid().ToString("N");
                SelectedUser.User.PasswordHash = _saltedHash.ComputeHash(password + SelectedUser.User.PasswordSalt);
            }

            //_adminDataUnit.SaveChanges();
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        public bool EditUserPasswordCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_EDIT_USERPASSWORD_ALLOWED);
        }

        #endregion
    }
}