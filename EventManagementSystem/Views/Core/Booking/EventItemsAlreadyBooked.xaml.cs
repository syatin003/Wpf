using System.Windows;
using EventManagementSystem.ViewModels.Core.Booking;
using Telerik.Windows.Controls;
//using System.Collections.Generic;
using System.ComponentModel;
using EventManagementSystem.Models;
using System.Collections.Generic;

namespace EventManagementSystem.Views.Core.Booking
{
    /// <summary>
    /// Interaction logic for MailFieldsView.xaml
    /// </summary>
    public partial class EventItemsAlreadyBooked : RadWindow
    {
        public readonly EventItemsAlreadyBookedViewModel ViewModel;

        public EventItemsAlreadyBooked(Models.EventModel Event, List<EventCateringModel> alreadyBookedCaterings, List<EventRoomModel> alreadyBookedRooms, List<EventGolfModel> alreadyBookedGolfs, System.Collections.ObjectModel.ObservableCollection<EventItemModel> alreadyBookedEventItems)
        {
            InitializeComponent();
            DataContext = ViewModel = new EventItemsAlreadyBookedViewModel(Event, alreadyBookedCaterings, alreadyBookedRooms, alreadyBookedGolfs, alreadyBookedEventItems);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            Owner = Application.Current.MainWindow;

            //Loaded += EventItemsAlreadyBooked_Loaded;
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

        //private void EventItemsAlreadyBooked_Loaded(object sender, RoutedEventArgs routedEventArgs)
        //{
        //    //ViewModel.LoadData();
        //}

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        //private void InsertButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    DialogResult = true;
        //    Close();
        //}
    }
}
