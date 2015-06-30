using System.Windows;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.ViewModels.Core.Booking;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking
{
    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : RadWindow
    {
        public readonly BookingViewModel ViewModel;

        public BookingView(BookingViews type, ModelBase model = null, bool IsDuplicate = false)
        {
            InitializeComponent();
            if (model != null)
                this.Header = "Edit Booking";
            DataContext = ViewModel = new BookingViewModel(type, model,IsDuplicate);

            Owner = Application.Current.MainWindow;
        }
    }
}
