using EventManagementSystem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Models.Custom;

namespace EventManagementSystem.ViewModels.Events
{
    public class UpdatesHistoryViewModel : ViewModelBase
    {
        #region Fields

        private List<UpdatesHistoryModel> _updatesHistory;

        #endregion Fields

        #region Properties

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

        public UpdatesHistoryViewModel(List<UpdatesHistoryModel> updatesHistory)
        {
            UpdatesHistory = updatesHistory;
        }
        #endregion Constructor
    }
}
