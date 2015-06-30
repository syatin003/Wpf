using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Events;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Events
{
    /// <summary>
    /// Interaction logic for AddCalendarNoteView.xaml
    /// </summary>
    public partial class AddCalendarNoteView : RadWindow
    {
        public AddCalendarNoteViewModel ViewModel { get; private set; }

        public AddCalendarNoteView(CalendarNoteModel model = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddCalendarNoteViewModel(model);

            Owner = Application.Current.MainWindow;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close(); 
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
