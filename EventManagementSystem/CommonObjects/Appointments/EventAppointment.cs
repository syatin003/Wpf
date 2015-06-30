using EventManagementSystem.Models;
using Telerik.Windows.Controls.ScheduleView;

namespace EventManagementSystem.CommonObjects.Appointments
{
    public class EventAppointment : ColorfulAppointment
    {
        #region Fields

        private EventModel _event;

        #endregion

        #region Properties

        public EventModel Event
        {
            get { return this.Storage<EventAppointment>()._event; }
            set
            {
                var storage = this.Storage<EventAppointment>();
                if (storage._event != value)
                {
                    storage._event = value;
                    this.OnPropertyChanged(() => Event);
                }
            }
        }

        #endregion

        #region Methdos

        public override IAppointment Copy()
        {
            var newAppointment = new EventAppointment();
            newAppointment.CopyFrom(this);
            return newAppointment;
        }

        public override void CopyFrom(IAppointment other)
        {
            var eventAppointment = other as EventAppointment;
            if (eventAppointment != null)
            {
                this.Event = eventAppointment.Event;
            }
            base.CopyFrom(other);
        }

        #endregion
    }
}