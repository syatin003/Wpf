using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class PermissionModel : ModelBase
    {
        #region Fields

        private readonly Permission _permission;
        private bool _isChecked;

        #endregion

        #region Properties

        public Permission Permission
        {
            get { return _permission; }
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

        #region Constructor

        public PermissionModel(Permission permission)
        {
            _permission = permission;
        }

        #endregion
    }
}
