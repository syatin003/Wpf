using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventOptionsViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;

        #endregion

        #region Properties

        public EventModel Event
        {
            get { return _event; }
            set
            {
                if (_event == value) return;
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }

        #endregion
    }
}
