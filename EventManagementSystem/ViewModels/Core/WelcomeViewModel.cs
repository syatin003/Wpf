using System;
using System.IO;
using System.Reflection;
using System.Windows;
using EventManagementSystem.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using EventManagementSystem.Views;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core
{
    public class WelcomeViewModel : ViewModelBase
    {
        #region Fields

        private readonly IWorkspaceDataUnit _workspaceDataUnit;
        private string _username;
        private string _password;
        private bool _isBusy;
        private string _applicationVersion;

        #endregion

        #region Properties

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username == value) return;
                _username = value;
                RaisePropertyChanged(() => Username);
                UserLoginCommand.RaiseCanExecuteChanged();
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
                UserLoginCommand.RaiseCanExecuteChanged();
            }
        }

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

        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set
            {
                if (_applicationVersion == value) return;
                _applicationVersion = value;
                RaisePropertyChanged(() => ApplicationVersion);
            }
        }

        public RelayCommand UserLoginCommand { get; private set; }

        #endregion

        #region Constructor

        public WelcomeViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _workspaceDataUnit = dataUnitLocator.ResolveDataUnit<IWorkspaceDataUnit>();

            ApplicationVersion = GetApplicationVersion();

            UserLoginCommand = new RelayCommand(UserLoginCommandExecuted, UserLoginCommandCanExecute);
        }

        #endregion

        #region Methods

        private string GetApplicationVersion()
        {
            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

            return string.Format("Release Date: {0}", buildDate.ToString("D"));
        }

        #endregion

        #region Commands

        private async void UserLoginCommandExecuted()
        {
            IsBusy = true;

            _workspaceDataUnit.UsersRepository.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins);
            AccessService.Current.User = await _workspaceDataUnit.UsersRepository.GetUserAsync(Username, Password);

            OnLoginRequested();
        }

        private void OnLoginRequested()
        {
            var loginSuccessfully = AccessService.Current.User != null;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (loginSuccessfully)
                {
                    PopupService.ShowMessage(Resources.MESSAGE_LOGIN_SUCCESSFULLY, MessageType.Successful);

                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.ViewModel.NavigateToWorkspaceView();
                }
                else
                {
                    PopupService.ShowMessage(Resources.MESSAGE_LOGIN_FAILED, MessageType.Failed);
                    IsBusy = false;
                }
            }));
        }

        private bool UserLoginCommandCanExecute()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        #endregion
    }
}