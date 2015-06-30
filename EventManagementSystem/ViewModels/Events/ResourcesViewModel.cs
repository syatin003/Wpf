using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking;
using EventManagementSystem.Views.Core.Booking;
using Microsoft.Practices.Unity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace EventManagementSystem.ViewModels.Events
{
    public class ResourcesViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;
        private ObservableCollection<RoomModel> _rooms;
        private ObservableCollection<GolfModel> _golfs;
        private DateTime _selectedDate;
        private bool _isDataLoadedOnce;

        #endregion

        #region Properties

        public List<EventRoomModel> RoomBookings { get; set; }
        public List<EventGolfModel> GolfBookings { get; set; }
        public List<EventCateringModel> CateringsBookings { get; set; }

        public bool IsEventSaved { get; set; }

        public ObservableCollection<RoomModel> Rooms
        {
            get { return _rooms; }
            set
            {
                if (_rooms == value) return;
                _rooms = value;
                RaisePropertyChanged(() => Rooms);
            }
        }

        public ObservableCollection<GolfModel> Golfs
        {
            get { return _golfs; }
            set
            {
                if (_golfs == value) return;
                _golfs = value;
                RaisePropertyChanged(() => Golfs);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public bool IsDataLoadedOnce
        {
            get { return _isDataLoadedOnce; }
            set
            {
                if (_isDataLoadedOnce == value) return;
                _isDataLoadedOnce = value;
                RaisePropertyChanged(() => IsDataLoadedOnce);
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                RaisePropertyChanged(() => SelectedDate);
                if (IsDataLoadedOnce)
                    Refresh();
            }
        }

        public RelayCommand CreateEventCommand { get; private set; }

        #endregion

        #region Constructors

        public ResourcesViewModel(DateTime selectedDate)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
            SelectedDate = selectedDate;
            CreateEventCommand = new RelayCommand(CreateEventCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _eventsDataUnit.GolfsRepository.Refresh(RefreshMode.ClientWins);
            var golfs = await _eventsDataUnit.GolfsRepository.GetAllAsync();
            Golfs = new ObservableCollection<GolfModel>(golfs.Select(x => new GolfModel(x)));

            _eventsDataUnit.RoomsRepository.Refresh(RefreshMode.ClientWins);
            var rooms = await _eventsDataUnit.RoomsRepository.GetAllAsync();
            Rooms = new ObservableCollection<RoomModel>(rooms.Select(x => new RoomModel(x)));

            _eventsDataUnit.EventGolfsRepository.Refresh(RefreshMode.ClientWins);
            var eventGolfs = await _eventsDataUnit.EventGolfsRepository.GetAllAsync(eGolfs => !eGolfs.Event.IsDeleted && eGolfs.Event.Date == SelectedDate.Date);
            eventGolfs = _eventsDataUnit.EventGolfsRepository.SetGolfCurrentValues(eventGolfs).ToList();
            GolfBookings = new List<EventGolfModel>(eventGolfs.Select(x => new EventGolfModel(x)));

            _eventsDataUnit.EventRoomsRepository.Refresh(RefreshMode.ClientWins);
            var eventRooms = await _eventsDataUnit.EventRoomsRepository.GetAllAsync(eRoom => !eRoom.Event.IsDeleted && eRoom.Event.Date == SelectedDate.Date);
            eventRooms = _eventsDataUnit.EventRoomsRepository.SetRoomsCurrentValues(eventRooms).ToList();
            RoomBookings = new List<EventRoomModel>(eventRooms.Select(x => new EventRoomModel(x)));

            _eventsDataUnit.EventCateringsRepository.Refresh(RefreshMode.ClientWins);
            var eventCaterings = await _eventsDataUnit.EventCateringsRepository.GetAllAsync(eCatering => !eCatering.Event.IsDeleted && eCatering.Event.Date == SelectedDate.Date);
            eventCaterings = _eventsDataUnit.EventCateringsRepository.SetCateringsCurrentValues(eventCaterings).ToList();
            CateringsBookings = new List<EventCateringModel>(eventCaterings.Select(x => new EventCateringModel(x)));

            RaisePropertyChanged("OnDataLoaded");
            IsDataLoadedOnce = true;
        }

        public async void Refresh()
        {
            IsBusy = true;

            _eventsDataUnit.EventGolfsRepository.Refresh(RefreshMode.ClientWins);
            var eventGolfs = await _eventsDataUnit.EventGolfsRepository.GetAllAsync(eGolfs => !eGolfs.Event.IsDeleted && eGolfs.Event.Date == SelectedDate.Date);
            eventGolfs = _eventsDataUnit.EventGolfsRepository.SetGolfCurrentValues(eventGolfs).ToList();
            GolfBookings = new List<EventGolfModel>(eventGolfs.Select(x => new EventGolfModel(x)));

            _eventsDataUnit.EventRoomsRepository.Refresh(RefreshMode.ClientWins);
            var eventRooms = await _eventsDataUnit.EventRoomsRepository.GetAllAsync(eRoom => !eRoom.Event.IsDeleted && eRoom.Event.Date == SelectedDate.Date);
            eventRooms = _eventsDataUnit.EventRoomsRepository.SetRoomsCurrentValues(eventRooms).ToList();
            RoomBookings = new List<EventRoomModel>(eventRooms.Select(x => new EventRoomModel(x)));

            _eventsDataUnit.EventCateringsRepository.Refresh(RefreshMode.ClientWins);
            var eventCaterings = await _eventsDataUnit.EventCateringsRepository.GetAllAsync(eCatering => !eCatering.Event.IsDeleted && eCatering.Event.Date == SelectedDate.Date);
            eventCaterings = _eventsDataUnit.EventCateringsRepository.SetCateringsCurrentValues(eventCaterings).ToList();
            CateringsBookings = new List<EventCateringModel>(eventCaterings.Select(x => new EventCateringModel(x)));

            RaisePropertyChanged("OnDataLoaded");
        }

        #endregion

        #region Commands

        private void CreateEventCommandExecuted()
        {
            Application.Current.Resources["SelectedEventStart"] = SelectedDate;

            RaisePropertyChanged("DisableParentWindow");

            var view = new BookingView(BookingViews.Event);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            IsEventSaved = view.DialogResult != null && view.DialogResult.Value;
        }

        #endregion
    }
}
