using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Resources
{
    public class RoomViewModel : ViewModelBase
    {
        #region Fields

        private RoomModel _room;
        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isDirty;

        #endregion

        #region Properties

        public RoomModel Room
        {
            get { return _room; }
            set
            {
                if (_room == value) return;
                _room = value;
                RaisePropertyChanged(() => Room);
            }
        }

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

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                RaisePropertyChanged(() => IsDirty);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand EditFacilitiesCommand { get; private set; }

        #endregion

        #region Constructor

        public RoomViewModel(RoomModel roomModel)
        {
            if (IsDirty)
                IsDirty = false;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            EditFacilitiesCommand = new RelayCommand(EditFacilitiesCommandExecuted);

            Room = roomModel;
            Room.PropertyChanged += OnPropertyChanged;
            Room.Room.PropertyChanged += OnPropertyChanged;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            if (IsDirty)
                _adminDataUnit.RoomsRepository.Refresh(Room.Room);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            IsDirty = true;
        }

        #endregion

        #region Commands

        private void EditFacilitiesCommandExecuted()
        {
            // TODO: Rework this part of code
        }

        private void SaveCommandExecuted()
        {
            IsBusy = true;

            IsDirty = false;
            _adminDataUnit.SaveChanges();

            IsBusy = false;
        }

        private bool SaveCommandCanExecute()
        {
            return !_room.HasErrors && IsDirty;
        }

        #endregion
    }
}
