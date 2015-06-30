using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Admin.EPOS;
using EventManagementSystem.Views.Admin.EPOS.Products;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.EPOS.Products;
using EventManagementSystem.Views.Admin.EPOS.Tills;
using EventManagementSystem.Views.Admin.EPOS.ProductGroups;
using EventManagementSystem.ViewModels.Admin.EPOS.ProductGroups;
using EventManagementSystem.Views.Admin.EPOS.ProductDepartments;
using EventManagementSystem.ViewModels.Admin.EPOS.ProductDepartments;
using EventManagementSystem.Enums.Admin;

namespace EventManagementSystem.ViewModels.Admin.EPOS
{
    public class EPOSViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private readonly IReportsDataUnit _reportsDataUnit;
        private readonly IEventDataUnit _eventsDataUnit;
        private bool _isBusy;
        private ObservableCollection<ProductType> _productTypes;
        private ObservableCollection<ProductModel> _products;
        private ObservableCollection<TillDivisionModel> _tillDivisions;
        private ObservableCollection<TillDivisionModel> _tillDivisionsProgram;
        private object _selectedTreeViewObject;
        private ContentControl _content;

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

        public ObservableCollection<ProductType> ProductTypes
        {
            get { return _productTypes; }
            set
            {
                if (_productTypes == value) return;
                _productTypes = value;
                RaisePropertyChanged(() => ProductTypes);
            }
        }

        public ObservableCollection<ProductModel> Products
        {
            get { return _products; }
            set
            {
                if (_products == value) return;
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        public ObservableCollection<TillDivisionModel> TillDivisions
        {
            get { return _tillDivisions; }
            set
            {
                if (_tillDivisions == value) return;
                _tillDivisions = value;
                RaisePropertyChanged(() => TillDivisions);
            }
        }
        public ObservableCollection<TillDivisionModel> TillDivisionsProgram
        {
            get { return _tillDivisionsProgram; }
            set
            {
                if (_tillDivisionsProgram == value) return;
                _tillDivisionsProgram = value;
                RaisePropertyChanged(() => TillDivisionsProgram);
            }
        }

        public object SelectedTreeViewObject
        {
            get { return _selectedTreeViewObject; }
            set
            {
                if (_selectedTreeViewObject == value) return;
                _selectedTreeViewObject = value;
                RaisePropertyChanged(() => SelectedTreeViewObject);

                if (_selectedTreeViewObject is ProductType)
                    Content = new ProductsView(_selectedTreeViewObject as ProductType);
                else if (_selectedTreeViewObject is TillDivisionModel)
                    Content = null;
                else if (_selectedTreeViewObject is TillModel)
                {
                    var till = value as TillModel;
                    if (till != null)
                    {
                        var tillModel = new TillModel(till.Till);
                        Content = new TillView(tillModel, TillDivisions);
                    }
                }
                else if (_selectedTreeViewObject.ToString() == "Tills")
                {
                    Content = null;
                }
                else if (_selectedTreeViewObject.ToString() == "Products")
                {
                    Content = new ProductsView(null);
                }
                else if (_selectedTreeViewObject is TillDivisionGroupDepartmentModel)
                {
                    var tillDivisionGroupDepartment = value as TillDivisionGroupDepartmentModel;
                    if (tillDivisionGroupDepartment != null && tillDivisionGroupDepartment.Type == GroupDepartmentEnum.Groups)
                        Content = new ProductGroupView(tillDivisionGroupDepartment);
                    else if (tillDivisionGroupDepartment != null && tillDivisionGroupDepartment.Type == GroupDepartmentEnum.Departments)
                        Content = new ProductDepartmentView(tillDivisionGroupDepartment);
                }

                DeleteEPOSCommand.RaiseCanExecuteChanged();
            }
        }

        public ContentControl Content
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;
                RaisePropertyChanged(() => Content);
            }
        }


        public RelayCommand AddProductCommand { get; private set; }

        public RelayCommand AddTillCommand { get; private set; }

        public RelayCommand AddProductGroupCommand { get; private set; }
        public RelayCommand AddProductDepartmentCommand { get; private set; }

        public RelayCommand DeleteEPOSCommand { get; set; }


        #endregion

        #region Constructors

        public EPOSViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            AddProductCommand = new RelayCommand(AddProductCommandExecuted);
            AddTillCommand = new RelayCommand(AddTillCommandExecuted);
            AddProductGroupCommand = new RelayCommand(AddProductGroupCommandExecuted);
            AddProductDepartmentCommand = new RelayCommand(AddProductDepartmentCommandExecuted);
            DeleteEPOSCommand = new RelayCommand(DeleteEPOSCommandExecuted, DeleteEPOSCommandCanExecute);

        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var types = await _adminDataUnit.ProductTypesRepository.GetAllAsync();
            ProductTypes = new ObservableCollection<ProductType>(types.OrderBy(x => x.Type));

