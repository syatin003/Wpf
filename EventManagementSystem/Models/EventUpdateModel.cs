using System.Collections.Generic;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models.Custom;

namespace EventManagementSystem.Models
{
    public class EventUpdateModel : ModelBase
    {
        #region Fields

        private EventUpdate _eventUpdate;
        private List<UpdatesHistoryModel> _updatesHistory;

        #endregion Fields

        #region Properties

        public EventUpdate EventUpdate
        {
            get { return _eventUpdate; }
            set
            {
                if (_eventUpdate == value) return;
                _eventUpdate = value;
                RaisePropertyChanged(() => EventUpdate);
            }
        }
        public List<UpdatesHistoryModel> UpdatesHistory
        {
            get { return _updatesHistory; }
            set
            {
                if (_updatesHistory == value) return;
                _updatesHistory = value;
                RaisePropertyChanged(() => UpdatesHistory);
            }
        }
        #endregion Properties

        #region Constructor

        public EventUpdateModel(EventUpdate eventUpdate)
        {
            EventUpdate = eventUpdate;
            UpdatesHistory=new List<UpdatesHistoryModel>();
        }
        #endregion Constructor

    }
}
