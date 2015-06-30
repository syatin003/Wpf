using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items;
using Telerik.Windows.Controls;
using System.Collections.Generic;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Items
{
    /// <summary>
    /// Interaction logic for AddCateringItemView.xaml
    /// </summary>
    public partial class AddCateringItemView : RadWindow
    {
        private readonly AddCateringItemViewModel _viewModel;

        public AddCateringItemView(EventModel Event, EventCateringModel eventCatering = null, List<EventCateringModel> alreadyBookedCaterings = null, List<EventRoomModel> alreadyBookedRooms = null)
        {
            InitializeComponent();

            DataContext = _viewModel = new AddCateringItemViewModel(Event, eventCatering, alreadyBookedCaterings, alreadyBookedRooms);
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnAddCateringItemViewLoaded;
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

        private void OnAddCateringItemViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnAddCateringItemViewLoaded;
            _viewModel.LoadData();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
