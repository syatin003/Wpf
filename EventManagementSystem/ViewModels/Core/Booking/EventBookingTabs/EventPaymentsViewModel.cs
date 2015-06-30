using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Payments;
using Microsoft.Practices.Unity;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventPaymentsViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;
        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;

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

        public RelayCommand AddPaymentCommand { get; private set; }
        public RelayCommand<EventPaymentModel> DeletePaymentCommand { get; private set; }
        public RelayCommand<EventPaymentModel> EditPaymentCommand { get; private set; }

        #endregion

        #region Constructor

        public EventPaymentsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            AddPaymentCommand = new RelayCommand(AddPaymentCommandExecuted);
            DeletePaymentCommand = new RelayCommand<EventPaymentModel>(DeletePaymentCommandExecuted);
            EditPaymentCommand = new RelayCommand<EventPaymentModel>(EditPaymentCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadEventData()
        {
            IsBusy = true;

            if (!_event.EventPayments.Any())
            {
                var payments = await _eventsDataUnit.EventPaymentsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                _event.EventPayments = new ObservableCollection<EventPaymentModel>(payments.Select(x => new EventPaymentModel(x)));
            }
            else
            {
                //var desiredEvent = await _eventsDataUnit.EventsRepository.GetUpdatedEvent(_event.Event.ID);

                //if (desiredEvent != null && desiredEvent.LastEditDate != null && _event.LoadedTime < desiredEvent.LastEditDate)
                //{
                //    _eventsDataUnit.EventPaymentsRepository.Refresh();
                //    var payments = await _eventsDataUnit.EventPaymentsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                //    _event.EventPayments = new ObservableCollection<EventPaymentModel>(payments.Select(x => new EventPaymentModel(x)));
                //}
            }

            _event.UpdatePaymentDetails();

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void AddPaymentCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddEventPaymentView(Event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.EventPayments.Add(window.ViewModel.EventPayment);
                _event.UpdatePaymentDetails();
                RaisePropertyChanged("UpdateEventStatus");
            }
        }

        private void EditPaymentCommandExecuted(EventPaymentModel item)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddEventPaymentView(Event, item);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void DeletePaymentCommandExecuted(EventPaymentModel item)
        {
            _event.EventPayments.Remove(item);
            _eventsDataUnit.EventPaymentsRepository.Delete(item.EventPayment);

            _event.UpdatePaymentDetails();
        }

        #endregion
    }
}
