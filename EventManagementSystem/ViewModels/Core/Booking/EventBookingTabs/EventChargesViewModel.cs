using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Charges;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using System.Windows;
using EventManagementSystem.Data.Model;
using System.Collections.Generic;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventChargesViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;
        private bool _isBusy;
        private readonly IEventDataUnit _eventsDataUnit;
        private List<EventStatus> _eventStatuses;

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

                AddInvoiceCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddChargeCommand { get; private set; }
        public RelayCommand AddInvoiceCommand { get; private set; }

        public RelayCommand<EventChargeModel> EditChargeCommand { get; private set; }
        public RelayCommand<EventChargeModel> CommitChargeCommand { get; private set; }
        public RelayCommand<EventChargeModel> UndoCommitChargeCommand { get; private set; }

        #endregion

        #region Constructor

        public EventChargesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            AddChargeCommand = new RelayCommand(AddChargeCommandExecuted);
            AddInvoiceCommand = new RelayCommand(AddInvoiceCommandExecuted, AddInvoiceCommandCanExecute);

            EditChargeCommand = new RelayCommand<EventChargeModel>(EditChargeCommandExecuted);
            CommitChargeCommand = new RelayCommand<EventChargeModel>(CommitChargeCommandExecuted);
            UndoCommitChargeCommand = new RelayCommand<EventChargeModel>(UndoCommitChargeCommandExecuted, UndoCommitChargeCommandCanExecute);
        }

        #endregion

        #region Methods

        public async void LoadEventData()
        {
            IsBusy = true;

            if (!_event.EventCharges.Any())
            {
                var charges = await _eventsDataUnit.EventChargesRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                _event.EventCharges = new ObservableCollection<EventChargeModel>(charges.Select(x => new EventChargeModel(x)));
            }
            else
            {
                //var desiredEvent = await _eventsDataUnit.EventsRepository.GetUpdatedEvent(_event.Event.ID);

                //if (desiredEvent != null && desiredEvent.LastEditDate != null && _event.LoadedTime < desiredEvent.LastEditDate)
                //{
                //    _eventsDataUnit.EventChargesRepository.Refresh();
                //    var charges = await _eventsDataUnit.EventChargesRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                //    _event.EventCharges = new ObservableCollection<EventChargeModel>(charges.Select(x => new EventChargeModel(x)));
                //}
            }

            _eventStatuses = await _eventsDataUnit.EventStatusesRepository.GetAllAsync();

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void AddChargeCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddEventChargeView(Event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.EventCharges.Add(window.ViewModel.EventCharge);
                Event.RefreshEventPrice();
                Event.UpdatePaymentDetails();
            }
        }

        private void AddInvoiceCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddInvoiceView(Event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private bool AddInvoiceCommandCanExecute()
        {
            return (_event != null && (_event.EventCharges.Any() ? _event.EventCharges.All(x => x.IsCommited) : false));
        }

        private void EditChargeCommandExecuted(EventChargeModel obj)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddEventChargeView(Event, obj);
            window.ShowDialog();
            if (window.DialogResult != null && window.DialogResult.Value)
            {
                Event.RefreshEventPrice();
                Event.UpdatePaymentDetails();
            }
            RaisePropertyChanged("EnableParentWindow");
        }

        private void UndoCommitChargeCommandExecuted(EventChargeModel obj)
        {
            obj.IsCommited = false;
            AddInvoiceCommand.RaiseCanExecuteChanged();
        }

        private bool UndoCommitChargeCommandCanExecute(EventChargeModel arg)
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_UNDO_COMMIT_CHARGE);
        }

        private void CommitChargeCommandExecuted(EventChargeModel obj)
        {
            obj.IsCommited = true;

            if (_event.EventStatus.Name != "Confirmed")
            {
                // If event status is not Confirmed then we can change event status to Confirmed

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
            AddInvoiceCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
