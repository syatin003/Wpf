using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Charges;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Charges
{
    /// <summary>
    /// Interaction logic for AddEventChargeView.xaml
    /// </summary>
    public partial class AddEventChargeView : RadWindow
    {
        public readonly AddEventChargeViewModel ViewModel;

        public AddEventChargeView(EventModel eventModel, EventChargeModel charge = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddEventChargeViewModel(eventModel, charge);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddEventChargeViewLoaded;
        }

        private void OnAddEventChargeViewLoaded(object sender, RoutedEventArgs routedEventArgs)
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
