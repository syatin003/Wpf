using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class UserGroupModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly UserGroup _userGroup;
        private ObservableCollection<SafeUserModel> _users;
        private ObservableCollection<PermissionModel> _permissions;
        private ObservableCollection<PermissionGroupModel> _permissionGroups;

        #endregion

        #region Properties

        public UserGroup UserGroup
        {
            get { return _userGroup; }
        }

        public ObservableCollection<SafeUserModel> Users
        {
            get { return _users; }
            set
            {
                if (_users == value) return;
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public ObservableCollection<PermissionModel> Permissions
        {
            get { return _permissions; }
            set
            {
                if (_permissions == value) return;
                _permissions = value;
                RaisePropertyChanged(() => Permissions);
            }
        }

        public ObservableCollection<PermissionGroupModel> PermissionGroups
        {
            get { return _permissionGroups; }
            set
            {
                if (_permissionGroups == value) return;
                _permissionGroups = value;
                RaisePropertyChanged(() => PermissionGroups);
            }
        }

        public string Name
        {
            get { return _userGroup.Name; }
            set 
            { 
                if (_userGroup.Name == value) return;
                _userGroup.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string Description
        {
            get { return _userGroup.Description; }
            set
            {
                if (_userGroup.Description == value) return;
                _userGroup.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public bool IsAdmin
        {
            get { return _userGroup.IsAdmin; }
            set
            {
                if (_userGroup.IsAdmin == value) return;
                _userGroup.IsAdmin = value;
                RaisePropertyChanged(() => IsAdmin);
            }
        }

        public string Colour
        {
            get { return _userGroup.Colour; }
            set
            {
                if (_userGroup.Colour == value) return;
                _userGroup.Colour = value;
                RaisePropertyChanged(() => Colour);
            }
        }

        public string AuthCode
        {
            get { return _userGroup.AuthCode; }
            set
            {
                if (_userGroup.AuthCode == value) return;
                _userGroup.AuthCode = value;
                RaisePropertyChanged(() => AuthCode);
            }
        }

        #endregion

        #region Constructor

        public UserGroupModel(UserGroup userGroup)
        {
            _userGroup = userGroup;
        }

        #endregion

        #region IDataErrorInfo Properties

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        Error = "Please enter a name!";
                }
                if (columnName == "AuthCode")
                {
                    var regex = new Regex("[^0-9.-]+");

                    if (regex.IsMatch(AuthCode))
                        Error = "Only numeric characters allowed";
                }

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion

    }
}
