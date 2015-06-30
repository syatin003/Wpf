using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System;
using Microsoft.Practices.Unity;
using EventManagementSystem.Data.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using EventManagementSystem.Services;

namespace EventManagementSystem.ViewModels.Admin.EPOS.ProductGroups
{
    public class AddProductGroupViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isEditMode;
        private ProductGroupModel _productGroup;
        private ObservableCollection<Till> _tills;
        private TillDivision _tillDivision;

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

        public ProductGroupModel ProductGroup
        {
            get { return _productGroup; }
            set
            {
                if (_productGroup == value) return;
                _productGroup = value;
                RaisePropertyChanged(() => ProductGroup);
            }
        }

        public ObservableCollection<Till> Tills
        {
            get { return _tills; }
            set
            {
                if (_tills == value) return;
                _tills = value;
                RaisePropertyChanged(() => Tills);
            }
        }

        public TillDivision TillDivision
        {
            get { return _tillDivision; }
            set
            {
                if (_tillDivision == value) return;
                _tillDivision = value;
                RaisePropertyChanged(() => TillDivision);
            }
        }

        public RelayCommand OKCommand { get; private set; }

        #endregion

        #region Constructors

        public AddProductGroupViewModel(ProductGroupModel productGroupModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            OKCommand = new RelayCommand(OKCommandExecuted, OKCommandCanExecute);

            ProcessProductGroup(productGroupModel);
        }

        #endregion

        #region Methods

        private void ProcessProductGroup(ProductGroupModel productGroupModel)
        {
            _isEditMode = (productGroupModel != null);
            ProductGroup = productGroupModel ?? GetNewProductGroup();
            if (_isEditMode)
                TillDivision = productGroupModel.Till.TillDivision;
            ProductGroup.PropertyChanged += OnProductGroupPropertyChanged;
        }

        private ProductGroupModel GetNewProductGroup()
        {
            var productGroupModel = new ProductGroupModel(new ProductGroup()
             {
                 ID = Guid.NewGuid()
             });
            return productGroupModel;
        }
        public async void LoadData()
        {
            IsBusy = true;

            if (_isEditMode)
            {
                var tills = await _adminDataUnit.TillsRepository.GetAllAsync(till => till.TillDivision.ID == TillDivision.ID);
                Tills = new ObservableCollection<Till>(tills);
            }
            else
            {
                var tills = await _adminDataUnit.TillsRepository.GetAllAsync();
                Tills = new ObservableCollection<Till>(tills);
            }
            IsBusy = false;
        }

        private void OnProductGroupPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OKCommand.RaiseCanExecuteChanged();
        }
        private void SendDataToRelevantTill()
        {
            var productGroupModel = new EventManagementSystem.Models.TillDomainObjects.ProductGroupModel
            {
                Record = ProductGroup.RecordID,
                Name = ProductGroup.Name
            };

            string tillName = ProductGroup.Till.Name;
            string ipAddress = ProductGroup.Till.IPAddress;
            TillsCommunicationService.SetProductGroup(productGroupModel, tillName, ipAddress);

            IsBusy = false;
            RaisePropertyChanged("CloseDialog");
        }
        #endregion

        #region Commands

        private async void OKCommandExecuted()
        {
            IsBusy = true;
            if (!_isEditMode)
                _adminDataUnit.ProductGroupsRepository.Add(ProductGroup.ProductGroup);

            await _adminDataUnit.SaveChanges();

            SendDataToRelevantTill();
        }

        private bool OKCommandCanExecute()
        {
            return !ProductGroup.HasErrors;
        }

        #endregion
    }
}
