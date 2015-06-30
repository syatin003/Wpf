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

namespace EventManagementSystem.ViewModels.Admin.EPOS.ProductDepartments
{
    public class AddProductDepartmentViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isEditMode;
        private ProductDepartmentModel _productDepartment;
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

        public ProductDepartmentModel ProductDepartment
        {
            get { return _productDepartment; }
            set
            {
                if (_productDepartment == value) return;
                _productDepartment = value;
                RaisePropertyChanged(() => ProductDepartment);
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

        public AddProductDepartmentViewModel(ProductDepartmentModel productDepartmentModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            OKCommand = new RelayCommand(OKCommandExecuted, OKCommandCanExecute);

            ProcessProductDepartment(productDepartmentModel);
        }

        #endregion

        #region Methods

        private void ProcessProductDepartment(ProductDepartmentModel productDepartmentModel)
        {
            _isEditMode = (productDepartmentModel != null);
            ProductDepartment = productDepartmentModel ?? GetNewProductDepartment();
            if (_isEditMode)
                TillDivision = productDepartmentModel.Till.TillDivision;
            ProductDepartment.PropertyChanged += OnProductDepartmentPropertyChanged;
        }

        private ProductDepartmentModel GetNewProductDepartment()
        {
            var productDepartmentModel = new ProductDepartmentModel(new ProductDepartment()
             {
                 ID = Guid.NewGuid()
             });
            return productDepartmentModel;
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

        private void OnProductDepartmentPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OKCommand.RaiseCanExecuteChanged();
        }
        private void SendDataToRelevantTill()
        {
            var productDepartmentModel = new EventManagementSystem.Models.TillDomainObjects.ProductDepartmentModel
            {
                Record = ProductDepartment.RecordID,
                Name = ProductDepartment.Name
            };

            string tillName = ProductDepartment.Till.Name;
            string ipAddress = ProductDepartment.Till.IPAddress;
            TillsCommunicationService.SetProductDepartment(productDepartmentModel, tillName, ipAddress);

            IsBusy = false;
            RaisePropertyChanged("CloseDialog");
        }
        #endregion

        #region Commands

        private async void OKCommandExecuted()
        {
            IsBusy = true;

            if (!_isEditMode)
                _adminDataUnit.ProductDepartmentsRepository.Add(ProductDepartment.ProductDepartment);

            await _adminDataUnit.SaveChanges();

            SendDataToRelevantTill();
        }

        private bool OKCommandCanExecute()
        {
            return !ProductDepartment.HasErrors;
        }

        #endregion
    }
}
