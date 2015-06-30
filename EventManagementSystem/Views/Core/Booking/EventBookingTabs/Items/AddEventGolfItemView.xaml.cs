using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Items
{
    /// <summary>
    /// Interaction logic for AddEventGolfItemView.xaml
    /// </summary>
    public partial class AddEventGolfItemView : RadWindow
    {
        private readonly AddEventGolfItemViewModel _viewModel;

        public AddEventGolfItemView(EventModel eventModel, EventGolfModel golfModel = null, System.Collections.Generic.List<EventGolfModel> alreadyBookedGolfs = null)
        {
            InitializeComponent();
            DataContext = _viewModel = new AddEventGolfItemViewModel(eventModel, golfModel, alreadyBookedGolfs);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnAddEventGolfItemViewLoaded;
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

        private void OnAddEventGolfItemViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
