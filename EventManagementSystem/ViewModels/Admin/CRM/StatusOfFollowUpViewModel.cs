using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class StatusOfFollowUpViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private FollowUpStatus _followUpStatus;
        private bool _isDataLoadedOnce;
        private bool _isDirty;


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

        public FollowUpStatus FollowUpStatus
        {
            get { return _followUpStatus; }
            set
            {
                if (_followUpStatus == value) return;
                _followUpStatus = value;
                RaisePropertyChanged(() => FollowUpStatus);
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
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand SaveChangesCommand { get; set; }

        #endregion

        #region Constructor

        public StatusOfFollowUpViewModel(FollowUpStatus followUpStatus)
        {
            IsDirty = false;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            _followUpStatus = followUpStatus;

            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);

            FollowUpStatus.PropertyChanged += StatusOfFollowUp_PropertyChanged;
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var followUpStatus = await _adminDataUnit.FollowUpStatusesRepository.GetUpdatedFollowUpStatus(_followUpStatus.ID);

            FollowUpStatus = followUpStatus;
            //  FollowUpStatuses = new ObservableCollection<FollowUpStatus>(followUpStatuses.OrderBy(x => x.NumberOfDays));

            IsBusy = false;
        }

        public void Refresh()
        {
            if (IsDirty)
                _adminDataUnit.FollowUpStatusesRepository.Refresh(FollowUpStatus);

        }

        private void StatusOfFollowUp_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        #endregion

        #region Commands

        private void SaveChangesCommandExecuted()
        {
            IsDirty = false;

            _adminDataUnit.SaveChanges();
        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsDirty;
        }

        #endregion
    }
}
