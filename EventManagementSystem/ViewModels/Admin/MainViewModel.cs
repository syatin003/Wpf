using System.Windows.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Admin.CRM;
using EventManagementSystem.Views.Admin.Events;
using EventManagementSystem.Views.Admin.EPOS;
using EventManagementSystem.Views.Admin.Members;
using EventManagementSystem.Views.Admin.Resources;
using EventManagementSystem.Views.Admin.Settings;
using EventManagementSystem.Views.Admin.UserGroups;
using EventManagementSystem.Views.Admin.Users;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private ContentControl _windowContent;

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

        public RelayCommand NavigateToResourcesCommand { get; private set; }
        public RelayCommand NavigateToUsersCommand { get; private set; }
        public RelayCommand NavigateToUserGroupsCommand { get; private set; }
        public RelayCommand NavigateToEventsCommand { get; private set; }
        public RelayCommand NavigateToProductsCommand { get; private set; }
        public RelayCommand NavigateToSettingsCommand { get; private set; }
        public RelayCommand NavigateToCRMCommand { get; private set; }
        public RelayCommand NavigateToMembersCommand { get; private set; }


        #endregion

        #region Constructors

        public MainViewModel()
        {
            NavigateToResourcesCommand = new RelayCommand(NavigateToResourcesCommandExecuted, NavigateToResourcesCommandCanExecute);
            NavigateToUsersCommand = new RelayCommand(NavigateToUsersCommandExecuted, NavigateToUsersCommandCanExecute);
            NavigateToUserGroupsCommand = new RelayCommand(NavigateToUserGroupsCommandExecuted, NavigateToUserGroupsCommandCanExecute);
            NavigateToEventsCommand = new RelayCommand(NavigateToEventsCommandExecuted, NavigateToEventsCommandCanExecute);
            NavigateToProductsCommand = new RelayCommand(NavigateToProductsExecuted, NavigateToProductsCanExecute);
            NavigateToSettingsCommand = new RelayCommand(NavigateToSettingsCommandExecuted, NavigateToSettingsCommandCanExecute);
            NavigateToCRMCommand = new RelayCommand(NavigateToCRMCommandExecuted, NavigateToCRMCommandCanExecute);
            NavigateToMembersCommand = new RelayCommand(NavigateToMembersCommandExecuted, NavigateToMembersCommandCanExecute);

        }

        #endregion

        #region Commands

        private void NavigateToResourcesCommandExecuted()
        {
            WindowContent = new ResourcesView();
        }

        private void NavigateToUsersCommandExecuted()
        {
            WindowContent = new UsersView();
        }

        private void NavigateToUserGroupsCommandExecuted()
        {
            WindowContent = new UserGroupsView();
        }

        private void NavigateToEventsCommandExecuted()
        {
            WindowContent = new EventsView();
        }

        private void NavigateToProductsExecuted()
        {
            WindowContent = new EPOSView();
        }

        private void NavigateToSettingsCommandExecuted()
        {
            WindowContent = new SettingsView();
        }

        private void NavigateToCRMCommandExecuted()
        {
            WindowContent = new CRMView();
        }

        private void NavigateToMembersCommandExecuted()
        {
            WindowContent = new MembersView();
        }

        private bool NavigateToResourcesCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_RESOURCES_TAB_ALLOWED);
        }

        private bool NavigateToUsersCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_USERS_TAB_ALLOWED);
        }

        private bool NavigateToUserGroupsCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_USERGROUPS_TAB_ALLOWED);
        }

        private bool NavigateToEventsCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_EVENTS_TAB_ALLOWED);
        }

        private bool NavigateToProductsCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_PRODUCTS_TAB_ALLOWED);
        }

        private bool NavigateToSettingsCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_CLUB_SETTINGS_TAB_ALLOWED);
        }

        private bool NavigateToCRMCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_CRM_ALLOWED);
        }

        private bool NavigateToMembersCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_MEMBERS_TAB_ALLOWED);
        }
        #endregion
    }
}