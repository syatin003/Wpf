using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using EventManagementSystem.ViewModels.CRM;
using EventManagementSystem.ViewModels.Events;

namespace EventManagementSystem.Converters
{
    public class DateDueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateDue = (DateTime)value;
            if (parameter != null && parameter.ToString() == "CRM")
            {
                var statuses = CRMViewModel.FollowUpStatuses.OrderByDescending(x => x.NumberOfDays);
                return GetStatusColor(dateDue, statuses);
            }
            else if (parameter != null && parameter.ToString() == "Event")
            {
                var statuses = EventsViewModel.ReminderStatuses.OrderByDescending(x => x.NumberOfDays);
                return GetStatusColor(dateDue, statuses);
            }
            return string.Empty;
        }

        private static object GetStatusColor(DateTime dateDue, IOrderedEnumerable<Data.Model.FollowUpStatus> statuses)
        {
            foreach (var status in statuses)
            {
                if ((dateDue.Date - DateTime.Today).TotalDays >= status.NumberOfDays)
                {
                    return status.Color;
                }
            }
            return statuses.Last().Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
