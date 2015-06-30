using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Reminders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders
{
    /// <summary>
    /// Interaction logic for UpdatedReminders.xaml
    /// </summary>
    public partial class UpdatedRemindersView : RadWindow
    {
        public readonly UpdatedRemindersViewModel ViewModel;

        public UpdatedRemindersView(ObservableCollection<EventReminderModel> eventReminders)
        {
            InitializeComponent();
            DataContext = ViewModel = new UpdatedRemindersViewModel(eventReminders);

            Owner = Application.Current.MainWindow;

        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
