using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using EventManagementSystem.Views.Admin.EPOS.ProductGroups;
using Telerik.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using EventManagementSystem.Data.Model;


namespace EventManagementSystem.ViewModels.Admin.EPOS.ProductGroups
{
    public class ProductGroupViewModel : EventManagementSystem.Core.ViewModels.ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;

        private ObservableCollection<ProductGroupModel> _productGroups;

        private TillDivision _tillDivision;

        #endregion

        #region Properties

        public List<ProductGroupModel> AllProductGroups { get; set; }

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

        public ObservableCollection<ProductGroupModel> ProductGroups
        {
            get { return _productGroups; }
            set
            {
                if (_productGroups == value) return;
                _productGroups = value;
                RaisePropertyChanged(() => ProductGroups);
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
        public RelayCommand<ProductGroupModel> DeleteProductGroupCommand { get; private set; }
        public RelayCommand<ProductGroupModel> EditProductGroupCommand { get; private set; }

        #endregion

        #region Constructors

        public ProductGroupViewModel(TillDivisionGroupDepartmentModel tillDivisionGroupDepartmentModel)
        {
            TillDivision = tillDivisionGroupDepartmentModel.TillDivision;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            DeleteProductGroupCommand = new RelayCommand<ProductGroupModel>(DeleteProductGroupCommandExecuted);
            EditProductGroupCommand = new RelayCommand<ProductGroupModel>(EditProductGroupCommandExecuted);

        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.ProductGroupsRepository.Refresh(System.Data.Entity.Core.Objects.RefreshMode.ClientWins);

            var productGroups = await _adminDataUnit.ProductGroupsRepository.GetAllAsync(productGroup => productGroup.Till.TillDivision.ID == TillDivision.ID && productGroup.Till.ID == TillDivision.MasterTillID);
            AllProductGroups = new List<ProductGroupModel>(productGroups.Select(x => new ProductGroupModel(x)));
            RefreshProductGroups();
            IsBusy = false;
        }

        public void RefreshProductGroups()
        {
            ProductGroups = new ObservableCollection<ProductGroupModel>(AllProductGroups.OrderBy(productGroup => productGroup.RecordID));
        }
        #endregion

        #region Commands

        private async void DeleteProductGroupCommandExecuted(ProductGroupModel productGroupModel)
        {
            if (productGroupModel == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            // Check if product Group has dependencies
            if (productGroupModel.ProductGroup.Products.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine("Sorry, we can't delete this product group :(");
                sb.AppendLine("This product group already in use by these products:");

                productGroupModel.ProductGroup.Products.Select(x => x.Name).ForEach(x => sb.AppendLine(string.Format("- {0}", x)));

                RaisePropertyChanged("DisableParentWindow");

                RadWindow.Alert(sb.ToString());

                RaisePropertyChanged("EnableParentWindow");

                return;
            }

            // Delete product group

            _adminDataUnit.ProductGroupsRepository.Delete(productGroupModel.ProductGroup);

            await _adminDataUnit.SaveChanges();

            ProductGroups.Remove(productGroupModel);
        }

        private void EditProductGroupCommandExecuted(ProductGroupModel model)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddProductGroupView(model);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }
        #endregion
    }
}
