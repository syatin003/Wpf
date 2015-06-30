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
using EventManagementSystem.Views.Admin.EPOS.Products;
using EventManagementSystem.Views.Events;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Services;
using System.Data.Entity.Core.Objects;
using EventManagementSystem.Core.Serialization;
using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;


namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items
{
    public class AddEventGolfItemViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private readonly EventModel _event;
        private bool _isBusy;
        private ObservableCollection<Product> _products;
        private EventGolfModel _eventGolf;
        private EventGolfModel _eventGolfOriginal;
        private ObservableCollection<Golf> _golfs;
        private ObservableCollection<GolfHole> _golfHoles;
        private List<EventGolfModel> _alreadyBookedGolfs;
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

        public ObservableCollection<Golf> Golfs
        {
            get { return _golfs; }
            set
            {
                if (_golfs == value) return;
                _golfs = value;
                RaisePropertyChanged(() => Golfs);
            }
        }

        public ObservableCollection<GolfHole> GolfHoles
        {
            get { return _golfHoles; }
            set
            {
                if (_golfHoles == value) return;
                _golfHoles = value;
                RaisePropertyChanged(() => GolfHoles);
            }
        }

        public EventGolfModel EventGolf
        {
            get { return _eventGolf; }
            set
            {
                if (_eventGolf == value) return;
                _eventGolf = value;
                RaisePropertyChanged(() => EventGolf);
            }
        }
        public EventGolfModel EventGolfOriginal
        {
            get { return _eventGolfOriginal; }
            set
            {
                if (_eventGolfOriginal == value) return;
                _eventGolfOriginal = value;
                RaisePropertyChanged(() => EventGolfOriginal);
            }
        }
        public List<EventGolfModel> AlreadyBookedGolfs
        {
            get { return _alreadyBookedGolfs; }
            set
            {
                if (_alreadyBookedGolfs == value) return;
                _alreadyBookedGolfs = value;
                RaisePropertyChanged(() => AlreadyBookedGolfs);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand AddItemCommand { get; private set; }
        public RelayCommand AddProductCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand ShowResourcesCommand { get; private set; }
        public RelayCommand<EventBookedProductModel> DeleteBookedProductCommand { get; private set; }

        #endregion

        #region Constructors

        public AddEventGolfItemViewModel(EventModel eventModel, EventGolfModel golfModel, List<EventGolfModel> alreadyBookedGolfs)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            AddItemCommand = new RelayCommand(AddItemCommandExecuted);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
            ShowResourcesCommand = new RelayCommand(ShowResourcesCommandExecuted);
            AddProductCommand = new RelayCommand(AddProductCommandExecuted);
            DeleteBookedProductCommand = new RelayCommand<EventBookedProductModel>(DeleteBookedProductCommandExecuted);
            AlreadyBookedGolfs = alreadyBookedGolfs;
            ProcessEventGolf(golfModel);

        }

        #endregion

        #region Methods

        private void ProcessEventGolf(EventGolfModel golfModel)
        {
            _isEditMode = (golfModel != null);

            EventGolf = (_isEditMode) ? golfModel : GetEventGolf();
            if (_isEditMode)
                EventGolfOriginal = EventGolf.Clone();
            EventGolf.PropertyChanged += OnEventBookedProductModelPropertyChanged;
        }

        private EventGolfModel GetEventGolf()
        {
            var eventGolf = new EventGolfModel(new EventGolf
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Slots = (int)Math.Round((double)(_event.Places / 4), 0),
                ShowInInvoice = true,
                IncludeInCorrespondence = true,
                IncludeInForwardBook = true
            });

            return eventGolf;
        }

        private void AddGolfProduct(EventGolfModel model)
        {
            var charge = new EventCharge
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                ShowInInvoice = model.EventGolf.ShowInInvoice
            };

            var item = new EventBookedProductModel(new EventBookedProduct
            {
                ID = Guid.NewGuid(),
                EventBookingItemID = model.EventGolf.ID,
                EventID = _event.Event.ID,
                EventCharge = charge
            });

            item.Quantity = _event.Event.Places;
            item.PropertyChanged += OnEventBookedProductModelPropertyChanged;

            model.EventBookedProducts.Add(item);
        }

        private void OnEventBookedProductModelPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            _eventDataUnit.GolfHolesRepository.Refresh(RefreshMode.ClientWins);
            var holes = await _eventDataUnit.GolfHolesRepository.GetAllAsync();
            GolfHoles = new ObservableCollection<GolfHole>(holes.OrderBy(x => x.Hole));

            _eventDataUnit.GolfsRepository.Refresh(RefreshMode.ClientWins);
            var golfs = await _eventDataUnit.GolfsRepository.GetAllAsync();
            Golfs = new ObservableCollection<Golf>(golfs);

            var products = await _eventDataUnit.ProductsRepository.GetAllAsync();

            if (_event.EventType == null)
            {
                Products = new ObservableCollection<Product>(products
                    .Where(x => x.ProductOption.OptionName == "Golf")
                    .OrderBy(x => x.Name)
                    .ToList());
            }
            else
            {
                Products = new ObservableCollection<Product>(products
                    .Where(x => x.ProductEventTypes.Any(e => e.EventType.ID == _event.EventType.ID))
                    .Where(x => x.ProductOption.OptionName == "Golf")
                    .OrderBy(x => x.Name)
                    .ToList());
            }

            IsBusy = false;
        }
        private void MapChangedDataAfterRefresh(EventGolf eventGolfOriginal, EventGolf eventGolfChanged)
        {

            eventGolfOriginal.TeeID = eventGolfChanged.TeeID;
            eventGolfOriginal.HoleID = eventGolfChanged.HoleID;
            eventGolfOriginal.Time = eventGolfChanged.Time;
            eventGolfOriginal.Slots = eventGolfChanged.Slots;
            eventGolfOriginal.ShowInInvoice = eventGolfChanged.ShowInInvoice;
            eventGolfOriginal.IncludeInForwardBook = eventGolfChanged.IncludeInForwardBook;
            eventGolfOriginal.IncludeInCorrespondence = eventGolfChanged.IncludeInCorrespondence;
            eventGolfOriginal.Notes = eventGolfChanged.Notes;
        }

        #endregion

        #region Commands

        private async void SubmitCommandExecuted()
        {
            IsBusy = true;
            SubmitCommand.RaiseCanExecuteChanged();
            if (EventGolf.HasErrors)
            {
                IsBusy = false;
                SubmitCommand.RaiseCanExecuteChanged();
                return;
            }
            EventGolf.EventGolf.Event = _event.Event;
            var eventGolf = EventGolf.Clone();
            EventGolfModel linkedEventGolf = GetEventGolf();
            EventGolf linkedGolf = null;
            _eventDataUnit.EventGolfsRepository.Refresh(RefreshMode.ClientWins);
            var golfs = await _eventDataUnit.EventGolfsRepository.GetAllAsync();
            golfs = _eventDataUnit.EventGolfsRepository.SetGolfCurrentValues(golfs).ToList();
            var eventGolfs = golfs.Select(x => new EventGolfModel(x)).ToList();

            if (_isEditMode)
            {
                if (EventGolf.EventGolf.LinkedEventGolfId != null)
                {
                    linkedGolf = eventGolfs.FirstOrDefault(p => p.EventGolf.ID == EventGolf.EventGolf.LinkedEventGolfId).EventGolf;
                    eventGolfs.RemoveAll(x => x.EventGolf.ID == EventGolf.EventGolf.LinkedEventGolfId);
                }
            }
            eventGolfs.RemoveAll(x => x.EventGolf.ID == _eventGolf.EventGolf.ID);
            if (AlreadyBookedGolfs != null)
            {
                AlreadyBookedGolfs.ForEach(alreadyBookedItem =>
                {
                    eventGolfs.RemoveAll(p => p.EventGolf.ID == alreadyBookedItem.EventGolf.ID);
                    if (alreadyBookedItem.EventGolf.EventGolf1 != null)
                    {
                        eventGolfs.RemoveAll(p => p.EventGolf.ID == alreadyBookedItem.EventGolf.EventGolf1.ID);
                    }
                });
            }

            var bookingService = new BookingsService { BookedGolfs = eventGolfs };
            MapChangedDataAfterRefresh(EventGolf.EventGolf, eventGolf.EventGolf);


            //check 1st Golf Booking
            var startTime = new DateTime(_event.Date.Year, _event.Date.Month, _event.Date.Day, _eventGolf.Time.Hour, _eventGolf.Time.Minute, 0);
            var endTime = startTime.AddMinutes(EventGolf.Golf.TimeInterval.TotalMinutes * EventGolf.EventGolf.Slots);
            var firstBookingAllowed = bookingService.IsGolfAvailable(EventGolf.Golf, startTime, endTime);
            var linkedBookingAllowed = false;

            var bookingAllowed = false;
            //Check if first booking allowed and then check second if allowed
            if (firstBookingAllowed)
            {
                //Check Whether Golf is 9 golfHoles or 18 Holes
                if (EventGolf.GolfHole.Hole.ToLower() == "18 holes")
                {
                    if (EventGolf.Golf.TurnDefault != null)
                    {
                        var golf = Golfs.FirstOrDefault(m => m.ID == EventGolf.Golf.TurnDefault);
                        if (golf != null)
                        {

                            startTime = startTime.Add(EventGolf.Golf.TurnTime);
                            startTime = startTime.AddMinutes(((new TimeSpan(startTime.Hour, startTime.Minute, 0).TotalMinutes - golf.StartTime.TotalMinutes) % golf.TimeInterval.TotalMinutes));
                            //  startTime = startTime.AddTicks((golf.StartTime.Ticks + new TimeSpan(0, 0, 1).Ticks - new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second).Ticks) / golf.TimeInterval.Ticks);
                            endTime = startTime.AddMinutes(Golfs.FirstOrDefault(m => m.ID == EventGolf.Golf.TurnDefault).TimeInterval.TotalMinutes * EventGolf.EventGolf.Slots);
                            MapChangedDataAfterRefresh(linkedEventGolf.EventGolf, eventGolf.EventGolf);
                            linkedEventGolf.EventGolf.TeeID = golf.ID;
                            linkedEventGolf.EventGolf.Time = new DateTime(0001, 01, 01, startTime.Hour, startTime.Minute, startTime.Second);
                            linkedEventGolf.EventGolf.IsLinked = true;
                            linkedEventGolf.EventGolf.Event = _event.Event;
                            linkedBookingAllowed = bookingService.IsGolfAvailable(golf, startTime, endTime);
                        }
                    }
                    else
                    {
                        EventGolf.EventGolf.LinkedEventGolfId = null;
                        linkedBookingAllowed = true;
                    }
                    bookingAllowed = firstBookingAllowed && linkedBookingAllowed;
                }
                else
                {
                    bookingAllowed = firstBookingAllowed;
                }
            }
            else
            {
                bookingAllowed = firstBookingAllowed;
            }
            if (bookingAllowed && EventGolf.HasValidProducts)
            {
                if (!_isEditMode)
                {
                    if (EventGolf.GolfHole.Hole.ToLower() == "18 holes" && EventGolf.Golf.TurnDefault != null
                        && Golfs.FirstOrDefault(m => m.ID == EventGolf.Golf.TurnDefault) != null)
                    {
                        EventGolf.EventGolf.EventGolf1 = linkedEventGolf.EventGolf;
                        EventGolf.EventGolf.LinkedEventGolfId = linkedEventGolf.EventGolf.ID;
                    }
                    _event.EventGolfs.Add(EventGolf);
                    _eventDataUnit.EventGolfsRepository.Add(EventGolf.EventGolf);
                    //if (EventGolf.GolfHole.Hole.ToLower() == "18 holes")
                    //    _eventDataUnit.EventGolfsRepository.Add(linkedEventGolf.EventGolf);

                    foreach (var product in EventGolf.EventBookedProducts)
                    {
                        product.EventCharge.EventCharge.ShowInInvoice = EventGolf.EventGolf.ShowInInvoice;

                        _event.EventCharges.Add(product.EventCharge);
                        _eventDataUnit.EventChargesRepository.Add(product.EventCharge.EventCharge);

                        _event.EventBookedProducts.Add(product);
                        _eventDataUnit.EventBookedProductsRepository.Add(product.EventBookedProduct);
                    }
                }
                else
                {
                    if (linkedGolf != null)
                        _eventDataUnit.EventGolfsRepository.Delete(linkedGolf);
                    if (EventGolf.GolfHole.Hole.ToLower() == "18 holes" && EventGolf.Golf.TurnDefault != null && Golfs.FirstOrDefault(m => m.ID == EventGolf.Golf.TurnDefault) != null)
                    {
                        EventGolf.EventGolf.EventGolf1 = linkedEventGolf.EventGolf;
                        EventGolf.EventGolf.LinkedEventGolfId = linkedEventGolf.EventGolf.ID;
                    }
                    //if (EventGolf.GolfHole.Hole.ToLower() == "18 holes")
                    //    _eventDataUnit.EventGolfsRepository.Add(linkedEventGolf.EventGolf);
                    EventGolf.EventBookedProducts.ForEach(eventBookedProduct =>
                        {
                            eventBookedProduct.EventCharge.EventCharge.ShowInInvoice = EventGolf.EventGolf.ShowInInvoice;
                        });
                    var newProdcuts = _eventGolf.EventBookedProducts.Except(_event.EventBookedProducts).ToList();
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
                IsBusy = false;
                SubmitCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("DisableParentWindow");

                string confirmText = Resources.MESSAGE_TEE_IS_BOOKED;
                RadWindow.Alert(new DialogParameters()
                {
                    Owner = Application.Current.MainWindow,
                    Content = confirmText,
                });

                RaisePropertyChanged("EnableParentWindow");
            }
        }

        private bool SubmitCommandCanExecute()
        {
            if (IsBusy)
                return false;
            return EventGolf != null && !EventGolf.HasErrors;
        }

        private void AddItemCommandExecuted()
        {
            AddGolfProduct(EventGolf);
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public void CancelCommandExecuted()
        {

            if (_isEditMode)
            {
                MapChangedDataAfterRefresh(EventGolf.EventGolf, EventGolfOriginal.EventGolf);
                EventGolf.Golf = Golfs.FirstOrDefault(p => p.ID == EventGolfOriginal.Golf.ID);
                EventGolf.GolfHole = GolfHoles.FirstOrDefault(p => p.ID == EventGolfOriginal.GolfHole.ID);
                EventGolf.Refresh();
            }
            else
            {
                _eventDataUnit.RevertChanges();
            }
        }

        private void ShowResourcesCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new ResourcesView(_event.Date);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
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

            _eventGolf.EventBookedProducts.Remove(obj);

            _event.EventCharges.Remove(obj.EventCharge);
            _eventDataUnit.EventChargesRepository.Delete(obj.EventCharge.EventCharge);

            _event.EventBookedProducts.Remove(obj);
            _eventDataUnit.EventBookedProductsRepository.Delete(obj.EventBookedProduct);
            SubmitCommand.RaiseCanExecuteChanged();

        }

        #endregion
    }
}