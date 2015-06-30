using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.ViewModels.Admin.EPOS.Tills
{
    public class TillViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private TillModel _till;
        private ObservableCollection<TillDivision> _tillDivisions;
        public ObservableCollection<MembershipCategoryGroupDefault> _membershipCategoryGroupDefaults;
        private bool _isDirty;

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

        public ObservableCollection<MembershipCategoryGroupDefault> MembershipCategoryGroupDefaults
        {
            get { return _membershipCategoryGroupDefaults; }
            set
            {
                if (_membershipCategoryGroupDefaults == value) return;
                _membershipCategoryGroupDefaults = value;
                RaisePropertyChanged(() => MembershipCategoryGroupDefaults);
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
            }
        }
        public RelayCommand SaveCommand { get; private set; }

        #endregion

        #region Constructor

        public TillViewModel(TillModel tillModel, ObservableCollection<TillDivisionModel> tillDivisions)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            Till = tillModel;
            TillDivisions = new ObservableCollection<TillDivision>(tillDivisions.Select(tillDivision => tillDivision.TillDivision));
            Till.PropertyChanged += TillOnPropertyChanged;
            Till.Till.PropertyChanged += TillOnPropertyChanged;
        }

        #endregion

        #region Methods

        public void RefreshTill()
        {
            if (IsDirty)
                _adminDataUnit.TillsRepository.Refresh(Till.Till);
        }

        private void TillOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            IsDirty = true;
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private async void SaveCommandExecuted()
        {
            if (Till.IsMaster)
            {
                Till.TillDivision.Till = Till.Till;
                Till.TillDivision.MasterTillID = Till.Till.ID;
            }
            else if (Till.TillDivision.Till.ID == Till.Till.ID)
            {
                Till.TillDivision.Till = null;
                Till.TillDivision.MasterTillID = null;
            }
            await _adminDataUnit.SaveChanges();
            SaveCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged("RefreshTills");
        }

        private bool SaveCommandCanExecute()
        {
            return !_till.HasErrors && IsDirty;
        }

        #endregion

    }
}
