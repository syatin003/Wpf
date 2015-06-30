using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class ActivitySettingsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private object _selectedObject;
        private bool _isCRMPropertySelected;
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

        public bool IsCRMPropertySelected
        {
            get { return _isCRMPropertySelected; }
            set
            {
                _isCRMPropertySelected = value;
                RaisePropertyChanged(() => IsCRMPropertySelected);
            }
        }

        public object SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (_selectedObject == value) return;
                _selectedObject = value;
                RaisePropertyChanged(() => SelectedObject);           
              
            }
        }   

        #endregion

        #region Constructor

        public ActivitySettingsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

          //  SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            IsBusy = true;
            OnDataLoaded();
        }

/*        private void OnLoadEventTypes()
        {
            _adminDataUnit.EnquiryStatusesRepository.GetAllAsync().Subscribe(
                statuses => EnquiryStatuses = new ObservableCollection<EnquiryStatus>(statuses.OrderBy(x => x.Status).ToList()), OnLoadEventStatuses);
        }

        private void OnLoadEventStatuses()
        {
            _adminDataUnit.EnquiryReceiveMethodsRepository.GetAllAsync().Subscribe(
                receivedMethods => EnquiryReceivedMethods = new ObservableCollection<EnquiryReceiveMethod>(receivedMethods.OrderBy(x => x.ReceiveMethod).ToList()), OnDataLoaded);
        }*/

        private void OnDataLoaded()
        {
            IsBusy = false;
        }

        #endregion

        #region Commands

        #endregion
    }
}
