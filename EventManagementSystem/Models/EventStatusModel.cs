using System.Collections.ObjectModel;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class EventStatusModel : ModelBase
    {
        #region Fields

        private readonly EventStatus _eventStatus;
        private ObservableCollection<EventOptionModel> _options;

        #endregion

        #region Properties

        public EventStatus EventStatus
        {
            get { return _eventStatus; }
        }

        public ObservableCollection<EventOptionModel> Options
        {
            get { return _options; }
            set
            {
                if (_options == value) return;
                _options = value;
                RaisePropertyChanged(() => Options);
            }
        }

        #endregion

        #region Constructors

        public EventStatusModel(EventStatus status)
        {
            _eventStatus = status;
        }

        #endregion
    }
}
