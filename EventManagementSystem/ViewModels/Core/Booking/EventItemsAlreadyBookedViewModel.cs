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
using EventManagementSystem.Core.Serialization;
using System.Linq;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EventManagementSystem.ViewModels.Core.Booking
{
    public class EventItemsAlreadyBookedViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;
        private ObservableCollection<EventItemModel> _eventItemsAlreadyBooked;
        private List<EventCateringModel> _alreadyBookedCaterings;
        private List<EventRoomModel> _alreadyBookedRooms;
        private List<EventGolfModel> _alreadyBookedGolfs;

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
        public ObservableCollection<EventItemModel> EventItemsAlreadyBooked
        {
            get { return _eventItemsAlreadyBooked; }
            set
            {
                if (_eventItemsAlreadyBooked == value) return;
                _eventItemsAlreadyBooked = value;
                RaisePropertyChanged(() => EventItemsAlreadyBooked);
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

        public RelayCommand<EventItemModel> EditItemCommand { get; set; }
        public RelayCommand<EventItemModel> DeleteItemCommand { get; set; }

        #endregion

        #region Constructor

        public EventItemsAlreadyBookedViewModel(EventModel model, List<EventCateringModel> alreadyBookedCaterings, List<EventRoomModel> alreadyBookedRooms, List<EventGolfModel> alreadyBookedGolfs, ObservableCollection<EventItemModel> eventItemsAlreadyBooked)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            DeleteItemCommand = new RelayCommand<EventItemModel>(DeleteItemCommandExecuted);
            EditItemCommand = new RelayCommand<EventItemModel>(EditItemCommandExecuted);
            Event = model;
            AlreadyBookedCaterings = alreadyBookedCaterings;
            AlreadyBookedRooms = alreadyBookedRooms;
            AlreadyBookedGolfs = alreadyBookedGolfs;
            EventItemsAlreadyBooked = eventItemsAlreadyBooked;

        }

        #endregion


        #region Commands

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
                AlreadyBookedCaterings.Remove(model);
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
                AlreadyBookedGolfs.Remove(model);
                _event.EventGolfs.Remove(model);
                _eventsDataUnit.EventGolfsRepository.Delete(model.EventGolf);
            }
            else if (item.Instance.GetType() == typeof(EventRoomModel))
            {
                var model = (EventRoomModel)item.Instance;

                // Remove booked products
                model.EventBookedProducts.ForEach(RemoveEventBookedProductAndCharges);
                AlreadyBookedRooms.Remove(model);
                _event.EventRooms.Remove(model);
                _eventsDataUnit.EventRoomsRepository.Delete(model.EventRoom);
            }
            EventItemsAlreadyBooked.Remove(item);
            _event.Event.LastEditDate = DateTime.Now;
            _event.RefreshItems();
            if (EventItemsAlreadyBooked.Count == 0)
            {
                RaisePropertyChanged("CloseDialog");
            }
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

                var window = new AddCateringItemView(Event, model, AlreadyBookedCaterings, AlreadyBookedRooms);
                window.ShowDialog();


                if (window.DialogResult != null && window.DialogResult.Value)
                {
                    AlreadyBookedCaterings.Remove(model);
                    EventItemsAlreadyBooked.Remove(item);
                    _event.Event.LastEditDate = DateTime.Now;
                    _event.RefreshItems();
                }
            }
            else if (item.Instance.GetType() == typeof(EventGolfModel))
            {
                var model = (EventGolfModel)item.Instance;

                var window = new AddEventGolfItemView(Event, model, AlreadyBookedGolfs);
                window.ShowDialog();

                if (window.DialogResult != null && window.DialogResult.Value)
                {
                    AlreadyBookedGolfs.Remove(model);
                    EventItemsAlreadyBooked.Remove(item);
                    _event.Event.LastEditDate = DateTime.Now;
                    _event.RefreshItems();
                }
            }
            else if (item.Instance.GetType() == typeof(EventRoomModel))
            {
                var model = (EventRoomModel)item.Instance;

                var window = new AddRoomItemView(Event, model, AlreadyBookedCaterings, AlreadyBookedRooms);
                window.ShowDialog();

                if (window.DialogResult != null && window.DialogResult.Value)
                {
                    AlreadyBookedRooms.Remove(model);
                    EventItemsAlreadyBooked.Remove(item);
                    _event.Event.LastEditDate = DateTime.Now;
                    _event.RefreshItems();
                }
            }

            RaisePropertyChanged("EnableParentWindow");
            if (EventItemsAlreadyBooked.Count == 0)
            {
                RaisePropertyChanged("CloseDialog");
            }
        }

        #endregion
    }
}