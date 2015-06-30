using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Enums.Admin;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Members.Category
{
    public class AddMembershipCategoryViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isEditMode;
        private MembershipCategoryModel _membershipCategory;
        private ObservableCollection<MembershipGroupStyle> _membershipGroupStyles;
        private ObservableCollection<MembershipGroupAge> _membershipGroupAges;
        private ObservableCollection<MembershipGroup> _membershipGroups;

        private ObservableCollection<Product> _membershipComponents;


        #endregion Fields

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

        public MembershipCategoryModel MembershipCategory
        {
            get { return _membershipCategory; }
            set
            {
                if (_membershipCategory == value) return;
                _membershipCategory = value;
                RaisePropertyChanged(() => MembershipCategory);
            }
        }
        public ObservableCollection<MembershipGroupStyle> MembershipGroupStyles
        {
            get { return _membershipGroupStyles; }
            set
            {
                if (_membershipGroupStyles == value) return;
                _membershipGroupStyles = value;
                RaisePropertyChanged(() => MembershipGroupStyles);
            }
        }
        public ObservableCollection<MembershipGroupAge> MembershipGroupAges
        {
            get { return _membershipGroupAges; }
            set
            {
                if (_membershipGroupAges == value) return;
                _membershipGroupAges = value;
                RaisePropertyChanged(() => MembershipGroupAges);
            }
        }
        public ObservableCollection<MembershipGroup> MembershipGroups
        {
            get { return _membershipGroups; }
            set
            {
                if (_membershipGroups == value) return;
                _membershipGroups = value;
                RaisePropertyChanged(() => MembershipGroups);
            }
        }

        public List<CategoryType> CategoryTypes
        {
            get
            { return Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().ToList(); }
        }

        public ObservableCollection<Product> MembershipComponents
        {
            get { return _membershipComponents; }
            set
            {
                if (_membershipComponents == value) return;
                _membershipComponents = value;
                RaisePropertyChanged(() => MembershipComponents);
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public AddMembershipCategoryViewModel(MembershipCategoryModel membershipCategory)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);

            ProcessMembershipCategory(membershipCategory);
        }

        #endregion  Constructor

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var styles = await _adminDataUnit.MembershipGroupStylesRepository.GetAllAsync();
            MembershipGroupStyles = new ObservableCollection<MembershipGroupStyle>(styles.OrderBy(x => x.Name));

            var ages = await _adminDataUnit.MembershipGroupAgesRepository.GetAllAsync();
            MembershipGroupAges = new ObservableCollection<MembershipGroupAge>(ages.OrderBy(x => x.Name));

            var groups = await _adminDataUnit.MembershipGroupsRepository.GetAllAsync();
            MembershipGroups = new ObservableCollection<MembershipGroup>(groups.OrderBy(x => x.Name));

            var components = await _adminDataUnit.ProductsRepository.GetAllAsync(product => product.ProductType.Type == "Membership");
            MembershipComponents = new ObservableCollection<Product>(components.OrderBy(product => product.Name));

            IsBusy = false;
        }

        private void ProcessMembershipCategory(MembershipCategoryModel membershipCategory)
        {
            _isEditMode = (membershipCategory != null);

            MembershipCategory = membershipCategory ?? GetNewMembershipCategory();

            if (!_isEditMode)
            {
                MembershipCategory.MembershipFullPaymentComponent = new MembershipFullPaymentCostModel(new MembershipFullPaymentComponent()
                {
                    MembershipCategory = MembershipCategory.MembershipCategory,
                    ID = MembershipCategory.MembershipCategory.ID
                });
                MembershipCategory.MembershipMonthlyPaymentUpFrontCost = new MembershipMonthlyPaymentUpFrontCostModel(new MembershipMonthlyPaymentUpFrontCost()
                {
                    MembershipCategory = MembershipCategory.MembershipCategory,
                    ID = MembershipCategory.MembershipCategory.ID
                });
                MembershipCategory.MembershipMonthlyPaymentOngoingCost = new MembershipMonthlyPaymentOngoingCostModel(new MembershipMonthlyPaymentOngoingCost()
                {
                    MembershipCategory = MembershipCategory.MembershipCategory,
                    ID = MembershipCategory.MembershipCategory.ID
                });
                MembershipCategory.MembershipCategoryGroupDefault = new MembershipCategoryGroupDefaultModel(new MembershipCategoryGroupDefault()
                    {
                        MembershipCategory = MembershipCategory.MembershipCategory,
                        ID = MembershipCategory.MembershipCategory.ID,
                        MembershipCategoryGroupDefaultEPOS = new MembershipCategoryGroupDefaultEPOS()
                        {
                            AllowedClubCard = true
                        }
                    });
            }
            MembershipCategory.PropertyChanged += OnMembershipCategoryPropertyChanged;
        }

        public void MembershipCategoryGroupDefault_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private MembershipCategoryModel GetNewMembershipCategory()
        {
            var membershipCategory = new MembershipCategoryModel(new MembershipCategory()
            {
                ID = Guid.NewGuid(),
                CurrentCategory = true,
            });
            return membershipCategory;
        }

        private void OnMembershipCategoryPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "MembershipGroup")
            {
                MembershipCategory.MembershipCategoryGroupDefault.Name = MembershipCategory.MembershipGroup.Name;
                SetCategoryGroupDefaults(MembershipCategory.MembershipCategoryGroupDefault.MembershipCategoryGroupDefault, MembershipCategory.MembershipGroup);

                MembershipCategory.MembershipCategoryGroupDefault.PropertyChanged += MembershipCategoryGroupDefault_PropertyChanged;
            }

            SaveCommand.RaiseCanExecuteChanged();
        }

        private void SetCategoryGroupDefaults(MembershipCategoryGroupDefault membershipCategoryGroupDefault, MembershipGroup membershipGroup)
        {
            membershipCategoryGroupDefault.Description = membershipGroup.Description;
            membershipCategoryGroupDefault.MemberOnBankHolidays = membershipGroup.MemberOnBankHolidays;
            membershipCategoryGroupDefault.MemberAtChristmasWeek = membershipGroup.MemberAtChristmasWeek;
            membershipCategoryGroupDefault.IsMonAvailable = membershipGroup.IsMonAvailable;
            membershipCategoryGroupDefault.IsTuesAvailable = membershipGroup.IsTuesAvailable;
            membershipCategoryGroupDefault.IsWedAvailable = membershipGroup.IsWedAvailable;
            membershipCategoryGroupDefault.IsThursAvailable = membershipGroup.IsThursAvailable;
            membershipCategoryGroupDefault.IsFriAvailable = membershipGroup.IsFriAvailable;
            membershipCategoryGroupDefault.IsSatAvailable = membershipGroup.IsSatAvailable;
            membershipCategoryGroupDefault.IsSunAvailable = membershipGroup.IsSunAvailable;
            membershipCategoryGroupDefault.Token1 = membershipGroup.Token1;
            membershipCategoryGroupDefault.Token2 = membershipGroup.Token2;
            membershipCategoryGroupDefault.Token3 = membershipGroup.Token3;
            membershipCategoryGroupDefault.Token4 = membershipGroup.Token4;
            membershipCategoryGroupDefault.Token5 = membershipGroup.Token5;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.AllowedClubCard = membershipGroup.MembershipGroupEPOS.AllowedClubCard;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.OverdraftLimit1 = membershipGroup.MembershipGroupEPOS.OverdraftLimit1;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.OverdraftLimit2 = membershipGroup.MembershipGroupEPOS.OverdraftLimit2;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.DiscountRateAll = membershipGroup.MembershipGroupEPOS.DiscountRateAll;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.DiscountRate1 = membershipGroup.MembershipGroupEPOS.DiscountRate1;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.DiscountRate2 = membershipGroup.MembershipGroupEPOS.DiscountRate2;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.DiscountRate3 = membershipGroup.MembershipGroupEPOS.DiscountRate3;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.DiscountRate4 = membershipGroup.MembershipGroupEPOS.DiscountRate4;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.DiscountRate5 = membershipGroup.MembershipGroupEPOS.DiscountRate5;
            membershipCategoryGroupDefault.MembershipCategoryGroupDefaultEPOS.DisplayMessage = membershipGroup.MembershipGroupEPOS.DisplayMessage;
        }

        #endregion Methods

        #region Commands

        private async void SaveCommandExecuted()
        {
            IsBusy = true;

            if (!_isEditMode)
            {
                _adminDataUnit.MembershipCategoryGroupDefaultsRepository.Add(MembershipCategory.MembershipCategoryGroupDefault.MembershipCategoryGroupDefault);
                _adminDataUnit.MembershipFullPaymentCostsRepository.Add(MembershipCategory.MembershipFullPaymentComponent.MembershipFullPaymentComponent);
                _adminDataUnit.MembershipMonthlyPaymentUpFrontCostsRepository.Add(MembershipCategory.MembershipMonthlyPaymentUpFrontCost.MembershipMonthlyPaymentUpFrontCost);
                _adminDataUnit.MembershipMonthlyPaymentOngoingCostsRepository.Add(MembershipCategory.MembershipMonthlyPaymentOngoingCost.MembershipMonthlyPaymentOngoingCost);
                _adminDataUnit.MembershipCategoriesRepository.Add(MembershipCategory.MembershipCategory);
            }

            await _adminDataUnit.SaveChanges();

            IsBusy = false;
        }

        private bool SaveCommandCanExecute()
        {
            return !MembershipCategory.HasErrors && !MembershipCategory.MembershipCategoryGroupDefault.HasErrors;
        }

        #endregion Commands
    }
}
