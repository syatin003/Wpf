using System;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ObjectBuilder2;
using System.Collections.ObjectModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Data.Model;
using System.Threading.Tasks;
using EventManagementSystem.Services;


namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items
{
    public class FromTemplateViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;
        private ObservableCollection<EventModel> _events;
        private List<EventModel> _allEvents;
        private EventModel _selectedEvent;
        private readonly EventModel _event;

        #endregion Fields

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

        public ObservableCollection<EventModel> Events
        {
            get { return _events; }
            set
            {
                if (_events == value) return;
                _events = value;
                RaisePropertyChanged(() => Events);
            }
        }
        public EventModel SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (_selectedEvent == value) return;
                _selectedEvent = value;
                RaisePropertyChanged(() => SelectedEvent);

                OKCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand OKCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public FromTemplateViewModel(EventModel eventModel)
        {
            _event = eventModel;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
            OKCommand = new RelayCommand(OKCommandExecuted, OKCommandCanExecute);

        }
        #endregion Constructor

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;
            _eventsDataUnit.EventsRepository.Refresh();
            var events = await _eventsDataUnit.EventsRepository.GetLightEventsAsync(x => !x.IsDeleted && x.UsedAsTemplate && x.ID != _event.Event.ID && x.EventBookedProducts.Any());
            _allEvents = new List<EventModel>(events.OrderBy(eventItem => eventItem.Date).Select(x => new EventModel(x)));
            Events = new ObservableCollection<EventModel>(_allEvents);

            IsBusy = false;
        }

        public async Task LoadLightEventDetails(EventModel model)
        {
            if (!model.EventItems.Any())
            {
                var products = await _eventsDataUnit.EventBookedProductsRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventBookedProducts = new List<EventBookedProductModel>(products.Select(x => new EventBookedProductModel(x)));

                var caterings = await _eventsDataUnit.EventCateringsRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventCaterings = new List<EventCateringModel>(caterings.Select(x => new EventCateringModel(x)));

                var rooms = await _eventsDataUnit.EventRoomsRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventRooms = new List<EventRoomModel>(rooms.Select(x => new EventRoomModel(x)));

                var golfs = await _eventsDataUnit.EventGolfsRepository.GetAllAsync(x => x.EventID == model.Event.ID && x.IsLinked == false);
                model.EventGolfs = new List<EventGolfModel>(golfs.Select(x => new EventGolfModel(x)));

                var invoices = await _eventsDataUnit.EventInvoicesRepository.GetAllAsync(x => x.EventID == model.Event.ID);
                model.EventInvoices = new List<EventInvoiceModel>(invoices.Select(x => new EventInvoiceModel(x)));
            }
        }

        #endregion Methods

        #region Commands

        private bool OKCommandCanExecute()
        {
            return SelectedEvent != null;
        }

        private async void OKCommandExecuted()
        {
            IsBusy = true;

            var fromEvent = SelectedEvent;

            await LoadLightEventDetails(fromEvent);

            // Event Caterings
            if (fromEvent.EventCaterings.Any())
            {
                fromEvent.EventCaterings.ForEach(x =>
                {
                    var catering = new EventCatering()
                    {
                        ID = Guid.NewGuid(),
                        EventID = _event.Event.ID,
                        Event = _event.Event,
                        Time = x.EventCatering.Time,
                        RoomID = x.EventCatering.RoomID,
                        StartTime = x.EventCatering.StartTime,
                        EndTime = x.EventCatering.EndTime,
                        Notes = x.EventCatering.Notes,
                        ShowInInvoice = x.EventCatering.ShowInInvoice,
                        IncludeInForwardBook = x.EventCatering.IncludeInForwardBook,
                        IncludeInCorrespondence = x.EventCatering.IncludeInCorrespondence,
                        IsSpecial = x.EventCatering.IsSpecial
                    };

                    var products = fromEvent.EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == x.EventCatering.ID).ToList();

                    if (products.Any())
                    {
                        products.ForEach(y =>
                        {
                            var product = new EventBookedProduct()
                            {
                                ID = Guid.NewGuid(),
                                EventID = _event.Event.ID,
                                ProductID = y.EventBookedProduct.ProductID,
                                Product = y.EventBookedProduct.Product,
                                EventBookingItemID = catering.ID,
                                Quantity = _event.Event.Places,
                                Price = y.EventBookedProduct.Price
                            };

                            var charge = new EventCharge()
                            {
                                ID = product.ID,
                                EventID = _event.Event.ID,
                                ProductID = product.ProductID,
                                Quantity = product.Quantity,
                                Price = product.Price,
                                Product = product.Product,
                                ShowInInvoice = catering.ShowInInvoice
                            };

                            product.EventCharge = charge;


                            _eventsDataUnit.EventBookedProductsRepository.Add(product);
                            _eventsDataUnit.EventChargesRepository.Add(charge);
                            _event.EventBookedProducts.Add(new EventBookedProductModel(product));
                            _event.EventCharges.Add(new EventChargeModel(charge));
                        });
                    }
                    _eventsDataUnit.EventCateringsRepository.Add(catering);
                    _event.EventCaterings.Add(new EventCateringModel(catering));
                });
            }

            // Event Rooms
            if (fromEvent.EventRooms.Any())
            {
                fromEvent.EventRooms.ForEach(x =>
                {
                    var room = new EventRoom()
                    {
                        ID = Guid.NewGuid(),
                        EventID = _event.Event.ID,
                        Event = _event.Event,
                        RoomID = x.EventRoom.RoomID,
                        StartTime = x.EventRoom.StartTime,
                        EndTime = x.EventRoom.EndTime,
                        Notes = x.EventRoom.Notes,
                        ShowInInvoice = x.EventRoom.ShowInInvoice,
                        IncludeInForwardBook = x.EventRoom.IncludeInForwardBook,
                        IncludeInCorrespondence = x.EventRoom.IncludeInCorrespondence,
                    };

                    var products = fromEvent.EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == x.EventRoom.ID).ToList();

                    if (products.Any())
                    {
                        products.ForEach(y =>
                        {
                            var product = new EventBookedProduct()
                            {
                                ID = Guid.NewGuid(),
                                EventID = _event.Event.ID,
                                ProductID = y.EventBookedProduct.ProductID,
                                Product = y.EventBookedProduct.Product,
                                EventBookingItemID = room.ID,
                                Quantity = _event.Event.Places,
                                Price = y.EventBookedProduct.Price
                            };

                            var charge = new EventCharge()
                            {
                                ID = product.ID,
                                EventID = _event.Event.ID,
                                ProductID = product.ProductID,
                                Quantity = product.Quantity,
                                Price = product.Price,
                                Product = product.Product,
                                ShowInInvoice = room.ShowInInvoice
                            };
                            product.EventCharge = charge;

                            _eventsDataUnit.EventBookedProductsRepository.Add(product);
                            _eventsDataUnit.EventChargesRepository.Add(charge);
                            _event.EventBookedProducts.Add(new EventBookedProductModel(product));
                            _event.EventCharges.Add(new EventChargeModel(charge));
                        });
                    }
                    _eventsDataUnit.EventRoomsRepository.Add(room);
                    _event.EventRooms.Add(new EventRoomModel(room));
                });
            }

            // Event Golfs
            var fromEventGolfs = fromEvent.EventGolfs.Where(eventGolf => !eventGolf.EventGolf.IsLinked);
            if (fromEventGolfs.Any())
            {
                fromEvent.EventGolfs.ForEach(x =>
                {
                    var golf = new EventGolf()
                    {
                        ID = Guid.NewGuid(),
                        EventID = _event.Event.ID,
                        Event = _event.Event,
                        Time = x.EventGolf.Time,
                        TeeID = x.EventGolf.TeeID,
                        HoleID = x.EventGolf.HoleID,
                        Slots = x.EventGolf.Slots,
                        Notes = x.EventGolf.Notes,
                        ShowInInvoice = x.EventGolf.ShowInInvoice,
                        IncludeInForwardBook = x.EventGolf.IncludeInForwardBook,
                        IncludeInCorrespondence = x.EventGolf.IncludeInCorrespondence,
                        EventGolf1 = x.EventGolf.EventGolf1 != null ? new EventGolf()
                        {
                            ID = Guid.NewGuid(),
                            EventID = _event.Event.ID,
                            Event = _event.Event,
                            Time = x.EventGolf.EventGolf1.Time,
                            TeeID = x.EventGolf.EventGolf1.TeeID,
                            HoleID = x.EventGolf.EventGolf1.HoleID,
                            Slots = x.EventGolf.EventGolf1.Slots,
                            Notes = x.EventGolf.EventGolf1.Notes,
                            ShowInInvoice = x.EventGolf.EventGolf1.ShowInInvoice,
                            IncludeInForwardBook = x.EventGolf.EventGolf1.IncludeInForwardBook,
                            IncludeInCorrespondence = x.EventGolf.EventGolf1.IncludeInCorrespondence,
                            IsLinked = true
                        } : null
                    };

                    var products = fromEvent.EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == x.EventGolf.ID).ToList();

                    if (products.Any())
                    {
                        products.ForEach(y =>
                        {
                            var product = new EventBookedProduct()
                            {
                                ID = Guid.NewGuid(),
                                EventID = _event.Event.ID,
                                ProductID = y.EventBookedProduct.ProductID,
                                Product = y.EventBookedProduct.Product,
                                EventBookingItemID = golf.ID,
                                Quantity = _event.Event.Places,
                                Price = y.EventBookedProduct.Price
                            };

                            var charge = new EventCharge()
                            {
                                ID = product.ID,
                                EventID = _event.Event.ID,
                                ProductID = product.ProductID,
                                Product = product.Product,
                                Quantity = product.Quantity,
                                Price = product.Price,
                                ShowInInvoice = golf.ShowInInvoice
                            };
                            product.EventCharge = charge;
                            _eventsDataUnit.EventBookedProductsRepository.Add(product);
                            _eventsDataUnit.EventChargesRepository.Add(charge);
                            _event.EventBookedProducts.Add(new EventBookedProductModel(product));
                            _event.EventCharges.Add(new EventChargeModel(charge));
                        });
                    }
                    _eventsDataUnit.EventGolfsRepository.Add(golf);
                    _event.EventGolfs.Add(new EventGolfModel(golf));
                });
            }

            // Event Invoices
            if (fromEvent.EventInvoices.Any())
            {
                fromEvent.EventInvoices.ForEach(x =>
                {
                    var invoice = new EventInvoice()
                    {
                        ID = Guid.NewGuid(),
                        EventID = _event.Event.ID,
                        Event = _event.Event,
                        Notes = x.EventInvoice.Notes,
                        ShowInInvoice = x.EventInvoice.ShowInInvoice,
                        IncludeInForwardBook = x.EventInvoice.IncludeInForwardBook,
                        IncludeInCorrespondence = x.EventInvoice.IncludeInCorrespondence,
                    };

                    var products = fromEvent.EventBookedProducts.Where(y => y.EventBookedProduct.EventBookingItemID == x.EventInvoice.ID).ToList();

                    if (products.Any())
                    {
                        products.ForEach(y =>
                        {
                            var product = new EventBookedProduct()
                            {
                                ID = Guid.NewGuid(),
                                EventID = _event.Event.ID,
                                ProductID = y.EventBookedProduct.ProductID,
                                Product = y.EventBookedProduct.Product,
                                EventBookingItemID = invoice.ID,
                                Quantity = _event.Event.Places,
                                Price = y.EventBookedProduct.Price
                            };

                            var charge = new EventCharge()
                            {
                                ID = product.ID,
                                EventID = _event.Event.ID,
                                ProductID = product.ProductID,
                                Product = product.Product,
                                Quantity = product.Quantity,
                                Price = product.Price,
                                ShowInInvoice = invoice.ShowInInvoice
                            };
                            product.EventCharge = charge;
                            _eventsDataUnit.EventBookedProductsRepository.Add(product);
                            _eventsDataUnit.EventChargesRepository.Add(charge);
                            _event.EventBookedProducts.Add(new EventBookedProductModel(product));
                            _event.EventCharges.Add(new EventChargeModel(charge));
                        });
                    }
                    _eventsDataUnit.EventInvoicesRepository.Add(invoice);
                    _event.EventInvoices.Add(new EventInvoiceModel(invoice));
                });
            }
            RaisePropertyChanged("CloseDialog");
            IsBusy = false;
        }
        #endregion Commands
    }
}
