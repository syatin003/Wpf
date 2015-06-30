using System.Collections.ObjectModel;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class PermissionGroupModel : ModelBase
    {
        #region Fields

        private readonly PermissionGroup _permissionGroup;
        private ObservableCollection<PermissionModel> _permissions;
        private bool _isChecked;

        #endregion

        #region Properties

        public PermissionGroup PermissionGroup
        {
            get { return _permissionGroup; }
        }

        public string Name
        {
            get { return _permissionGroup.GroupName; }
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

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        #endregion

        #region Constructors

        public PermissionGroupModel(PermissionGroup permissionGroup)
        {
            _permissionGroup = permissionGroup;
        }

        #endregion
    }
}
