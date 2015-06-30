using EventManagementSystem.Models;

namespace EventManagementSystem.CommonObjects.Appointments
{
    class CalendarNoteAppointment : ColorfulAppointment
    {
        #region Fields

        private CalendarNoteModel _calendarNote;

        #endregion

        #region Properties

        public CalendarNoteModel CalendarNote
        {
            get { return this.Storage<CalendarNoteAppointment>()._calendarNote; }
            set
            {
                var storage = this.Storage<CalendarNoteAppointment>();
                if (storage._calendarNote != value)
                {
                    storage._calendarNote = value;
                    this.OnPropertyChanged(() => CalendarNote);
                }
            }
        }

        #endregion
    }
}
