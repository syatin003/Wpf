using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Documents.Flow.Model.Shapes;

namespace EventManagementSystem.Models.Custom
{
    public class UpdatesHistoryModel : ModelBase
    {
        #region Fields

        private EventUpdate _eventUpdate;

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


        public string HistoryMessage
        {
            get
            {
                var message = "";
                switch (_eventUpdate.Action)
                {
                    case UpdateAction.Added:
                        message = string.Format("{0}: {1}", _eventUpdate.Action, _eventUpdate.NewValue);
                        break;
                    case UpdateAction.Edited:
                        message = string.Format("{0}: Changed from {1} to {2}", _eventUpdate.Action, _eventUpdate.OldValue, _eventUpdate.NewValue);
                        break;
                    case UpdateAction.Removed:
                        message = string.Format("{0}: {1}", _eventUpdate.Action, _eventUpdate.OldValue);
                        break;
                }
                return message;
            }
        }

        #endregion Properties
    }
}
