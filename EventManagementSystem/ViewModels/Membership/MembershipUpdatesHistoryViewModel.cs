using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models.Custom;
using System.Collections.Generic;

namespace EventManagementSystem.ViewModels.Membership
{
    public class MembershipUpdatesHistoryViewModel : ViewModelBase
    {
        #region Fields

        private List<MembershipUpdatesHistoryModel> _membershipUpdatesHistory;

        #endregion Fields

        #region Properties

        public List<MembershipUpdatesHistoryModel> MembershipUpdatesHistory
        {
            get { return _membershipUpdatesHistory; }
            set
            {
                if (_membershipUpdatesHistory == value) return;
                _membershipUpdatesHistory = value;
                RaisePropertyChanged(() => MembershipUpdatesHistory);
            }
        }

        #endregion Properties

        #region Constructor

        public MembershipUpdatesHistoryViewModel(List<MembershipUpdatesHistoryModel> membershipUpdatesHistory)
        {
            MembershipUpdatesHistory = membershipUpdatesHistory;
        }

        #endregion Constructor
    }
}
