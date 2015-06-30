using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.Models
{
    public class EmailServerSettingsModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private string _server;
        private bool _enableSsl;
        private string _username;
        private string _password;
        private int _port;

        #endregion

        #region Properties

        public string Server
        {
            get { return _server; }
            set
            {
                if (_server == value) return;
                _server = value;
                RaisePropertyChanged(() => Server);
            }
        }

        public bool EnableSsl
        {
            get { return _enableSsl; }
            set
            {
                if (_enableSsl == value) return;
                _enableSsl = value;
                RaisePropertyChanged(() => EnableSsl);
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username == value) return;
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public int Port
        {
            get { return _port; }
            set
            {
                if (_port == value) return;
                _port = value;
                RaisePropertyChanged(() => Port);
            }
        }

        #endregion

        #region Methods

        #endregion

        #region IDataError implementation

        public bool HasErrors
        {
            get { return typeof(EmailServerSettingsModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Port")
                    if (Port <= 0)
                        Error = "Port can't be empty or zero";

                if (columnName == "Password")
                    if (string.IsNullOrWhiteSpace(Password))
                        Error = "Password can't be empty or empty";

                if (columnName == "Username")
                    if (string.IsNullOrWhiteSpace(Username))
                        Error = "Username can't be empty or empty";

                if (columnName == "Server")
                    if (string.IsNullOrWhiteSpace(Server))
                        Error = "Server can't be empty or empty";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}