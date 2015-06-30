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
using Microsoft.Practices.ObjectBuilder2;
using EventManagementSystem.Core.Serialization;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;



namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items
{
    public class AddCateringItemViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private readonly EventModel _event;
        private ObservableCollection<Room> _rooms;
        private bool _isBusy;
        private ObservableCollection<Product> _products;
        private EventCateringModel _eventCatering;
        private EventCateringModel _eventCateringOriginal;
        private List<EventCateringModel> _alreadyBookedCaterings;
        private List<EventRoomModel> _alreadyBookedRooms;
        private ObservableCollection<TimeSpan> _clockItems;

        private bool _isEditMode;

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

        public EventCateringModel EventCatering
        {
            get { return _eventCatering; }
            set
            {
                if (_eventCatering == value) return;
                _eventCatering = value;
                RaisePropertyChanged(() => EventCatering);
            }
        }
        public EventCateringModel EventCateringOriginal
        {
            get { return _eventCateringOriginal; }
            set
            {
                if (_eventCateringOriginal == value) return;
                _eventCateringOriginal = value;
                RaisePropertyChanged(() => EventCateringOriginal);
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

        public AddCateringItemViewModel(EventModel eventModel, EventCateringModel cateringModel, List<EventCateringModel> alreadyBookedCaterings, List<EventRoomModel> alreadyBookedRooms)
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

            ProcessEventCatering(cateringModel);
        }

        #endregion

        #region Methods

        private void ProcessEventCatering(EventCateringModel cateringModel)
        {
            _isEditMode = (cateringModel != null);

            EventCatering = cateringModel ?? GetEventCatering();
            if (_isEditMode)
            {
                CreateClockItems();
                EventCateringOriginal = EventCatering.Clone();
            }
            EventCatering.PropertyChanged += OnEventBookedProductModelPropertyChanged;
            if (_isEditMode)
                cateringModel.EventBookedProducts.ForEach(product =>
                    {
                        product.PropertyChanged += OnEditBookedProductModelPropertyChanged;
                    });
        }

        private EventCateringModel GetEventCatering()
        {
            var cateringModel = new EventCateringModel(new EventCatering
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                StartTime = _event.Date,
                EndTime = _event.Date,
                ShowInInvoice = true,
                IncludeInCorrespondence = true,
                IncludeInForwardBook = true
            });

            return cateringModel;
        }

        private void AddCateringProduct(EventCateringModel model)
        {
            var charge = new EventCharge
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                ShowInInvoice = model.EventCatering.ShowInInvoice
            };

            var productModel = new EventBookedProductModel(new EventBookedProduct
            {
                ID = Guid.NewGuid(),
                EventBookingItemID = model.EventCatering.ID,
                EventID = _event.Event.ID,
                EventCharge = charge
            });

            productModel.Quantity = _event.Event.Places;
            productModel.PropertyChanged += OnEventBookedProductModelPropertyChanged;

            model.EventBookedProducts.Add(productModel);
        }
        private void OnEditBookedProductModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case "Quantity":
                    if (sender.GetType() == typeof(EventBookedProductModel))
                    {
                        var model = (EventBookedProductModel)sender;
                        var eventChargeModel = _event.EventCharges.FirstOrDefault(x => x.EventCharge.ID == model.EventCharge.EventCharge.ID);
                        if (
                            eventChargeModel !=
                            null)
                            eventChargeModel.Quantity = model.Quantity;
                    }
                    break;
                case "Product":
                    if (sender.GetType() == typeof(EventBookedProductModel))
                    {
                        var model = (EventBookedProductModel)sender;
                        var eventChargeModel = _event.EventCharges.FirstOrDefault(x => x.EventCharge.ID == model.EventCharge.EventCharge.ID);
                        if (
                            eventChargeModel !=
                            null)
                            eventChargeModel.Product = model.Product;
                    }
                    break;
                case "Price":
                    if (sender.GetType() == typeof(EventBookedProductModel))
                    {
                        var model = (EventBookedProductModel)sender;
                        var eventChargeModel = _event.EventCharges.FirstOrDefault(x => x.EventCharge.ID == model.EventCharge.EventCharge.ID);
                        if (
                            eventChargeModel !=
                            null)
                            eventChargeModel.Price = model.Price;
                    }
                    break;
            }
        }
        private void OnEventBookedProductModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // When you select a time, default the start time of the booking to the same time.
            if (args.PropertyName == "Time")
                _eventCatering.StartTime = _eventCatering.Time;
            if (args.PropertyName == "Room")
                CreateClockItems();
            SubmitCommand.RaiseCanExecuteChanged();
        }

        private void CreateClockItems()
        {
            ClockItems = new ObservableCollection<TimeSpan>();
            var startTime = EventCatering.Room.StartTime;
            var endTime = EventCatering.Room.EndTime;
            var timeInterval = EventCatering.Room.TimeInterval;
            var clockItem = new TimeSpan(startTime.Hours, startTime.Minutes, startTime.Seconds);
            ClockItems.Add(clockItem);

            if (startTime < endTime)
            {
                Int32 slots = Convert.ToInt32((endTime.Ticks - startTime.Ticks) / timeInterval.Ticks);
                for (int i = 0; i < slots; i++)
                {
                    clockItem = clockItem.Add(EventCatering.Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
            }
            else
            {
                var endOfTheDay = new TimeSpan(24, 0, 0);
                Int32 slotsBeforeEndOfDay = Convert.ToInt32((endOfTheDay.Ticks - startTime.Ticks) / timeInterval.Ticks);
                for (int i = 0; i < slotsBeforeEndOfDay; i++)
                {
                    clockItem = clockItem.Add(EventCatering.Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
                Int32 slotsAfterEndOfDay = Convert.ToInt32(endTime.Ticks / timeInterval.Ticks);
                clockItem = new TimeSpan();
                for (int i = 0; i < slotsAfterEndOfDay; i++)
                {
                    clockItem = clockItem.Add(EventCatering.Room.TimeInterval);
                    ClockItems.Add(clockItem);
                }
            }
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
                    .Where(x => x.ProductOption.OptionName == "Catering")
                    .OrderBy(x => x.Name)
                    .ToList());
            }
            else
            {
                Products = new ObservableCollection<Product>(products
                    .Where(x => x.ProductEventTypes.Any(e => e.EventType.ID == _event.EventType.ID))
                    .Where(x => x.ProductOption.OptionName == "Catering")
                    .ToList());
            }

            IsBusy = false;
        }

        #endregion

        #region Commands

        private async void SubmitCommandExecuted()
        {
            SetLoadingIndicator(true);
            if (EventCatering.HasErrors)
            {
                SetLoadingIndicator(false);
                return;
            }
            EventCatering.EventCatering.Event = _event.Event;
            var eventCatering = EventCatering.Clone();

            _eventDataUnit.EventRoomsRepository.Refresh(RefreshMode.ClientWins);
            var rooms = await _eventDataUnit.EventRoomsRepository.GetAllAsync(eRoom =>
                !eRoom.Event.IsDeleted
                && eRoom.EventID != _event.Event.ID
                && eRoom.Event.Date == _event.Event.Date
                && eRoom.RoomID == EventCatering.Room.ID);
            rooms = _eventDataUnit.EventRoomsRepository.SetRoomsCurrentValues(rooms).ToList();
            var eventRooms = rooms.Select(x => new EventRoomModel(x)).ToList();
            if (AlreadyBookedRooms != null)
            {
                AlreadyBookedRooms.ForEach(alreadyBookedItem =>
                {
                    eventRooms.RemoveAll(p => p.EventRoom.ID == alreadyBookedItem.EventRoom.ID);
                });
            }

            _eventDataUnit.EventCateringsRepository.Refresh(RefreshMode.ClientWins);
            var caterings = await _eventDataUnit.EventCateringsRepository.GetAllAsync(eCatering =>
                !eCatering.Event.IsDeleted
                && eCatering.EventID != _event.Event.ID
                && eCatering.Event.Date == _event.Event.Date
                && eCatering.RoomID == EventCatering.Room.ID
                );
            caterings = _eventDataUnit.EventCateringsRepository.SetCateringsCurrentValues(caterings).ToList();
            var eventCaterings = caterings.Select(x => new EventCateringModel(x)).ToList();
            if (AlreadyBookedCaterings != null)
            {
                AlreadyBookedCaterings.ForEach(alreadyBookedItem => eventCaterings.RemoveAll(p => p.EventCatering.ID == alreadyBookedItem.EventCatering.ID));
            }
            eventCaterings.RemoveAll(x => x.EventCatering.ID == _eventCatering.EventCatering.ID);

            var bookingService = new BookingsService { BookedRooms = eventRooms, BookedCaterings = eventCaterings };

            MapChangedDataAfterRefresh(EventCatering.EventCatering, eventCatering.EventCatering);

            var startTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, _eventCatering.StartTime.Hour, _eventCatering.StartTime.Minute, 0);
            var endTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, _eventCatering.EndTime.Hour, _eventCatering.EndTime.Minute, 0);

            bool bookingAllowed = bookingService.IsRoomAvailable(_event.Event.ID, EventCatering.Room, startTime, endTime);

            if (!bookingAllowed && EventCatering.Room.MultipleBooking)
            {
                bool? dialogResult = null;
                string confirmText = Resources.MESSAGE_ROOM_IS_BOOKED_BUT_SOPPORTS_MULTIBOOKING;

                RadWindow.Confirm(new System.Windows.Controls.TextBlock { Text = confirmText, TextWrapping = TextWrapping.Wrap, Width = 400 },
                   (s, args) => { dialogResult = args.DialogResult; });

                if (dialogResult != true)
                {
                    SetLoadingIndicator(false);
                    return;
                }

                bookingAllowed = true;
            }

            if (bookingAllowed && EventCatering.HasValidProducts)
            {
                if (!_isEditMode)
                {
                    _event.EventCaterings.Add(EventCatering);

                    _eventDataUnit.EventCateringsRepository.Add(EventCatering.EventCatering);

                    foreach (var product in EventCatering.EventBookedProducts)
                    {
                        product.EventCharge.EventCharge.ShowInInvoice = EventCatering.EventCatering.ShowInInvoice;
                        _event.EventCharges.Add(product.EventCharge);
                        _eventDataUnit.EventChargesRepository.Add(product.EventCharge.EventCharge);

                        _event.EventBookedProducts.Add(product);
                        _eventDataUnit.EventBookedProductsRepository.Add(product.EventBookedProduct);
                    }
                }
                else
                {
                    EventCatering.EventBookedProducts.ForEach(eventBookedProduct =>
                        {
                            eventBookedProduct.EventCharge.EventCharge.ShowInInvoice = EventCatering.EventCatering.ShowInInvoice;
                        });
                    var newProdcuts = _eventCatering.EventBookedProducts.Except(_event.EventBookedProducts).ToList();
                    if (newProdcuts.Any())
                    {
                        foreach (var product in newProdcuts)
                        {
                            _event.EventBookedProducts.Add(product);
                            _eventDataUnit.EventBookedProductsRepository.Add(product.EventBookedProduct);

                            _event.EventCharges.Add(product.EventCharge);
                            _eventDataUnit.EventChargesRepository.Add(product.EventCharge.EventCharge);
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
        private void MapChangedDataAfterRefresh(EventCatering eventCateringOriginal, EventCatering eventCateringChanged)
        {
            eventCateringOriginal.Time = eventCateringChanged.Time;
            eventCateringOriginal.RoomID = eventCateringChanged.RoomID;
            eventCateringOriginal.StartTime = eventCateringChanged.StartTime;
            eventCateringOriginal.EndTime = eventCateringChanged.EndTime;
            eventCateringOriginal.Notes = eventCateringChanged.Notes;
            eventCateringOriginal.ShowInInvoice = eventCateringChanged.ShowInInvoice;
            eventCateringOriginal.IncludeInForwardBook = eventCateringChanged.IncludeInForwardBook;
            eventCateringOriginal.IncludeInCorrespondence = eventCateringChanged.IncludeInCorrespondence;
            eventCateringOriginal.IsSpecial = eventCateringChanged.IsSpecial;

        }


        private bool SubmitCommandCanExecute()
        {
            if (IsBusy)
                return false;
            return !EventCatering.HasErrors;
        }

        private void AddItemCommandExecuted()
        {
            AddCateringProduct(EventCatering);
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public void CancelCommandExecuted()
        {
            _eventDataUnit.RevertChanges();

            if (_isEditMode)
            {
                MapChangedDataAfterRefresh(EventCatering.EventCatering, EventCateringOriginal.EventCatering);
                EventCatering.Room = Rooms.FirstOrDefault(p => p.ID == EventCateringOriginal.Room.ID);
                EventCatering.Refresh();
            }
        }

        private void AddProductCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddProductView();
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult.Value)
            {
                Products.Add(view.ViewModel.Product.Product);
            }
        }

        private void DeleteBookedProductCommandExecuted(EventBookedProductModel obj)
        {
            obj.PropertyChanged -= OnEventBookedProductModelPropertyChanged;

            _eventCatering.EventBookedProducts.Remove(obj);

            _event.EventCharges.Remove(obj.EventCharge);
            _eventDataUnit.EventChargesRepository.Delete(obj.EventCharge.EventCharge);

            _event.EventBookedProducts.Remove(obj);
            _eventDataUnit.EventBookedProductsRepository.Delete(obj.EventBookedProduct);

            SubmitCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}