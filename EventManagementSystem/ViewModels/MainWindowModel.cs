using System.Windows.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Core;

namespace EventManagementSystem.ViewModels
{
    public class MainWindowModel : ViewModelBase
    {
        #region Fields

        private ContentControl _windowContent;
        private string _username;
        private bool _popupOverlay;


        #endregion

        #region Properties

        public ContentControl WindowContent
        {
            get { return _windowContent; }
            set
            {
                if (_windowContent == value) return;
                _windowContent = value;
                RaisePropertyChanged(() => WindowContent);
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

        public bool PopupOverlay
        {
            get { return _popupOverlay; }
            set
            {
                if (_popupOverlay == value) return;
                _popupOverlay = value;
                RaisePropertyChanged(() => PopupOverlay);
            }
        }

        public RelayCommand LogoutCommand { get; private set; }

        #endregion

        #region Constructor

        public MainWindowModel()
        {
            LogoutCommand = new RelayCommand(LogoutCommandExecuted);

            NavigateToWelcomeView();
        }

        #endregion

        #region Methods

        private void LogoutCommandExecuted()
        {
            Logout();
        }

        public void Logout()
        {
            Username = null;
            AccessService.Current.User = null;

            NavigateToWelcomeView();
        }

        public void NavigateToWelcomeView()
        {
            WindowContent = new WelcomeView();
        }

        public void NavigateToWorkspaceView()
        {
            WindowContent = new WorkspaceView();

            Username = string.Format("Welcome, {0} {1}", AccessService.Current.User.FirstName, AccessService.Current.User.LastName);
        }

        public void ChangePopupOverlay()
        {
            if (PopupOverlay)
                PopupOverlay = false;
            else
                PopupOverlay = true;
        }
        #endregion
    }
}