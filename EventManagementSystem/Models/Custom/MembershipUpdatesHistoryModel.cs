using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Models.Custom
{
    [Serializable]
    public class MembershipUpdatesHistoryModel : ModelBase
    {
        #region Fields

        private MembershipUpdate _membershipUpdate;

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


        public string HistoryMessage
        {
            get
            {
                var message = "";
                switch (_membershipUpdate.Action)
                {
                    case 0:
                        message = string.Format("{0}: {1}", Enum.GetName(typeof(UpdateAction), _membershipUpdate.Action), _membershipUpdate.NewValue);
                        break;
                    case 1:
                        message = string.Format("{0}: Changed from {1} to {2}", Enum.GetName(typeof(UpdateAction), _membershipUpdate.Action), _membershipUpdate.OldValue, _membershipUpdate.NewValue);
                        break;
                    case 2:
                        message = string.Format("{0}: {1}", Enum.GetName(typeof(UpdateAction), _membershipUpdate.Action), _membershipUpdate.OldValue);
                        break;
                }
                return message;
            }
        }

        #endregion Properties
    }
}
