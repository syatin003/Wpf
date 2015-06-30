using System;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;
using EventManagementSystem.Services;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class EmailSettingsViewModel : ViewModelBase
    {
        #region Fields

        private EmailServerSettingsModel _emailServerSettings;
        private bool _isDirty;

        #endregion

        #region Properties

        public EmailServerSettingsModel EmailServerSettings
        {
            get { return _emailServerSettings; }
            set
            {
                if (_emailServerSettings == value) return;
                _emailServerSettings = value;
                RaisePropertyChanged(() => EmailServerSettings);
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
        public RelayCommand SaveChangesCommand { get; set; }

        #endregion

        #region Constructors

        public EmailSettingsViewModel()
        {
            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);

            LoadEmailServerSettings();
        }

        #endregion

        #region Methods

        private void LoadEmailServerSettings()
        {
            EmailServerSettings = new EmailServerSettingsModel()
            {
                Server = (string)ApplicationSettings.Read("Server"),
                Username = (string)ApplicationSettings.Read("Username"),
                Password = (string)ApplicationSettings.Read("Password"),
            };

            var enableSsl = ApplicationSettings.Read("EnableSsl");

            if (enableSsl != null)
                EmailServerSettings.EnableSsl = Convert.ToBoolean(enableSsl);

            var port = ApplicationSettings.Read("Port");

            if (port != null)
                EmailServerSettings.Port = Convert.ToInt32(port);

            EmailServerSettings.PropertyChanged += EmailServerSettings_PropertyChanged;
        }

        private void EmailServerSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsDirty")
                IsDirty = true;
        }

        #endregion

        #region Commands

        private void SaveChangesCommandExecuted()
        {
            IsDirty = false;

            ApplicationSettings.Write("Server", string.IsNullOrWhiteSpace(EmailServerSettings.Server) ? "" : EmailServerSettings.Server);
            ApplicationSettings.Write("EnableSsl", EmailServerSettings.EnableSsl);
            ApplicationSettings.Write("Username", string.IsNullOrWhiteSpace(EmailServerSettings.Username) ? "" : EmailServerSettings.Username);
            ApplicationSettings.Write("Password", string.IsNullOrWhiteSpace(EmailServerSettings.Password) ? "" : EmailServerSettings.Password);
            ApplicationSettings.Write("Port", EmailServerSettings.Port.ToString());

        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsDirty;
        }
        #endregion
    }
}
