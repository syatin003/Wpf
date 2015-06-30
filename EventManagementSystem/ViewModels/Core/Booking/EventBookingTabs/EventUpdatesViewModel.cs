using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventUpdatesViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;
        private bool _isBusy;
        private readonly IEventDataUnit _eventsDataUnit;

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

        #endregion

        #region Constructor

        public EventUpdatesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
        }

        #endregion

        #region Methods

        public async void LoadEventData()
        {
            IsBusy = true;

            if (!_event.EventUpdates.Any())
            {
                var updates = await _eventsDataUnit.EventUpdatesRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                _event.EventUpdates = new ObservableCollection<EventUpdate>(updates.OrderByDescending(x => x.Date));
            }
            else
            {
                //var desiredEvent = await _eventsDataUnit.EventsRepository.GetUpdatedEvent(_event.Event.ID);

                //if (desiredEvent != null && desiredEvent.LastEditDate != null && _event.LoadedTime < desiredEvent.LastEditDate)
                //{
                //    _eventsDataUnit.EventUpdatesRepository.Refresh();
                //    var updates = await _eventsDataUnit.EventUpdatesRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                //    _event.EventUpdates = new ObservableCollection<EventUpdate>(updates.OrderByDescending(x => x.Date));
                //}
            }

            IsBusy = false;
        }

        #endregion
    }
}
