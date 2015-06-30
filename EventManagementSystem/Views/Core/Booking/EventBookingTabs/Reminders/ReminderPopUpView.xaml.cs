using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Reminders;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders
{
    /// <summary>
    /// Interaction logic for FollowUpPopUp.xaml
    /// </summary>
    public partial class ReminderPopUpView : RadWindow
    {
        private readonly ReminderPopUpViewModel _viewModel;

        public ReminderPopUpView(EventReminderModel eventReminder)
        {
            InitializeComponent();
            DataContext = _viewModel = new ReminderPopUpViewModel(eventReminder);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;
            Loaded += OnFollowUpPopUpViewLoaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "EnableParentWindow")
            {
                this.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                this.IsEnabled = false;
            }
            else if (args.PropertyName == "CloseParentWindow")
            {
                Close();
            }
        }

        private void OnFollowUpPopUpViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }
        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnSnoozeButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
