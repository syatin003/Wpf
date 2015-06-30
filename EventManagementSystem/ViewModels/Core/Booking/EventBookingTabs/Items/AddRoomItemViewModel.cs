using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Admin.EPOS.Products;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using System.Data.Entity.Core.Objects;
using EventManagementSystem.Core.Serialization;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;


namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items
{
    public class AddRoomItemViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private readonly EventModel _event;
        private ObservableCollection<Room> _rooms;
        private bool _isBusy;
        private ObservableCollection<Product> _products;
        private EventRoomModel _eventRoom;
        private EventRoomModel _eventRoomOriginal;
        private List<EventCateringModel> _alreadyBookedCaterings;
        private List<EventRoomModel> _alreadyBookedRooms;
        private bool _isEditMode;
        private ObservableCollection<TimeSpan> _clockItems;


        #endregion

        #region Properties

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

        public ObservableCollection<Room> Rooms
        {
            get { return _rooms; }
            set
            {
                if (_rooms == value) return;
                _rooms = value;
                RaisePropertyChanged(() => Rooms);
            }
        }

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                if (_products == value) return;
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        public EventRoomModel EventRoom
        {
            get { return _eventRoom; }
            set
            {
                if (_eventRoom == value) return;
                _eventRoom = value;
                RaisePropertyChanged(() => EventRoom);
            }
        }
        public EventRoomModel EventRoomOriginal
        {
            get { return _eventRoomOriginal; }
            set
            {
                if (_eventRoomOriginal == value) return;
                _eventRoomOriginal = value;
                RaisePropertyChanged(() => EventRoomOriginal);
            }
        }
        public List<EventCateringModel> AlreadyBookedCaterings
        {
            get { return _alreadyBookedCaterings; }
            set
            {
                if (_alreadyBookedCaterings == value) return;
                _alreadyBookedCaterings = value;
                RaisePropertyChanged(() => AlreadyBookedCaterings);
            }
        }
        public List<EventRoomModel> AlreadyBookedRooms
        {
            get { return _alreadyBookedRooms; }
            set
            {
                if (_alreadyBookedRooms == value) return;
                _alreadyBookedRooms = value;
                RaisePropertyChanged(() => AlreadyBookedRooms);
            }
        }
        public ObservableCollection<TimeSpan> ClockItems
        {
            get
            {
                return _clockItems;
            }
            set
            {
                if (_clockItems == value) return;
                _clockItems = value;
                RaisePropertyChanged(() => ClockItems);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand AddItemCommand { get; private set; }
        public RelayCommand AddProductCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand<EventBookedProductModel> DeleteBookedProductCommand { get; private set; }

        #endregion

        #region Constructors

        public AddRoomItemViewModel(EventModel eventModel, EventRoomModel roomModel, List<EventCateringModel> alreadyBookedCaterings, List<EventRoomModel> alreadyBookedRooms)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            AddItemCommand = new RelayCommand(AddItemCommandExecuted);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
            AddProductCommand = new RelayCommand(AddProductCommandExecuted);
            DeleteBookedProductCommand = new RelayCommand<EventBookedProductModel>(DeleteBookedProductCommandExecuted);

            AlreadyBookedCaterings = alreadyBookedCaterings;
            AlreadyBookedRooms = alreadyBookedRooms;

            ProcessEventRoom(roomModel);
        }

        private void ProcessEventRoom(EventRoomModel roomModel)
        {
            _isEditMode = (roomModel != null);

            EventRoom = (_isEditMode) ? roomModel : GetEventRoom();
            if (_isEditMode)
            {
                CreateClockItems();
                EventRoomOriginal = EventRoom.Clone();
            }
            EventRoom.PropertyChanged += OnEventBookedProductModelPropertyChanged;
        }

        #endregion

        #region Methods

        private EventRoomModel GetEventRoom()
        {
            var eventRoom = new EventRoomModel(new EventRoom
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                StartTime = _event.Date,
                EndTime = _event.Date,
                ShowInInvoice = true,
                IncludeInCorrespondence = true,
                IncludeInForwardBook = true
            });

            return eventRoom;
        }

        private void AddRoomProduct(EventRoomModel model)
        {
            var charge = new EventCharge
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                ShowInInvoice = model.EventRoom.ShowInInvoice
            };

            var item = new EventBookedProductModel(new EventBookedProduct
            {
                ID = Guid.NewGuid(),
                EventBookingItemID = model.EventRoom.ID,
                EventID = _event.Event.ID,
                EventCharge = charge
            });

            item.Quantity = _event.Event.Places;
            item.PropertyChanged += OnEventBookedProductModelPropertyChanged;

            model.EventBookedProducts.Add(item);
        }

        private void OnEventBookedProductModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Room")
                CreateClockItems();

            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            _eventDataUnit.RoomsRepository.Refresh(RefreshMode.ClientWins);
            var rooms = await _eventDataUnit.RoomsRepository.GetAllAsync();
            Rooms = new ObservableCollection<Room>(rooms);

            var products = await _eventDataUnit.ProductsRepository.GetAllAsync();

            if (_event.EventType == null)
            {
                Products = new ObservableCollection<Product>(products
                    .Where(x => x.ProductOption.OptionName == "Room")
                    .OrderBy(x => x.Name)
                    .ToList());
            }
            else
            {
                Products = new ObservableCollection<Product>(products
                    .Where(x => x.ProductEventTypes.Any(e => e.EventType.ID == _event.EventType.ID))
                    .Where(x => x.ProductOption.OptionName == "Room")
                    .OrderBy(x => x.Name)
                    .ToList());
            }

            IsBusy = false;
        }
        private void CreateClockItems()
        {
            ClockItems = new ObservableCollection<TimeSpan>();
            var startTime = EventRoom.Room.StartTime;
            var endTime = EventRoom.Room.EndTime;
            var timeInterval = EventRoom.Room.TimeInterval;
            var clockItem = new TimeSpan(startTime.Hours, startTime.Minutes, startTime.Seconds);
            ClockItems.Add(clockItem);

            if (startTime < endTime)
            {
                Int32 slots = Convert.ToInt32((endTime.Ticks - startTime.Ticks) / timeInterval.Ticks);
                for (int i = 0; i < slots; i++)
                {
                    clockItem = clockItem.Add(EventRoom.Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
            }
            else
            {
                var endOfTheDay = new TimeSpan(24, 0, 0);
                Int32 slotsBeforeEndOfDay = Convert.ToInt32((endOfTheDay.Ticks - startTime.Ticks) / timeInterval.Ticks);
                for (int i = 0; i < slotsBeforeEndOfDay; i++)
                {
                    clockItem = clockItem.Add(EventRoom.Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
                Int32 slotsAfterEndOfDay = Convert.ToInt32(endTime.Ticks / timeInterval.Ticks);
                clockItem = new TimeSpan();
                for (int i = 0; i < slotsAfterEndOfDay; i++)
                {
                    clockItem = clockItem.Add(EventRoom.Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
            }
        }
        #endregion

        #region Commands

        private async void SubmitCommandExecuted()
        {
            SetLoadingIndicator(true);
            if (EventRoom.HasErrors)
            {
                SetLoadingIndicator(false);
                return;
            }
            var eventRoom = EventRoom.Clone();

            EventRoom.EventRoom.Event = _event.Event;

            _eventDataUnit.EventRoomsRepository.Refresh(RefreshMode.ClientWins);
            var rooms = await _eventDataUnit.EventRoomsRepository.GetAllAsync(eRoom =>
                !eRoom.Event.IsDeleted
                && eRoom.EventID != _event.Event.ID
                && eRoom.Event.Date == _event.Event.Date
                && eRoom.RoomID == EventRoom.Room.ID);
            rooms = _eventDataUnit.EventRoomsRepository.SetRoomsCurrentValues(rooms).ToList();
            var eventRooms = rooms.Select(x => new EventRoomModel(x)).ToList();
            if (AlreadyBookedRooms != null)
            {
                AlreadyBookedRooms.ForEach(alreadyBookedItem =>
                    {
                        eventRooms.RemoveAll(p => p.EventRoom.ID == alreadyBookedItem.EventRoom.ID);
                    });
            }

            eventRooms.RemoveAll(x => x.EventRoom.ID == _eventRoom.EventRoom.ID);
            _eventDataUnit.EventCateringsRepository.Refresh(RefreshMode.ClientWins);
            var caterings = await _eventDataUnit.EventCateringsRepository.GetAllAsync(eCatering =>
                !eCatering.Event.IsDeleted
                && eCatering.EventID != _event.Event.ID
                && eCatering.Event.Date == _event.Event.Date
                && eCatering.RoomID == EventRoom.Room.ID);
            caterings = _eventDataUnit.EventCateringsRepository.SetCateringsCurrentValues(caterings).ToList();
            var eventCaterings = caterings.Select(x => new EventCateringModel(x)).ToList();
            if (AlreadyBookedCaterings != null)
            {
                AlreadyBookedCaterings.ForEach(alreadyBookedItem =>
                {
                    eventCaterings.RemoveAll(p => p.EventCatering.ID == alreadyBookedItem.EventCatering.ID);
                });
            }

            var bookingService = new BookingsService { BookedRooms = eventRooms, BookedCaterings = eventCaterings };

            MapChangedDataAfterRefresh(EventRoom.EventRoom, eventRoom.EventRoom);

            var startTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, _eventRoom.StartTime.Hour, _eventRoom.StartTime.Minute, 0);
            var endTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, _eventRoom.EndTime.Hour, _eventRoom.EndTime.Minute, 0);

            bool bookingAllowed = bookingService.IsRoomAvailable(_event.Event.ID, EventRoom.Room, startTime, endTime);

            if (!bookingAllowed && EventRoom.Room.MultipleBooking)
            {
                bool? dialogResult = null;
                string confirmText = Resources.MESSAGE_ROOM_IS_BOOKED_BUT_SOPPORTS_MULTIBOOKING;

                RadWindow.Confirm(new DialogParameters
                {
                    Owner = Application.Current.MainWindow,
                    Content = confirmText,
                    Closed = (sender, args) => { dialogResult = args.DialogResult; }
                });

                if (dialogResult != true)
                {
                    SetLoadingIndicator(false);
                    return;
                }
                bookingAllowed = true;
            }

            if (bookingAllowed && EventRoom.HasValidProducts)
            {
                if (!_isEditMode)
                {
                    _event.EventRooms.Add(EventRoom);
                    _eventDataUnit.EventRoomsRepository.Add(EventRoom.EventRoom);

                    foreach (var product in EventRoom.EventBookedProducts)
                    {
                        product.EventCharge.EventCharge.ShowInInvoice = EventRoom.EventRoom.ShowInInvoice;

                        _event.EventCharges.Add(product.EventCharge);
                        _eventDataUnit.EventChargesRepository.Add(product.EventCharge.EventCharge);

                        _event.EventBookedProducts.Add(product);
                        _eventDataUnit.EventBookedProductsRepository.Add(product.EventBookedProduct);
                    }
                }
                else
                {
                    EventRoom.EventBookedProducts.ForEach(eventBookedProduct =>
                       {
                           eventBookedProduct.EventCharge.EventCharge.ShowInInvoice = EventRoom.EventRoom.ShowInInvoice;
                       });
                    var newProdcuts = _eventRoom.EventBookedProducts.Except(_event.EventBookedProducts).ToList();
                    if (newProdcuts.Any())
                    {
                        foreach (var prodcut in newProdcuts)
                        {
                            _event.EventBookedProducts.Add(prodcut);
                            _eventDataUnit.EventBookedProductsRepository.Add(prodcut.EventBookedProduct);

                            _event.EventCharges.Add(prodcut.EventCharge);
                            _eventDataUnit.EventChargesRepository.Add(prodcut.EventCharge.EventCharge);
                        }
                    }
                }

                RaisePropertyChanged("CloseDialog");
            }
            else
            {
                RaisePropertyChanged("DisableParentWindow");

                string confirmText = Resources.MESSAGE_ROOM_IS_BOOKED;
                RadWindow.Alert(new DialogParameters
                {
                    Owner = Application.Current.MainWindow,
                    Content = confirmText,
                });
                SetLoadingIndicator(false);
                RaisePropertyChanged("EnableParentWindow");
            }
        }

        private void SetLoadingIndicator(bool busyState)
        {
            IsBusy = busyState;
            SubmitCommand.RaiseCanExecuteChanged();
        }

        private void MapChangedDataAfterRefresh(EventRoom eventRoomOriginal, EventRoom eventRoomChanged)
        {
            eventRoomOriginal.RoomID = eventRoomChanged.RoomID;
            eventRoomOriginal.StartTime = eventRoomChanged.StartTime;
            eventRoomOriginal.EndTime = eventRoomChanged.EndTime;
            eventRoomOriginal.ShowInInvoice = eventRoomChanged.ShowInInvoice;
            eventRoomOriginal.IncludeInForwardBook = eventRoomChanged.IncludeInForwardBook;
            eventRoomOriginal.IncludeInCorrespondence = eventRoomChanged.IncludeInCorrespondence;
            eventRoomOriginal.Notes = eventRoomChanged.Notes;
        }

        private bool SubmitCommandCanExecute()
        {
            if (IsBusy)
                return false;
            return EventRoom != null && !EventRoom.HasErrors;
        }

        private void AddItemCommandExecuted()
        {
            AddRoomProduct(EventRoom);
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public void CancelCommandExecuted()
        {

            if (_isEditMode)
            {
                MapChangedDataAfterRefresh(EventRoom.EventRoom, EventRoomOriginal.EventRoom);
                EventRoom.Room = Rooms.FirstOrDefault(p => p.ID == EventRoomOriginal.Room.ID);
                EventRoom.Refresh();
            }
            else
            {
                _eventDataUnit.RevertChanges();
            }
        }

        private void AddProductCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddProductView();
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true && !view.ViewModel.Product.HasErrors)
            {
                Products.Add(view.ViewModel.Product.Product);
            }
        }

        private void DeleteBookedProductCommandExecuted(EventBookedProductModel obj)
        {
            obj.PropertyChanged -= OnEventBookedProductModelPropertyChanged;

            _eventRoom.EventBookedProducts.Remove(obj);

            _event.EventCharges.Remove(obj.EventCharge);
            _eventDataUnit.EventChargesRepository.Delete(obj.EventCharge.EventCharge);

            _event.EventBookedProducts.Remove(obj);
            _eventDataUnit.EventBookedProductsRepository.Delete(obj.EventBookedProduct);
            SubmitCommand.RaiseCanExecuteChanged();

        }

        #endregion
    }
}