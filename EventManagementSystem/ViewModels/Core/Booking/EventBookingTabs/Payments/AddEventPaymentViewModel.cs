using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Payments
{
    public class AddEventPaymentViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventsDataUnit;
        private readonly EventModel _event;
        private bool _isBusy;
        private ObservableCollection<User> _users;
        private ObservableCollection<PaymentMethod> _paymentMethods;
        private List<EventStatus> _eventStatuses;
        private EventPaymentModel _eventPayment;
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

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                if (_users == value) return;
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public ObservableCollection<PaymentMethod> PaymentMethods
        {
            get { return _paymentMethods; }
            set
            {
                if (_paymentMethods == value) return;
                _paymentMethods = value;
                RaisePropertyChanged(() => PaymentMethods);
            }
        }

        public EventPaymentModel EventPayment
        {
            get { return _eventPayment; }
            set
            {
                if (_eventPayment == value) return;
                _eventPayment = value;
                RaisePropertyChanged(() => EventPayment);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructors

        public AddEventPaymentViewModel(EventModel eventModel, EventPaymentModel model)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessPayment(model);
        }

        #endregion

        #region Methods

        private void ProcessPayment(EventPaymentModel model)
        {
            _isEditMode = (model != null);

            EventPayment = (_isEditMode) ? model : GetPayment();
            EventPayment.PropertyChanged += EventPaymentOnPropertyChanged;
        }

        private EventPaymentModel GetPayment()
        {
            return new EventPaymentModel(new EventPayment()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                CameFrom = "Event"
            });
        }

        private void EventPaymentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var methods = await _eventsDataUnit.PaymentMethodsRepository.GetAllAsync();
            PaymentMethods = new ObservableCollection<PaymentMethod>(methods);

            var users = await _eventsDataUnit.UsersRepository.GetUsersAsync();
            Users = new ObservableCollection<User>(users);

            _eventStatuses = await _eventsDataUnit.EventStatusesRepository.GetAllAsync();

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void SubmitCommandExecuted()
        {
            if (!_isEditMode)
            {
                _eventsDataUnit.EventPaymentsRepository.Add(EventPayment.EventPayment);
            }

            if (_eventPayment.IsDeposit && _event.EventStatus.Name == "Provisional")
            {
                // If this is deposit payment and event status is Provisional then we can change event status to Confirmed

                bool? dialogResult = null;

                RadWindow.Confirm(new DialogParameters()
                {
                    Owner = Application.Current.MainWindow,
                    Content = "Would you like to change event status to confirmed?",
                    Closed = (sender, args) => { dialogResult = args.DialogResult; }
                });

                if (dialogResult == true)
                {
                    _event.EventStatus = _eventStatuses.FirstOrDefault(x => x.Name == "Confirmed");
                }
            }
        }

        private bool SubmitCommandCanExecute()
        {
            return !EventPayment.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            _eventsDataUnit.RevertChanges();

            EventPayment.Refresh();
        }

        #endregion
    }
}