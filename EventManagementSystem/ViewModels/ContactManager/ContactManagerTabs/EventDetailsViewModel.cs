using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Core.Booking;
using EventManagementSystem.Views.Core.Booking;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs
{
    public class EventDetailsViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private EventModel _event;
        private readonly IEventDataUnit _eventsDataUnit;
        private readonly IContactsDataUnit _contactsDataUnit;

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

        public RelayCommand<EventModel> EditEventCommand { get; private set; }

        #endregion

        #region Constructor

        public EventDetailsViewModel(EventModel model)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
            _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();

            EditEventCommand = new RelayCommand<EventModel>(EditEventCommandExecuted, EditEventCommandCanExecute);

            Event = model;
        }

        #endregion

        #region Methods

        public async Task LoadLightEventDetails()
        {
            IsBusy = true;

            if (!Event.EventItems.Any())
            {
                var products = await _eventsDataUnit.EventBookedProductsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventBookedProducts = new List<EventBookedProductModel>(products.Select(x => new EventBookedProductModel(x)));

                var caterings = await _eventsDataUnit.EventCateringsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventCaterings = new List<EventCateringModel>(caterings.Select(x => new EventCateringModel(x)));

                var rooms = await _eventsDataUnit.EventRoomsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventRooms = new List<EventRoomModel>(rooms.Select(x => new EventRoomModel(x)));

                var golfs = await _eventsDataUnit.EventGolfsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventGolfs = new List<EventGolfModel>(golfs.Select(x => new EventGolfModel(x)));

                var invoices = await _eventsDataUnit.EventInvoicesRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventInvoices = new List<EventInvoiceModel>(invoices.Select(x => new EventInvoiceModel(x)));

                Event.RefreshItems();
            }

            if (!Event.EventPayments.Any())
            {
                var payments = await _eventsDataUnit.EventPaymentsRepository.GetAllAsync(x => x.EventID == Event.Event.ID);
                Event.EventPayments = new ObservableCollection<EventPaymentModel>(payments.Select(x => new EventPaymentModel(x)));
            }

            Event.RefreshResourceBookingsList();

            Event.UpdatePaymentDetails();

            IsBusy = false;
        }

        #endregion

        #region Commands

        private async void EditEventCommandExecuted(EventModel item)
        {
            // We should get event from EventDataUnit to use EventBookingView
            var events = await _eventsDataUnit.EventsRepository.GetLightEventsAsync(x => x.ID == item.Event.ID);
            var model = new EventModel(events.FirstOrDefault());

            RaisePropertyChanged("DisableParentWindow");

            var view = new BookingView(BookingViews.Event, model);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult.Value)
            {
                _contactsDataUnit.EventsRepository.Refresh();

                events = await _contactsDataUnit.EventsRepository.GetLightEventsAsync(x => x.ID == model.Event.ID);
                Event = new EventModel(events.FirstOrDefault());
                await LoadLightEventDetails();
                model.RefreshItems();
            }
        }

        private bool EditEventCommandCanExecute(EventModel arg)
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_EDIT_EVENT_ALLOWED);
        }

        #endregion
    }
}