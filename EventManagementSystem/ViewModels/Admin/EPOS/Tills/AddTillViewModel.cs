using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Security;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.EPOS.Tills
{
    public class AddTillViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private ObservableCollection<TillDivision> _tillDivisions;

        private TillModel _till;
        private bool _isBusy;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public TillModel Till
        {
            get { return _till; }
            set
            {
                if (_till == value) return;
                _till = value;
                RaisePropertyChanged(() => Till);
            }
        }

        public ObservableCollection<TillDivision> TillDivisions
        {
            get { return _tillDivisions; }
            set
            {
                if (_tillDivisions == value) return;
                _tillDivisions = value;
                RaisePropertyChanged(() => TillDivisions);
            }
        }

        public RelayCommand OkCommand { get; private set; }

        #endregion

        #region Constructors

        public AddTillViewModel(ObservableCollection<TillDivisionModel> tillDivisions)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            TillDivisions = new ObservableCollection<TillDivision>(tillDivisions.Select(tillDivision => tillDivision.TillDivision));
            OkCommand = new RelayCommand(OkCommandExecuted, OkCommandCanExecute);

            AddTill();
        }

        #endregion

        #region Methods

        private void AddTill()
        {
            var tillModel = new TillModel(new Till()
            {
                ID = Guid.NewGuid()
            });

            tillModel.PropertyChanged += OnTillPropertyChanged;
            Till = tillModel;
        }

        private void OnTillPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OkCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void OkCommandExecuted()
        {
            if (Till.IsMaster)
            {
                Till.TillDivision.Till = Till.Till;
                Till.TillDivision.MasterTillID = Till.Till.ID;
            }
            _adminDataUnit.TillsRepository.Add(Till.Till);
            _adminDataUnit.SaveChanges();
        }

        private bool OkCommandCanExecute()
        {
            return !Till.HasErrors;
        }

        #endregion
    }
}
