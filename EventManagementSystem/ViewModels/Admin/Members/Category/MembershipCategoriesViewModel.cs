using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Admin.Members.Category;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Members.Category
{
    public class MembershipCategoriesViewModel : ViewModelBase
    {

        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private readonly IMembershipDataUnit _membershipDataUnit;
        private bool _isBusy;

        private ObservableCollection<MembershipCategoryModel> _membershipCategories;
        public MembershipCategoryModel _selectedMembershipCategory;

        #endregion

        #region Properties

        public List<MembershipCategoryModel> AllMembershipCategories { get; set; }

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

        public ObservableCollection<MembershipCategoryModel> MembershipCategories
        {
            get { return _membershipCategories; }
            set
            {
                if (_membershipCategories == value) return;
                _membershipCategories = value;
                RaisePropertyChanged(() => MembershipCategories);
            }
        }

        public MembershipCategoryModel SelectedMembershipCategory
        {
            get { return _selectedMembershipCategory; }
            set
            {
                if (_selectedMembershipCategory == value) return;
                _selectedMembershipCategory = value;
                RaisePropertyChanged(() => SelectedMembershipCategory);
            }
        }

        public RelayCommand<MembershipCategoryModel> DeleteMembershipCategoryCommand { get; private set; }
        public RelayCommand<MembershipCategoryModel> EditMembershipCategoryCommand { get; private set; }

        #endregion

        #region Constructors

        public MembershipCategoriesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();

            DeleteMembershipCategoryCommand = new RelayCommand<MembershipCategoryModel>(DeleteMembershipCategoryCommandExecuted);
            EditMembershipCategoryCommand = new RelayCommand<MembershipCategoryModel>(EditMembershipCategoryCommandExecuted);

        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            await OnLoadMembershipCategories();

            IsBusy = false;
        }

        private async Task OnLoadMembershipCategories()
        {
            IsBusy = true;

            _adminDataUnit.MembershipCategoriesRepository.RefreshCategoryGroupsWithItsTabs();
            var membershipCategories = await _adminDataUnit.MembershipCategoriesRepository.GetAllCategoriesWithItsTabsDataAsync();
            AllMembershipCategories = new List<MembershipCategoryModel>(membershipCategories.Select(x => new MembershipCategoryModel(x)));

            RefreshMembershipCategories();

            IsBusy = false;
        }

        public void RefreshMembershipCategories()
        {
            MembershipCategories = new ObservableCollection<MembershipCategoryModel>(AllMembershipCategories);
        }

        #endregion

        #region Commands

        private async void DeleteMembershipCategoryCommandExecuted(MembershipCategoryModel membershipCategory)
        {
            if (membershipCategory == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            // Delete MembershipCategoryGroupDefaultEPOS
            _adminDataUnit.MembershipCategoryGroupDefaultEPOSRepository.Delete(membershipCategory.MembershipCategoryGroupDefault.MembershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS);

            // Delete MembershipCategoryGroupDefaults
            //var categoryGroupDefaults = await _adminDataUnit.MembershipCategoryGroupDefaultsRepository.GetAllAsync(categoryGroupDefault => categoryGroupDefault.ID == membershipCategory.MembershipCategory.ID);
            _adminDataUnit.MembershipCategoryGroupDefaultsRepository.Delete(membershipCategory.MembershipCategoryGroupDefault.MembershipCategoryGroupDefault);

            // Delete MembershipFullPaymentCosts
            //var categoryFullPaymentCosts = await _adminDataUnit.MembershipFullPaymentCostsRepository.GetAllAsync(fullPaymentCost => fullPaymentCost.ID == membershipCategory.MembershipCategory.ID);
            _adminDataUnit.MembershipFullPaymentCostsRepository.Delete(membershipCategory.MembershipFullPaymentComponent.MembershipFullPaymentComponent);

            // Delete MembershipMontlyPaymentUpFrontCosts
            //var categoryMonthlytPaymentUpFrontCosts = await _adminDataUnit.MembershipMonthlyPaymentUpFrontCostsRepository.GetAllAsync(fullPaymentCost => fullPaymentCost.ID == membershipCategory.MembershipCategory.ID);
            _adminDataUnit.MembershipMonthlyPaymentUpFrontCostsRepository.Delete(membershipCategory.MembershipMonthlyPaymentUpFrontCost.MembershipMonthlyPaymentUpFrontCost);

            // Delete MembershipMontlyPaymentOnGoingCosts
            //var categoryMonthlytPaymentOngoingCosts = await _adminDataUnit.MembershipMonthlyPaymentOngoingCostsRepository.GetAllAsync(fullPaymentCost => fullPaymentCost.ID == membershipCategory.MembershipCategory.ID);
            _adminDataUnit.MembershipMonthlyPaymentOngoingCostsRepository.Delete(membershipCategory.MembershipMonthlyPaymentOngoingCost.MembershipMonthlyPaymentOngoingCost);

            // Check if Category has dependencies
            if (membershipCategory.MembershipCategory.MembershipLinkTypes.Any() || membershipCategory.MembershipCategory.MembershipLinkTypes1.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine("Sorry, we can't delete this category :(");
                sb.AppendLine("This category is being used in link types");

                RaisePropertyChanged("DisableParentWindow");

                RadWindow.Alert(sb.ToString());

                RaisePropertyChanged("EnableParentWindow");

                return;
            }

            // Delete MembershipCategory
            _adminDataUnit.MembershipCategoriesRepository.Delete(membershipCategory.MembershipCategory);

            await _adminDataUnit.SaveChanges();

            MembershipCategories.Remove(membershipCategory);
        }

        private void EditMembershipCategoryCommandExecuted(MembershipCategoryModel membershipCategory)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddMembershipCategoryView(membershipCategory);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true)
            {
                _membershipDataUnit.MembershipCategoriesRepository.Refresh();
                _adminDataUnit.MembershipCategoriesRepository.RefreshCategoryGroupsWithItsTabs();
                RefreshMembershipCategories();
                SelectedMembershipCategory = membershipCategory;
            }
        }

        #endregion
    }
}
