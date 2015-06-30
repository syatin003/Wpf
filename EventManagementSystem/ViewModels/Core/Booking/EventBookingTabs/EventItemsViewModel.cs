using System;
using System.Windows;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Items;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventItemsViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;

        private readonly IEventDataUnit _eventsDataUnit;

        #endregion

        #region Properties

        public EventModel Event
        {
            get { return _event; }
            set
            {
                if (_event == value) return;
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }

        public RelayCommand AddCateringCommand { get; private set; }
        public RelayCommand AddRoomCommand { get; private set; }
        public RelayCommand AddInvoiceCommand { get; private set; }
        public RelayCommand AddGolfCommand { get; private set; }

        public RelayCommand FromTemplateCommand { get; private set; }

        public RelayCommand<EventItemModel> EditItemCommand { get; set; }
        public RelayCommand<EventItemModel> DeleteItemCommand { get; set; }

        #endregion

        #region Constructor

        public EventItemsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            AddCateringCommand = new RelayCommand(AddCateringCommandExecuted);
            AddRoomCommand = new RelayCommand(AddRoomCommandExecuted);
            AddInvoiceCommand = new RelayCommand(AddInvoiceCommandExecuted);
            AddGolfCommand = new RelayCommand(AddGolfCommandExecuted);

            FromTemplateCommand = new RelayCommand(FromTemplateCommandExecuted);

            DeleteItemCommand = new RelayCommand<EventItemModel>(DeleteItemCommandExecuted);
            EditItemCommand = new RelayCommand<EventItemModel>(EditItemCommandExecuted);
        }

        #endregion

        #region Methods
        private void MapChangedDataAfterRefresh(Data.Model.EventRoom eventRoomOriginal, Data.Model.EventRoom eventRoomChanged)
        {
            eventRoomOriginal.RoomID = eventRoomChanged.RoomID;
            eventRoomOriginal.StartTime = eventRoomChanged.StartTime;
            eventRoomOriginal.EndTime = eventRoomChanged.EndTime;
            eventRoomOriginal.ShowInInvoice = eventRoomChanged.ShowInInvoice;
            eventRoomOriginal.IncludeInForwardBook = eventRoomChanged.IncludeInForwardBook;
            eventRoomOriginal.IncludeInCorrespondence = eventRoomChanged.IncludeInCorrespondence;
            eventRoomOriginal.Notes = eventRoomChanged.Notes;
        }
        private void MapChangedDataAfterRefresh(Data.Model.EventCatering eventCateringOriginal, Data.Model.EventCatering eventCateringChanged)
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
        private void MapChangedDataAfterRefresh(Data.Model.EventGolf eventGolfOriginal, Data.Model.EventGolf eventGolfChanged)
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

        private void AddCateringCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddCateringItemView(Event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.Event.LastEditDate = DateTime.Now;
                _event.RefreshItems();
            }
        }

        private void AddRoomCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddRoomItemView(Event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.Event.LastEditDate = DateTime.Now;
                _event.RefreshItems();
            }
        }

        private void AddInvoiceCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddEventInvoiceItemView(Event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.Event.LastEditDate = DateTime.Now;
                _event.RefreshItems();
            }
        }

        private void AddGolfCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddEventGolfItemView(Event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.Event.LastEditDate = DateTime.Now;
                _event.RefreshItems();
            }
        }

        private void FromTemplateCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var fromTemplateView = new FromTemplateView(Event);
            fromTemplateView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (fromTemplateView.DialogResult != null && fromTemplateView.DialogResult.Value)
            {
                _event.RefreshItems();
            }
        }

        private void DeleteItemCommandExecuted(EventItemModel item)
        {
            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(new DialogParameters()
            {
                Owner = Application.Current.MainWindow,
                Content = confirmText,
                Closed = (sender, args) => { dialogResult = args.DialogResult; }
            });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            if (item.Instance.GetType() == typeof(EventCateringModel))
            {
                var model = (EventCateringModel)item.Instance;

                // Remove booked products
                model.EventBookedProducts.ForEach(RemoveEventBookedProductAndCharges);
                _event.EventCaterings.Remove(model);
                _eventsDataUnit.EventCateringsRepository.Delete(model.EventCatering);
            }
            else if (item.Instance.GetType() == typeof(EventGolfModel))
            {
                var model = (EventGolfModel)item.Instance;

                // Remove booked products
                model.EventBookedProducts.ForEach(RemoveEventBookedProductAndCharges);
                if (model.EventGolf.LinkedEventGolfId != null)
                {
                    if (model.EventGolf.EventGolf1 == null)
                    {
                        _eventsDataUnit.EventGolfsRepository.DetachGolfEvent((Guid)model.EventGolf.LinkedEventGolfId);
                    }
                    else
                    {
                        _eventsDataUnit.EventGolfsRepository.Delete(model.EventGolf.EventGolf1);
                    }

                }
                _event.EventGolfs.Remove(model);

                _eventsDataUnit.EventGolfsRepository.Delete(model.EventGolf);
            }
            else if (item.Instance.GetType() == typeof(EventRoomModel))
            {
                var model = (EventRoomModel)item.Instance;

                // Remove booked products
                model.EventBookedProducts.ForEach(RemoveEventBookedProductAndCharges);

                _event.EventRooms.Remove(model);
                _eventsDataUnit.EventRoomsRepository.Delete(model.EventRoom);
            }
            else if (item.Instance.GetType() == typeof(EventInvoiceModel))
            {
                var model = (EventInvoiceModel)item.Instance;

                // Remove booked products
                model.EventBookedProducts.ForEach(RemoveEventBookedProductAndCharges);

                _event.EventInvoices.Remove(model);
                _eventsDataUnit.EventInvoicesRepository.Delete(model.EventInvoice);
            }

            _event.Event.LastEditDate = DateTime.Now;
            _event.RefreshItems();
        }

        private void RemoveEventBookedProductAndCharges(EventBookedProductModel product)
        {
            _eventsDataUnit.EventChargesRepository.Delete(product.EventCharge.EventCharge);
            _eventsDataUnit.EventBookedProductsRepository.Delete(product.EventBookedProduct);
            var eventCharges = new System.Collections.Generic.List<EventChargeModel>();
            _event.EventCharges.ForEach(eventCharge =>
            {
                if (eventCharge.EventCharge.ID == product.EventCharge.EventCharge.ID)
                {
                    eventCharges.Add(eventCharge);
                }
            });
            eventCharges.ForEach(echarge => _event.EventCharges.Remove(echarge));

            _event.EventBookedProducts.Remove(product);
        }

        private void EditItemCommandExecuted(EventItemModel item)
        {
            RaisePropertyChanged("DisableParentWindow");

            if (item.Instance.GetType() == typeof(EventCateringModel))
            {
                var model = (EventCateringModel)item.Instance;

                var window = new AddCateringItemView(Event, model);
                window.ShowDialog();


                if (window.DialogResult != null && window.DialogResult.Value)
                {
                    _event.Event.LastEditDate = DateTime.Now;
                    _event.RefreshItems();
                }
            }
            else if (item.Instance.GetType() == typeof(EventGolfModel))
            {
                var model = (EventGolfModel)item.Instance;

                var window = new AddEventGolfItemView(Event, model);
                window.ShowDialog();

                if (window.DialogResult != null && window.DialogResult.Value)
                {
                    _event.Event.LastEditDate = DateTime.Now;
                    _event.RefreshItems();
                }
            }
            else if (item.Instance.GetType() == typeof(EventRoomModel))
            {
                var model = (EventRoomModel)item.Instance;

                var window = new AddRoomItemView(Event, model);
                window.ShowDialog();

                if (window.DialogResult != null && window.DialogResult.Value)
                {
                    _event.Event.LastEditDate = DateTime.Now;
                    _event.RefreshItems();
                }
            }
            else if (item.Instance.GetType() == typeof(EventInvoiceModel))
            {
                var model = (EventInvoiceModel)item.Instance;
                var window = new AddEventInvoiceItemView(Event, model);
                window.ShowDialog();

                if (window.DialogResult != null && window.DialogResult.Value)
                {
                    _event.Event.LastEditDate = DateTime.Now;
                    _event.RefreshItems();
                }
            }
            RaisePropertyChanged("EnableParentWindow");
        }

        #endregion
    }
}