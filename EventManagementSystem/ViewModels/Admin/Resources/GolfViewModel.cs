using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;
using EventManagementSystem.Views.Admin.Resources;
using System.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace EventManagementSystem.ViewModels.Admin.Resources
{
    public class GolfViewModel : ViewModelBase
    {
        #region Fields

        private GolfModel _golf;
        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isGolfFollowResourcesAvailable;
        private bool _isBusy;
        private bool _isDirty;


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
        public bool IsGolfFollowResourcesAvailable
        {
            get
            {
                if (Golf.AvailableGolfs == null) return false;
                return Golf.AvailableGolfs.Count > 0;
            }
        }

        public bool IsBusy
        {
            get
            { return _isBusy; }
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
        public RelayCommand EditFollowResourceCommand { get; private set; }

        #endregion

        #region Constructor

        public GolfViewModel(GolfModel golfModel)
        {
            if (IsDirty)
                IsDirty = false;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            EditFollowResourceCommand = new RelayCommand(EditFollowResourceCommandExecuted);
            Golf = golfModel;
            Golf.PropertyChanged += OnPropertyChanged;
            Golf.Golf.PropertyChanged += OnPropertyChanged;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            if (IsDirty)
                _adminDataUnit.GolfsRepository.Refresh(Golf.Golf);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            IsDirty = true;
        }

        #endregion

        #region Commands

        private async void SaveCommandExecuted()
        {
            IsBusy = true;

            IsDirty = false;
            await _adminDataUnit.SaveChanges();

            IsBusy = false;
        }

        private void EditFollowResourceCommandExecuted()
        {
            Guid? turnDefaultId = new Guid();
            if (Golf.Golf.TurnDefault != null)
                turnDefaultId = Golf.Golf.TurnDefault;
            RaisePropertyChanged("DisableParentWindow");
            var window = new GolfFollowResource(Golf);
            window.ShowDialog();
            RaisePropertyChanged("EnableParentWindow");
            if (window.DialogResult != null && window.DialogResult == true)
            {
                Golf.AvailableGolfs = window.ViewModel.GolfFollowResources;
            }
            Golf.Refresh();
            if (turnDefaultId != null)
                SetGolfTurnDefault(Golf, turnDefaultId, window.ViewModel.AvailableGolfResources);
            RaisePropertyChanged(() => IsGolfFollowResourcesAvailable);
        }

        private void SetGolfTurnDefault(GolfModel model, Guid? turnDefaultId, ObservableCollection<GolfModel> Golfs)
        {
            if (turnDefaultId != null)
            {
                if (Golfs.Any(golf => golf.Golf.ID == turnDefaultId))
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        new Action(
                            () =>
                            {
                                model.TurnDefaultGolf =
                                    Golfs.FirstOrDefault(golf => golf.Golf.ID == turnDefaultId);
                            }));
                }
            }
        }

        private bool SaveCommandCanExecute()
        {
            return !_golf.HasErrors && IsDirty;
        }

        #endregion
    }
}
