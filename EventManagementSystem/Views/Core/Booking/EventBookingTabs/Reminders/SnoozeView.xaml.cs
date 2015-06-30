using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Reminders;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders
{
    /// <summary>
    /// Interaction logic for SnoozeView.xaml
    /// </summary>
    public partial class SnoozeView : RadWindow
    {
        private readonly SnoozeViewModel _viewModel;

        public SnoozeView(EventReminderModel eventReminder)
        {
            InitializeComponent();
            DataContext = _viewModel = new SnoozeViewModel(eventReminder);
        }

        private void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            for (int i = Application.Current.Windows.Count - 1; i >= 0; i--)
            {
                if ((Equals(Application.Current.Windows[i].Title, "Event Reminder") || Equals(Application.Current.Windows[i].Title, "Snooze")))
                {
                    Application.Current.Windows[i].Close();
                }
            }
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
