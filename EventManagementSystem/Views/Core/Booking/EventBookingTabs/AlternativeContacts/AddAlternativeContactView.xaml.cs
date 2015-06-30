using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.AlternativeContacts;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.AlternativeContacts
{
    /// <summary>
    /// Interaction logic for AddAlternativeContactView.xaml
    /// </summary>
    public partial class AddAlternativeContactView : RadWindow
    {
        public readonly AddAlternativeContactViewModel ViewModel;

        public AddAlternativeContactView(EventModel eventModel)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddAlternativeContactViewModel(eventModel);

            Owner = Application.Current.MainWindow;
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
