using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Notes;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Notes
{
    /// <summary>
    /// Interaction logic for AddNoteView.xaml
    /// </summary>
    public partial class AddNoteView : RadWindow
    {
        public readonly AddNoteViewModel ViewModel;

        public AddNoteView(EventModel eventModel, EventNoteModel note = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddNoteViewModel(eventModel, note);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddNoteViewLoaded;
        }

        private void OnAddNoteViewLoaded(object sender, RoutedEventArgs routedEventArgs)
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
