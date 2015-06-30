using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class MembershipUpdateModel : ModelBase
    {
        #region Fields

        private MembershipUpdate _membershipUpdate;
        private List<MembershipUpdatesHistoryModel> _membershipUpdatesHistory;

        #endregion Fields

        #region Properties

        public MembershipUpdate MembershipUpdate
        {
            get { return _membershipUpdate; }
            set
            {
                if (_membershipUpdate == value) return;
                _membershipUpdate = value;
                RaisePropertyChanged(() => MembershipUpdate);
            }
        }
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

        public string ContactName
        {
            get { return string.Concat(_membershipUpdate.Member.Contact.FirstName, " ", _membershipUpdate.Member.Contact.LastName); }
        }

        #endregion Properties

        #region Constructor

        public MembershipUpdateModel(MembershipUpdate membershipUpdate)
        {
            MembershipUpdate = membershipUpdate;
            MembershipUpdatesHistory = new List<MembershipUpdatesHistoryModel>();
        }
        #endregion Constructor

    }
}
