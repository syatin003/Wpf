using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using Telerik.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Reminders;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders
{
    /// <summary>
    /// Interaction logic for AddEventReminderView.xaml
    /// </summary>
    public partial class AddEventReminderView : RadWindow
    {
        public readonly AddEventReminderViewModel ViewModel;

        public AddEventReminderView(EventReminderModel eventReminder = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddEventReminderViewModel(eventReminder);
            if (eventReminder != null)
                this.Header = "Edit Event Reminder";
            Owner = Application.Current.MainWindow;
            Loaded += OnAddEventReminderViewLoaded;
        }

        public AddEventReminderView(EventModel eventModel, EventReminderModel eventReminder = null)
        {
            InitializeComponent();
            if (eventReminder != null)
                this.Header = "Edit Event Reminder";
            DataContext = ViewModel = new AddEventReminderViewModel(eventModel, eventReminder);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddEventReminderViewLoaded;
        }

        public AddEventReminderView(List<EventModel> events, EventReminderModel eventReminder = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddEventReminderViewModel(events, eventReminder);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddEventReminderViewLoaded;
        }
        private void OnAddEventReminderViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
