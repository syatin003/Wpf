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
    public class AddGolfViewModel : ViewModelBase
    {
        #region Fields

        private GolfModel _golf;
        private readonly IAdminDataUnit _adminDataUnit;

        #endregion

        #region Properties

        public GolfModel Golf
        {
            get { return _golf; }
            set
            {
                if (_golf == value) return;
                _golf = value;
                RaisePropertyChanged(() => Golf);
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        #endregion

        #region Constructor

        public AddGolfViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);

            Golf = GetGolfModel();
            Golf.PropertyChanged += GolfOnPropertyChanged;
        }

        #endregion

        #region Methods

        private GolfModel GetGolfModel()
        {
            return new GolfModel(new Golf() { ID = Guid.NewGuid()});
        }

        private void GolfOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            _adminDataUnit.GolfsRepository.Add(_golf.Golf);
            _adminDataUnit.SaveChanges();
        }

        private bool SaveCommandCanExecute()
        {
            return !_golf.HasErrors;
        }

        #endregion
    }
}
