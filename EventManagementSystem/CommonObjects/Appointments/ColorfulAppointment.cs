    using System.Windows.Media;
using Telerik.Windows.Controls.ScheduleView;

namespace EventManagementSystem.CommonObjects.Appointments
{
    public class ColorfulAppointment : Appointment
    {
        #region Fields

        private Brush _color;

        #endregion

        #region Properties

        public Brush Color
        {
            get { return this.Storage<ColorfulAppointment>()._color; }
            set
            {
                var storage = this.Storage<ColorfulAppointment>();
                if (storage._color != value)
                {
                    storage._color = value;
                    this.OnPropertyChanged(() => Color);
                }
            }
        }

        #endregion
    }
}