            var tillDivisions = await _adminDataUnit.TillDivisionsRepository.GetAllAsync();
            TillDivisions = new ObservableCollection<TillDivisionModel>(tillDivisions.Select(x => new TillDivisionModel(x)));
            TillDivisionsProgram = new ObservableCollection<TillDivisionModel>(tillDivisions.Select(x => new TillDivisionModel(x)));

            RaisePropertyChanged("SetProductsAsDefault");

            IsBusy = false;
        }
        public async void RefreshTills(TillModel _till)
        {
            var tillDivisions = await _adminDataUnit.TillDivisionsRepository.GetAllAsync();
            TillDivisions = new ObservableCollection<TillDivisionModel>(tillDivisions.Select(x => new TillDivisionModel(x)));
            if (_till != null)
            {
                var selectedTillDivision = TillDivisions.FirstOrDefault(tillDiv => tillDiv.TillDivision.ID == _till.TillDivision.ID);
                if (selectedTillDivision != null)
                {
                    var selectedTill = selectedTillDivision.Tills.FirstOrDefault(till => till.Till.ID == _till.Till.ID);
                    if (selectedTill != null)
                    {
                        SelectedTreeViewObject = selectedTill;
                        TillDivisions.ForEach(tilldivision =>
                            {
                                tilldivision.IsExpanded = false;
                            });
                        selectedTillDivision.IsExpanded = true;
                    }
                }
            }
        }

        #endregion

        #region Commands

        private void AddProductCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addProductView = new AddProductView();
            addProductView.ShowDialog();
            if (addProductView.DialogResult != null && addProductView.DialogResult == true)
            {
                if (Content != null)
                {
                    var viewModel = Content.DataContext as ProductsViewModel;
                    if (viewModel != null)
                    {
                        viewModel.AllProducts.Add(addProductView.ViewModel.Product);
                        viewModel.RefreshProducts();
                        viewModel.SelectedProduct = addProductView.ViewModel.Product;
                    }
                }
            }
            RaisePropertyChanged("EnableParentWindow");
        }
        private void AddTillCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addTillView = new AddTillView(TillDivisions);
            addTillView.ShowDialog();
            if (addTillView.DialogResult != null && addTillView.DialogResult == true)
            {
                var tillDivision = TillDivisions.Where(p => p.TillDivision.ID == addTillView.ViewModel.Till.TillDivision.ID).FirstOrDefault();
                if (tillDivision != null)
                    tillDivision.Tills.Add(addTillView.ViewModel.Till);
                RefreshTills(addTillView.ViewModel.Till);
            }
            RaisePropertyChanged("EnableParentWindow");
        }
        private void AddProductGroupCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addProductGroupView = new AddProductGroupView(null);
            addProductGroupView.ShowDialog();
            if (addProductGroupView.DialogResult != null && addProductGroupView.DialogResult == true)
            {
                if (Content != null)
                {
                    var viewModel = Content.DataContext as ProductGroupViewModel;
                    if (viewModel != null)
                    {
                        viewModel.AllProductGroups.Add(addProductGroupView.ViewModel.ProductGroup);
                        viewModel.RefreshProductGroups();
                    }
                }
            }
            RaisePropertyChanged("EnableParentWindow");
        }
        private void AddProductDepartmentCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addProductGroupView = new AddProductDepartmentView(null);
            addProductGroupView.ShowDialog();
            if (addProductGroupView.DialogResult != null && addProductGroupView.DialogResult == true)
            {
                if (Content != null)
                {
                    var viewModel = Content.DataContext as ProductDepartmentViewModel;
                    if (viewModel != null)
                    {
                        viewModel.AllProductDepartments.Add(addProductGroupView.ViewModel.ProductDepartment);
                        viewModel.RefreshProductDepartments();
                    }
                }
            }
            RaisePropertyChanged("EnableParentWindow");
        }
        private void DeleteEPOSCommandExecuted()
        {
            bool? dialogResult = null;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM,
                (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            if (SelectedTreeViewObject is TillModel)
            {
                var till = SelectedTreeViewObject as TillModel;
                var tillDiv = TillDivisions.Where(tillDivision => tillDivision.TillDivision.ID == till.Till.TillDivision.ID).FirstOrDefault();
                if (tillDiv.TillDivision.MasterTillID == till.Till.ID)
                    tillDiv.TillDivision.MasterTillID = null;
                tillDiv.Tills.Remove(till);

                _adminDataUnit.TillsRepository.Delete(till.Till);
                _adminDataUnit.SaveChanges();
            }
            Content = null;
        }
        private bool DeleteEPOSCommandCanExecute()
        {
            return (SelectedTreeViewObject is TillModel);
        }
        #endregion

    }

}
