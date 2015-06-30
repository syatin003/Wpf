using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Charges
{
    public class AddEventChargeViewModel : ViewModelBase
    {
        #region Fields

        private readonly EventModel _event;
        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;
        private EventChargeModel _eventCharge;
        private ObservableCollection<Product> _products;
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

        public EventChargeModel EventCharge
        {
            get { return _eventCharge; }
            set
            {
                if (_eventCharge == value) return;
                _eventCharge = value;
                RaisePropertyChanged(() => EventCharge);
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

        public RelayCommand SubmitCommand { get; private set; }

        #endregion

        #region Constructor

        public AddEventChargeViewModel(EventModel eventModel, EventChargeModel charge)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);

            ProcessCharge(charge);
        }

        #endregion

        #region Methods

        private void ProcessCharge(EventChargeModel model)
        {
            _isEditMode = (model != null);

            EventCharge = (_isEditMode) ? model : GetEventCharge();
            EventCharge.PropertyChanged += EventChargeOnPropertyChanged;
        }

        private EventChargeModel GetEventCharge()
        {
            return new EventChargeModel(new EventCharge()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                ShowInInvoice = true
            });
        }

        private void EventChargeOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var products = await _eventsDataUnit.ProductsRepository.GetAllAsync();
            Products = new ObservableCollection<Product>(
                        (_event.Event.EventType == null)
                            ? products
                            : products.Where(x => x.ProductEventTypes.Any(e => e.EventType.ID == _event.Event.EventType.ID)));

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void SubmitCommandExecuted()
        {
            if (!_isEditMode)
            {
                _eventsDataUnit.EventChargesRepository.Add(EventCharge.EventCharge);
            }
        }

        private bool SubmitCommandCanExecute()
        {
            return !EventCharge.HasErrors;
        }

        #endregion
    }
}