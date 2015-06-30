using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items;
using Telerik.Windows.Controls;
using System.Collections.Generic;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Items
{
    /// <summary>
    /// Interaction logic for AddRoomItemView.xaml
    /// </summary>
    public partial class AddRoomItemView : RadWindow
    {
        private readonly AddRoomItemViewModel _viewModel;

        public AddRoomItemView(EventModel Event, EventRoomModel roomModel = null, List<EventCateringModel> alreadyBookedCaterings = null, List<EventRoomModel> alreadyBookedRooms = null)
        {
            InitializeComponent();
            DataContext = _viewModel = new AddRoomItemViewModel(Event, roomModel, alreadyBookedCaterings, alreadyBookedRooms);
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnAddRoomItemViewLoaded;
        }

        private void OnAddRoomItemViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
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

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
