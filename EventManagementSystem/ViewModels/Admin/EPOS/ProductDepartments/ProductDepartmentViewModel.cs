using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using EventManagementSystem.Views.Admin.EPOS.ProductDepartments;
using Telerik.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using EventManagementSystem.Data.Model;


namespace EventManagementSystem.ViewModels.Admin.EPOS.ProductDepartments
{
    public class ProductDepartmentViewModel : EventManagementSystem.Core.ViewModels.ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;

        private ObservableCollection<ProductDepartmentModel> _productDepartments;

        private TillDivision _tillDivision;

        #endregion

        #region Properties

        public List<ProductDepartmentModel> AllProductDepartments { get; set; }

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

        public ObservableCollection<ProductDepartmentModel> ProductDepartments
        {
            get { return _productDepartments; }
            set
            {
                if (_productDepartments == value) return;
                _productDepartments = value;
                RaisePropertyChanged(() => ProductDepartments);
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
        public RelayCommand<ProductDepartmentModel> DeleteProductDepartmentCommand { get; private set; }
        public RelayCommand<ProductDepartmentModel> EditProductDepartmentCommand { get; private set; }

        #endregion

        #region Constructors

        public ProductDepartmentViewModel(TillDivisionGroupDepartmentModel tillDivisionGroupDepartmentModel)
        {
            TillDivision = tillDivisionGroupDepartmentModel.TillDivision;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            DeleteProductDepartmentCommand = new RelayCommand<ProductDepartmentModel>(DeleteProductDepartmentCommandExecuted);
            EditProductDepartmentCommand = new RelayCommand<ProductDepartmentModel>(EditProductDepartmentCommandExecuted);

        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.ProductDepartmentsRepository.Refresh(System.Data.Entity.Core.Objects.RefreshMode.ClientWins);

            var productDepartments = await _adminDataUnit.ProductDepartmentsRepository.GetAllAsync(productDepartment => productDepartment.Till.TillDivision.ID == TillDivision.ID && productDepartment.Till.ID == TillDivision.MasterTillID);
            AllProductDepartments = new List<ProductDepartmentModel>(productDepartments.Select(x => new ProductDepartmentModel(x)));
            RefreshProductDepartments();
            IsBusy = false;
        }

        public void RefreshProductDepartments()
        {
            ProductDepartments = new ObservableCollection<ProductDepartmentModel>(AllProductDepartments.OrderBy(productDepartment => productDepartment.RecordID));
        }
        #endregion

        #region Commands

        private async void DeleteProductDepartmentCommandExecuted(ProductDepartmentModel model)
        {
            if (model == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            // Check if product Department has dependencies
            if (model.ProductDepartment.Products.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine("Sorry, we can't delete this product Department :(");
                sb.AppendLine("This product Department already in use by these products:");

                model.ProductDepartment.Products.Select(x => x.Name).ForEach(x => sb.AppendLine(string.Format("- {0}", x)));

                RaisePropertyChanged("DisableParentWindow");

                RadWindow.Alert(sb.ToString());

                RaisePropertyChanged("EnableParentWindow");

                return;
            }

            // Delete product Department

            _adminDataUnit.ProductDepartmentsRepository.Delete(model.ProductDepartment);

            await _adminDataUnit.SaveChanges();

            ProductDepartments.Remove(model);
        }
        private void EditProductDepartmentCommandExecuted(ProductDepartmentModel model)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddProductDepartmentView(model);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }
        #endregion
    }
}
