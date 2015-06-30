using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class UserModel : SafeUserModel, IDataErrorInfo
    {
        #region Fields

        private string _password;
        private string _confirmPassword;

        #endregion

        #region Properties

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                RaisePropertyChanged(() => Password);
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (_confirmPassword == value) return;
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
                RaisePropertyChanged(() => Password);
            }
        }

        #endregion

        #region Constructor

        public UserModel(User user) : base(user)
        {
            ConfirmPassword = Password;
        }

        #endregion

        #region IDataErrorInfo

        public override bool HasErrors
        {
            get { return typeof (UserModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public new string this[string columnName]
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

                if (columnName == "Password")
                {
                    if (string.IsNullOrEmpty(Password))
                        Error = "Password can't be empty!";
                    else if (Password != ConfirmPassword)
                        Error = "Password does not match the confirm password!";
                }

                if (columnName == "ConfirmPassword")
                {
                    if (string.IsNullOrEmpty(ConfirmPassword))
                        Error = "Confirm password can't be empty!";
                    else if (Password != ConfirmPassword)
                        Error = "Password does not match the confirm password!";
                }

                if (columnName == "ShortCode")
                {
                    if (string.IsNullOrEmpty(ShortCode))
                        Error = "Short code can't be empty!";
                }

                if (columnName == "EmailAddress")
                {
                    string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                     + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                     + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                    var regex = new Regex(pattern, RegexOptions.IgnoreCase);

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

        #endregion
    }
}