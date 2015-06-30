using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class EnquiryStatusesViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isDataLoadedOnce;
        private EnquiryStatus _enquiryStatus;

        #endregion

        #region Properties

        public bool IsEditable { get; set; }


        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public EnquiryStatus EnquiryStatus
        {
            get { return _enquiryStatus; }
            set
            {
                if (_enquiryStatus == value) return;
                _enquiryStatus = value;
                RaisePropertyChanged(() => _enquiryStatus);
            }
        }
        private bool _isDirty;

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

        public RelayCommand SaveChangesCommand { get; private set; }

        #endregion

        #region Constructor

        public EnquiryStatusesViewModel(EnquiryStatus status)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            EnquiryStatus = status;
            IsEditable = status.Status != "Booked";
            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);

            EnquiryStatus.PropertyChanged += EnquiryStatus_PropertyChanged;

        }

        public void Refresh()
        {
            if (IsDirty)
            {
                _adminDataUnit.EnquiryStatusesRepository.Refresh(EnquiryStatus);
                IsDirty = false;
            }
        }


        private void EnquiryStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var enquiryStatus = await _adminDataUnit.EnquiryStatusesRepository.GetUpdatedEnquiryStatus(_enquiryStatus.ID);

            EnquiryStatus = enquiryStatus;

            OnDataLoaded();
        }

        private void OnDataLoaded()
        {
            IsBusy = false;
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
