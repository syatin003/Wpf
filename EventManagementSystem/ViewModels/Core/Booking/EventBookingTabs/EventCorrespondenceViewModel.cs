using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Correspondence;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventCorrespondenceViewModel : ViewModelBase
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

        public RelayCommand SendEmailCommand { get; private set; }

        public RelayCommand<CorrespondenceModel> ShowCorrespondenceCommand { get; private set; }

        #endregion

        #region Constructor

        public EventCorrespondenceViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SendEmailCommand = new RelayCommand(SendEmailCommandExecuted);
            ShowCorrespondenceCommand = new RelayCommand<CorrespondenceModel>(ShowCorrespondenceCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadEventData()
        {
            IsBusy = true;

            if (!_event.Correspondences.Any() || !_event.Documents.Any())
            {
                var correspondence = await _eventsDataUnit.CorresponcencesRepository.GetAllAsync(x => x.OwnerID == _event.Event.ID);
                _event.Correspondences = new ObservableCollection<CorrespondenceModel>(
                    correspondence.OrderByDescending(x => x.Date).Select(x => new CorrespondenceModel(x)));

                _event.Documents = await _eventsDataUnit.DocumentsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                var Documents = await _eventsDataUnit.DocumentsRepository.GetAllAsync();

                foreach (var eventCorresponcence in _event.Correspondences)
                {
                    foreach (var cd in eventCorresponcence.Correspondence.CorrespondenceDocuments)
                    {
                        eventCorresponcence.Documents.Add(Documents.FirstOrDefault(x => x.ID == cd.DocumentID));
                    }
                }
            }
            else
            {
                //var desiredEvent = await _eventsDataUnit.EventsRepository.GetUpdatedEvent(_event.Event.ID);

                //if (desiredEvent != null && desiredEvent.LastEditDate != null && _event.LoadedTime < desiredEvent.LastEditDate)
                //{
                //    _eventsDataUnit.CorresponcencesRepository.Refresh();
                //    var correspondence = await _eventsDataUnit.CorresponcencesRepository.GetAllAsync(x => x.OwnerID == _event.Event.ID);
                //    _event.Correspondences = new ObservableCollection<CorrespondenceModel>(
                //        correspondence.OrderByDescending(x => x.Date).Select(x => new CorrespondenceModel(x)));

                //    _eventsDataUnit.DocumentsRepository.Refresh();
                //    _event.Documents = await _eventsDataUnit.DocumentsRepository.GetAllAsync(x => x.EventID == _event.Event.ID);
                // }
            }

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void SendEmailCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new SendEventMailView(_event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void ShowCorrespondenceCommandExecuted(CorrespondenceModel obj)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new SendEventMailView(_event, obj);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        #endregion
    }
}