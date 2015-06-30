using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Payments;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Payments
{
    /// <summary>
    /// Interaction logic for AddEventPaymentView.xaml
    /// </summary>
    public partial class AddEventPaymentView : RadWindow
    {
        public readonly AddEventPaymentViewModel ViewModel;

        public AddEventPaymentView(EventModel Event, EventPaymentModel model = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddEventPaymentViewModel(Event, model);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddEventPaymentViewLoaded;
        }

        private void OnAddEventPaymentViewLoaded(object sender, RoutedEventArgs routedEventArgs)
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
