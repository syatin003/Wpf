using System.Collections.ObjectModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class UnlockEventsViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private ObservableCollection<Event> _events;
        private readonly IAdminDataUnit _adminDataUnit;

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

        public ObservableCollection<Event> Events
        {
            get { return _events; }
            set
            {
                if (_events == value) return;
                _events = value;
                RaisePropertyChanged(() => Events);
            }
        }

        public RelayCommand<Event> UnlockEventCommand { get; private set; }

        #endregion

        #region Constructor

        public UnlockEventsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            UnlockEventCommand = new RelayCommand<Event>(UnlockEventCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var events = await _adminDataUnit.EventsRepository.GetLightEventsAsync(x => !x.IsDeleted && x.LockedUserID != null);
            Events = new ObservableCollection<Event>(events);

            IsBusy = false;
        }

        #endregion

        #region Commands

        private async void UnlockEventCommandExecuted(Event @event)
        {
            @event.LockedUserID = null;
            await _adminDataUnit.SaveChanges();

            LoadData();
        }

        #endregion
    }
}
