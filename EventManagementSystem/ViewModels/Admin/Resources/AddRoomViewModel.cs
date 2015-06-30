using System;
using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Resources
{
    public class AddRoomViewModel : ViewModelBase
    {
        #region Fields

        private RoomModel _room;
        private readonly IAdminDataUnit _adminDataUnit;

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

        public RelayCommand SaveCommand { get; private set; }

        #endregion

        #region Constructor

        public AddRoomViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);

            Room = GetRoomModel();
            Room.PropertyChanged += RoomOnPropertyChanged;
        }

        #endregion

        #region Methods

        private RoomModel GetRoomModel()
        {
            return new RoomModel(new Room() { ID = Guid.NewGuid() });
        }

        private void RoomOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            _adminDataUnit.RoomsRepository.Add(_room.Room);
            _adminDataUnit.SaveChanges();
        }

        private bool SaveCommandCanExecute()
        {
            return !_room.HasErrors;
        }

        #endregion
    }
}
