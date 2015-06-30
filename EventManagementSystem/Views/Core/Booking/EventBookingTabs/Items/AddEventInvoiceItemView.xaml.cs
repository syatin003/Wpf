using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Items
{
    /// <summary>
    /// Interaction logic for AddEventInvoiceItemView.xaml
    /// </summary>
    public partial class AddEventInvoiceItemView : RadWindow
    {
        private readonly AddEventInvoiceItemViewModel _viewModel;

        public AddEventInvoiceItemView(EventModel Event, EventInvoiceModel invoiceModel = null)
        {
            InitializeComponent();
            DataContext = _viewModel = new AddEventInvoiceItemViewModel(Event, invoiceModel);
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnAddEventInvoiceItemViewLoaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
            else if (args.PropertyName == "EnableParentWindow")
            {
                this.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                this.IsEnabled = false;
            }
        }

        private void OnAddEventInvoiceItemViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
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
