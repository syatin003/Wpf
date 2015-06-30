using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.UserGroups
{
    public class UserGroupsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;

        private ObservableCollection<UserGroupModel> _userGroups;
        private UserGroupModel _selectedUserGroup;
        private object _selectedItem;
        private bool _isDirty;

        #endregion

        #region Properties

        public IEnumerable<Permission> Permissions { set; get; }
        public IEnumerable<PermissionGroup> PermissionGroups { get; set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
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

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);

                SelectedUserGroup = value as UserGroupModel;
            }
        }

        public UserGroupModel SelectedUserGroup
        {
            get { return _selectedUserGroup; }
            set
            {
                if (_selectedUserGroup == value) return;
                RefreshUserGroupAndRevertAllChanges();
                _selectedUserGroup = value;
                IsDirty = false;
                RaisePropertyChanged(() => SelectedUserGroup);

                DeleteUserGroupCommand.RaiseCanExecuteChanged();
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
        public RelayCommand AddUserGroupCommand { get; private set; }
        public RelayCommand DeleteUserGroupCommand { get; private set; }

        #endregion

        #region Constructor

        public UserGroupsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);
            AddUserGroupCommand = new RelayCommand(AddUserGroupCommandExecuted);
            DeleteUserGroupCommand = new RelayCommand(DeleteUserGroupCommandExecuted, DeleteUserGroupCommandCanExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            PermissionGroups = await _adminDataUnit.PermissionGroupsRepository.GetAllAsync();

            _adminDataUnit.PermissionsRepository.Refresh();

            Permissions = await _adminDataUnit.PermissionsRepository.GetAllAsync();

            _adminDataUnit.UserGroupsRepository.Refresh();

            var userGroups = await _adminDataUnit.UserGroupsRepository.GetAllAsync();
            UserGroups = new ObservableCollection<UserGroupModel>(userGroups.Select(x => new UserGroupModel(x)));

            foreach (UserGroupModel groupModel in UserGroups)
            {
                LoadPermissions(groupModel);
                LoadAuthorisationPermissions(groupModel);
            }

            IsBusy = false;
        }

        private void LoadPermissions(UserGroupModel groupModel)
        {
            groupModel.Permissions = new ObservableCollection<PermissionModel>();

            var groupPermissions = groupModel.UserGroup.UserGroupPermissions.Select(x => x.Permission);
            foreach (Permission permission in Permissions)
            {
                var permissionModel = new PermissionModel(permission)
                    {
                        IsChecked = groupPermissions.Contains(permission)
                    };

                permissionModel.PropertyChanged += PermissionModelOnPropertyChanged;
                permissionModel.Permission.PropertyChanged += OnPropertyChanged;

                groupModel.Permissions.Add(permissionModel);

                groupModel.PropertyChanged += OnPropertyChanged;
                groupModel.UserGroup.PropertyChanged += OnPropertyChanged;
            }
        }

        private void LoadAuthorisationPermissions(UserGroupModel groupModel)
        {
            groupModel.PermissionGroups = new ObservableCollection<PermissionGroupModel>();

            var groupPermissions = groupModel.UserGroup.UserGroupPermissions.Select(x => x.Permission);

            foreach (var permissionGroup in PermissionGroups)
            {
                var premissionGroupModel = new PermissionGroupModel(permissionGroup);
                premissionGroupModel.Permissions = new ObservableCollection<PermissionModel>();

                foreach (Permission permission in permissionGroup.Permissions)
                {
                    var permissionModel = new PermissionModel(permission)
                        {
                            IsChecked = groupPermissions.Contains(permission)
                        };

                    permissionModel.PropertyChanged += PermissionModelOnPropertyChanged;
                    permissionModel.Permission.PropertyChanged += OnPropertyChanged;

                    premissionGroupModel.Permissions.Add(permissionModel);
                }

                premissionGroupModel.IsChecked = premissionGroupModel.Permissions.Any() && premissionGroupModel.Permissions.All((x) => x.IsChecked);
                premissionGroupModel.PropertyChanged += PremissionGroupModelOnPropertyChanged;
                premissionGroupModel.PermissionGroup.PropertyChanged += PremissionGroupOnPropertyChanged;

                groupModel.PermissionGroups.Add(premissionGroupModel);
                groupModel.PropertyChanged += OnPropertyChanged;
                groupModel.UserGroup.PropertyChanged += OnPropertyChanged;
            }
        }

        private void PremissionGroupModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var permissionGroupModel = sender as PermissionGroupModel;

            if (args.PropertyName == "IsChecked")
            {
                IsDirty = true;
                permissionGroupModel.Permissions.ForEach(x => x.IsChecked = permissionGroupModel.IsChecked);
            }
        }

        private void PremissionGroupOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;

        }

        private void PermissionModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var permission = sender as PermissionModel;

            if (args.PropertyName == "IsChecked")
            {
                IsDirty = true;
                if (permission.IsChecked)
                {
                    SelectedUserGroup.UserGroup.Users.ForEach(user =>
                        {
                            var userPermission = user.UserPermissions.FirstOrDefault(per => per.PermissionID == permission.Permission.ID);
                            if (userPermission == null)
                            {
                                var newUserPermission = new UserPermission()
                                                {
                                                    ID = Guid.NewGuid(),
                                                    UserID = user.ID,
                                                    PermissionID = permission.Permission.ID,
                                                };

                                _adminDataUnit.UserPermissionsRepository.Add(newUserPermission);
                            }
                        });
                    var groupPermission = new UserGroupPermission()
                        {
                            ID = Guid.NewGuid(),
                            GroupID = SelectedUserGroup.UserGroup.ID,
                            PermissionID = permission.Permission.ID,
                        };
                    _adminDataUnit.UserGroupPermissionsRepository.Add(groupPermission);
                }
                else
                {
                    SelectedUserGroup.UserGroup.Users.ForEach(user =>
                       {
                           var userPermission = user.UserPermissions.FirstOrDefault(per => per.PermissionID == permission.Permission.ID);
                           if (userPermission != null)
                               _adminDataUnit.UserPermissionsRepository.Delete(userPermission);
                       });
                    var groupPermission = SelectedUserGroup.UserGroup.UserGroupPermissions.FirstOrDefault(x => x.PermissionID == permission.Permission.ID);
                    _adminDataUnit.UserGroupPermissionsRepository.Delete(groupPermission);
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        public void RefreshUserGroupAndRevertAllChanges()
        {
            if (IsDirty)
            {
                _adminDataUnit.UserGroupsRepository.RevertAllChanges();
                if (SelectedUserGroup != null)
                {
                    LoadPermissions(SelectedUserGroup);
                    LoadAuthorisationPermissions(SelectedUserGroup);
                }
            }
        }

        #endregion

        #region Commands

        private void SaveChangesCommandExecuted()
        {
            IsBusy = true;

            IsDirty = false;
            _adminDataUnit.SaveChanges();

            IsBusy = false;
        }


        private bool SaveChangesCommandCanExecute()
        {
            return SelectedUserGroup != null && IsDirty;
        }

        private void AddUserGroupCommandExecuted()
        {
            var group = new UserGroup()
                {
                    ID = Guid.NewGuid(),
                    Name = "New User Group",
                    Colour = "#808080",
                    AuthCode = ""
                };

            _adminDataUnit.UserGroupsRepository.Add(group);
            _adminDataUnit.SaveChanges();

            var groupModel = new UserGroupModel(group);

            LoadPermissions(groupModel);
            LoadAuthorisationPermissions(groupModel);

            UserGroups.Add(groupModel);

            SelectedUserGroup = groupModel;
        }

        private void DeleteUserGroupCommandExecuted()
        {
            if (SelectedUserGroup == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            if (dialogResult != true) return;

            var users = SelectedUserGroup.UserGroup.Users.ToList();

            // delete user permissions
            users.ForEach((user) => _adminDataUnit.UserPermissionsRepository.Delete(user.UserPermissions.ToList()));

            // delete all users
            _adminDataUnit.UsersRepository.Delete(users);

            // delete group permissions
            var groupPermissions = SelectedUserGroup.UserGroup.UserGroupPermissions.ToList();
            _adminDataUnit.UserGroupPermissionsRepository.Delete(groupPermissions);

            // delete group
            _adminDataUnit.UserGroupsRepository.Delete(SelectedUserGroup.UserGroup);
            _adminDataUnit.SaveChanges();

            UserGroups.Remove(SelectedUserGroup);
            SelectedUserGroup = null;
        }

        private bool DeleteUserGroupCommandCanExecuted()
        {
            return SelectedUserGroup != null;
        }

        #endregion
    }
}