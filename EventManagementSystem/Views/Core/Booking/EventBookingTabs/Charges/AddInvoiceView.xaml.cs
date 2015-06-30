using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Charges;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Charges
{
    /// <summary>
    /// Interaction logic for AddInvoiceView.xaml
    /// </summary>
    public partial class AddInvoiceView : RadWindow
    {
        private readonly AddInvoiceViewModel _viewModel;

        public AddInvoiceView(EventModel eventModel)
        {
            InitializeComponent();
            DataContext = _viewModel = new AddInvoiceViewModel(eventModel);

            Owner = Application.Current.MainWindow;
        }
    }
}
