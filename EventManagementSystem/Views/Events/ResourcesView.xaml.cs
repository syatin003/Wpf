using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Events;
using EventManagementSystem.Views.ContactManager.ContactManagerTabs;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;


namespace EventManagementSystem.Views.Events
{
    /// <summary>
    /// Interaction logic for ResourcesView.xaml
    /// </summary>
    public partial class ResourcesView : RadWindow
    {
        public readonly ResourcesViewModel ViewModel;

        public ResourcesView()
        {
            InitializeComponent();
            DataContext = ViewModel = new ResourcesViewModel(DateTime.Now);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;
            Loaded += OnResourcesViewLoaded;
        }

        public ResourcesView(DateTime date)
        {
            InitializeComponent();
            DataContext = ViewModel = new ResourcesViewModel(date);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;
            Loaded += OnResourcesViewLoaded;
        }

        private async void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "EnableParentWindow" || args.PropertyName == "DisableParentWindow")
            {
                var tile = this.ParentOfType<RadTileView>();

                if (tile != null)
                    tile.IsEnabled = args.PropertyName == "EnableParentWindow";
                else
                    this.IsEnabled = args.PropertyName == "EnableParentWindow";
            }

            if (args.PropertyName == "OnDataLoaded")
            {
                await Dispatcher.BeginInvoke((Action)(CheckGolfBookings));
                await Dispatcher.BeginInvoke((Action)(CheckRoomBookings));

                ViewModel.IsBusy = false;

            }
        }

        private void OnResourcesViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void CheckRoomBookings()
        {
            if (BookingsCalendar.SelectedDate == null) return;

            var checkDate = ViewModel.SelectedDate;

            var displayRooms = RoomItemsControl.ChildrenOfType<RadClock>().ToList();


            var bookingsService = new BookingsService()
            {
                BookedRooms = ViewModel.RoomBookings,
                BookedCaterings = ViewModel.CateringsBookings
            };
            if (bookingsService.BookedRooms.Count != 0 || bookingsService.BookedCaterings.Count != 0)
            {
                foreach (var room in ViewModel.Rooms)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        var displayRoom = displayRooms.FirstOrDefault(x => (string)x.Header == room.Name);

                        var roomTimeValues = displayRoom.ChildrenOfType<TextBlock>().ToList();
                        roomTimeValues.RemoveAt(0); // delete header TextBlock

                        foreach (TextBlock roomTimeValue in roomTimeValues)
                        {
                            int hours = Convert.ToInt32(roomTimeValue.Text.Substring(0, 2));
                            int minutes = Convert.ToInt32(roomTimeValue.Text.Substring(3, 2));

                            var value = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, hours, minutes, 0);

                            if (room.Room.EndTime < room.Room.StartTime)
                            {
                                if ((new TimeSpan(hours, minutes, 0)).Ticks < (new TimeSpan(room.Room.EndTime.Hours, room.Room.EndTime.Minutes, room.Room.EndTime.Seconds)).Ticks)
                                {
                                    value = value.AddDays(1);
                                }
                            }
                            var isAvailable = bookingsService.IsRoomAvailable(room.Room, value);

                            if (isAvailable)
                            {
                                roomTimeValue.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
                                roomTimeValue.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#111111"));
                            }
                            else
                            {
                                roomTimeValue.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#2980b9"));
                                roomTimeValue.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
                            }
                        }
                    }));
                }
            }
            else
            {
                foreach (var room in ViewModel.Rooms)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        var displayRoom = displayRooms.FirstOrDefault(x => (string)x.Header == room.Name);

                        var roomTimeValues = displayRoom.ChildrenOfType<TextBlock>().ToList();
                        roomTimeValues.RemoveAt(0); // delete header TextBlock

                        foreach (TextBlock roomTimeValue in roomTimeValues)
                        {
                            roomTimeValue.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
                            roomTimeValue.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#111111"));
                        }
                    }));

                }
            }
        }

        private void CheckGolfBookings()
        {
            if (BookingsCalendar.SelectedDate == null) return;

            var checkDate = ViewModel.SelectedDate;

            var displayGolfs = GolfItemsControl.ChildrenOfType<RadClock>().ToList();

            var bookingsService = new BookingsService()
            {
                BookedGolfs = ViewModel.GolfBookings
            };
            if (bookingsService.BookedGolfs.Count != 0)
            {
                foreach (var golf in ViewModel.Golfs)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        var displayGolf = displayGolfs.FirstOrDefault(x => (string)x.Header == golf.Name);

                        var golfTimeValues = displayGolf.ChildrenOfType<TextBlock>().ToList();
                        golfTimeValues.RemoveAt(0); // delete header TextBlock

                        foreach (TextBlock golfTimeValue in golfTimeValues)
                        {
                            int hours = Convert.ToInt32(golfTimeValue.Text.Substring(0, 2));
                            int minutes = Convert.ToInt32(golfTimeValue.Text.Substring(3, 2));

                            var startTime = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, hours, minutes, 0);

                            var isAvailable = bookingsService.IsGolfAvailable(golf.Golf, startTime);

                            if (isAvailable)
                            {
                                golfTimeValue.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
                                golfTimeValue.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#111111"));
                            }
                            else
                            {
                                golfTimeValue.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#2980b9"));
                                golfTimeValue.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
                            }
                        }
                    }));
                }
            }
            else
            {
                foreach (var golf in ViewModel.Golfs)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                     {
                         var displayGolf = displayGolfs.FirstOrDefault(x => (string)x.Header == golf.Name);

                         var golfTimeValues = displayGolf.ChildrenOfType<TextBlock>().ToList();
                         golfTimeValues.RemoveAt(0); // delete header TextBlock

                         foreach (TextBlock golfTimeValue in golfTimeValues)
                         {
                             golfTimeValue.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
                             golfTimeValue.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#111111"));
                         }
                     }));
                }
            }
        }

        private void RoomClock_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var checkDate = ViewModel.SelectedDate;

            var time = e.AddedItems[0].ToString();
            int hours = Convert.ToInt32(time.Substring(0, 2));
            int minutes = Convert.ToInt32(time.Substring(3, 2));

            var dateTime = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, hours, minutes, 0);

            var roomName = (string)(sender as RadClock).Header;

            var bookingsService = new BookingsService()
            {
                BookedRooms = ViewModel.RoomBookings,
                BookedCaterings = ViewModel.CateringsBookings,
                BookedGolfs = ViewModel.GolfBookings
            };

            var events = bookingsService.GetModelsByRoom(roomName, dateTime).Distinct().ToList();

            if (events.Any())
            {
                this.IsEnabled = false;

                // TODO: If current date has several events ?     //Done
                if (events.Count == 1)
                {
                    var view = new EventDetailsView(new EventModel(events.First()));
                    view.ShowDialog();
                }
                else
                {
                    var view = new EventsBookedView(new ObservableCollection<EventModel>(events.Select(p => new EventModel(p))));
                    view.ShowDialog();
                }
                this.IsEnabled = true;

                //ViewModel.Refresh();
            }
        }

        private void GolfClock_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var checkDate = ViewModel.SelectedDate;

            var time = e.AddedItems[0].ToString();
            int hours = Convert.ToInt32(time.Substring(0, 2));
            int minutes = Convert.ToInt32(time.Substring(3, 2));

            var dateTime = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, hours, minutes, 0);

            var roomName = (string)(sender as RadClock).Header;

            var bookingsService = new BookingsService()
            {
                BookedRooms = ViewModel.RoomBookings,
                BookedCaterings = ViewModel.CateringsBookings,
                BookedGolfs = ViewModel.GolfBookings
            };

            var _event = bookingsService.GetModelByGolf(roomName, dateTime);

            if (_event != null)
            {
                this.IsEnabled = false;

                var view = new EventDetailsView(new EventModel(_event));
                view.ShowDialog();

                this.IsEnabled = true;

                // ViewModel.Refresh();
            }

        }

    }
}