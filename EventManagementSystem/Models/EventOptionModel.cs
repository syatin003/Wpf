using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class EventOptionModel : ModelBase
    {
        #region Fields

        private readonly EventOption _eventOption;
        private bool _isChecked;

        #endregion

        #region Properties

        public EventOption EventOption
        {
            get { return _eventOption; }
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

        public EventOptionModel(EventOption option)
        {
            _eventOption = option;
        }

        #endregion
    }
}
