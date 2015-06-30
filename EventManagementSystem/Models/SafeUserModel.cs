using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class SafeUserModel : ModelBase
    {
        #region Fields

        private readonly User _user;
        private UserGroupModel _userGroup;
        private UserJobType _userJobType;
        private UserDepartment _userDepartment;

        private ObservableCollection<PermissionModel> _permissions;
        private ObservableCollection<PermissionGroupModel> _permissionGroups;

        #endregion

        #region Properties

        public List<string> RegisteredLogins { get; set; }

        public User User
        {
            get { return _user; }
        }

        public string Name
        {
            get { return string.Format("{0} {1}", _user.FirstName, _user.LastName); }
        }

        public UserJobType UserJobType
        {
            get { return _userJobType; }
            set
            {
                if (_userJobType == value) return;
                _userJobType = value;
                _user.JobTypeID = _userJobType.ID;
                RaisePropertyChanged(() => UserJobType);
            }
        }

        public UserDepartment UserDepartment
        {
            get { return _userDepartment; }
            set
            {
                if (_userDepartment == value) return;
                _userDepartment = value;
                _user.DepartmentID = _userDepartment.ID;
                RaisePropertyChanged(() => UserDepartment);
            }
        }

        public UserGroupModel UserGroup
        {
            get { return _userGroup; }
            set
            {
                if (_userGroup == value) return;
                _userGroup = value;
                _user.UserGroup = _userGroup.UserGroup;
                RaisePropertyChanged(() => UserGroup);
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

        public string FirstName
        {
            get { return _user.FirstName; }
            set
            {
                if (_user.FirstName == value) return;
                _user.FirstName = value;
                RaisePropertyChanged(() => FirstName);
                RaisePropertyChanged(() => Name);
            }
        }

        public string LastName
        {
            get { return _user.LastName; }
            set
            {
                if (_user.LastName == value) return;
                _user.LastName = value;
                RaisePropertyChanged(() => LastName);
                RaisePropertyChanged(() => Name);
            }
        }

        public string UserName
        {
            get { return _user.UserName; }
            set
            {
                if (_user.UserName == value) return;
                _user.UserName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public bool IsEnabled
        {
            get { return _user.IsEnabled; }
            set
            {
                if (_user.IsEnabled == value) return;
                _user.IsEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        public string ShortCode
        {
            get { return _user.ShortCode; }
            set
            {
                if (_user.ShortCode == value) return;
                _user.ShortCode = value;
                RaisePropertyChanged(() => ShortCode);
            }
        }

        public string EmailAddress
        {
            get { return _user.EmailAddress; }
            set
            {
                if (_user.EmailAddress == value) return;
                _user.EmailAddress = value;
                RaisePropertyChanged(() => EmailAddress);
            }
        }

        public string IdNumber
        {
            get { return _user.IdNumber; }
            set
            {
                if (_user.IdNumber == value) return;
                _user.IdNumber = value;
                RaisePropertyChanged(() => IdNumber);
            }
        }

        public string AuthCode
        {
            get { return _user.AuthCode; }
            set
            {
                if (_user.AuthCode == value) return;
                _user.AuthCode = value;
                RaisePropertyChanged(() => AuthCode);
            }
        }

        public string Colour
        {
            get { return _user.Colour; }
            set
            {
                _user.Colour = value;
                RaisePropertyChanged(() => Colour);
            }
        }

        public string InternalName
        {
            get
            {
                return _user.InternalName;
            }
            set
            {
                if (_user.InternalName == value) return;
                _user.InternalName = value;
                RaisePropertyChanged(() => InternalName);
            }
        }


        public string EmailSignature
        {
            get
            {
                return _user.EmailSignature;
            }
            set
            {
                if (_user.EmailSignature == value) return;
                _user.EmailSignature = value;
                RaisePropertyChanged(() => EmailSignature);
            }
        }


        #endregion

        #region Constructor

        public SafeUserModel(User user)
        {
            _user = user;
            UserGroup = new UserGroupModel(user.UserGroup);
        }

        #endregion

        #region IDataErrorInfo Properties

        public virtual bool HasErrors
        {
            get { return typeof(SafeUserModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                        Error = "First name can't be empty!";
                }

                if (columnName == "LastName")
                {
                    if (string.IsNullOrEmpty(LastName))
                        Error = "Last name can't be empty!";
                }

                if (columnName == "UserName")
                {
                    if (string.IsNullOrEmpty(UserName))
                        Error = "Username can't be empty!";
                    else if (RegisteredLogins != null && RegisteredLogins.Contains(UserName))
                        Error = "Sorry, that username is already taken";
                }

                if (columnName == "ShortCode")
                {
                    if (string.IsNullOrEmpty(ShortCode))
                        Error = "Short code can't be empty!";
                }

                if (columnName == "EmailAddress")
                {
                    var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

                    if (string.IsNullOrEmpty(EmailAddress))
                        Error = "Email address code can't be empty!";
                    else if (!regex.IsMatch(EmailAddress))
                        Error = "Email address is not valid";
                }

                if (columnName == "UserJobType")
                {
                    if (UserJobType == null)
                        Error = "Job type can't be empty!";
                }

                if (columnName == "UserDepartment")
                {
                    if (UserDepartment == null)
                        Error = "User department can't be empty!";
                }

                if (columnName == "UserGroup")
                {
                    if (UserGroup == null)
                        Error = "User group can't be empty!";
                }

                return Error;
            }
        }

        public string Error { get; protected set; }

        #endregion
    }
}
