using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking
{
    /// <summary>
    /// Interaction logic for EventBookingView.xaml
    /// </summary>
    public partial class EventBookingView : UserControl
    {
        public EventBookingViewModel ViewModel { get; private set; }

        public EventBookingView(EventModel eventModel, bool IsDuplicate=false)
        {
            InitializeComponent();
            DataContext = ViewModel = new EventBookingViewModel(eventModel,IsDuplicate);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnEventBookingViewLoaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var window = RadWindow.GetParentRadWindow(this);
            if (args.PropertyName == "CloseDialog")
            {
                window.DialogResult = true;
                window.Close();
            }
            else if (args.PropertyName == "EnableParentWindow")
            {
                window.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                window.IsEnabled = false;
            }
        }

        private void OnEventBookingViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnEventBookingViewLoaded;
            ViewModel.LoadData();
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            var window = RadWindow.GetParentRadWindow(this);

            window.DialogResult = true;
            window.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            var window = RadWindow.GetParentRadWindow(this);

            window.DialogResult = false;
            window.Close();
        }
    }
}