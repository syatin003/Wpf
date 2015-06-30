using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Members.LinkTypes
{
    public class AddMembershipLinkTypeViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private MembershipLinkTypeModel _membershipLinkType;
        private bool _isBusy;
        private bool _isEditMode;
        private ObservableCollection<MembershipCategory> _membershipCategories;
        private ObservableCollection<MembershipCategory> _nonMembershipCategories;

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
        public MembershipLinkTypeModel MembershipLinkType
        {
            get { return _membershipLinkType; }
            set
            {
                if (_membershipLinkType == value) return;
                _membershipLinkType = value;
                RaisePropertyChanged(() => MembershipLinkType);
            }
        }
        public ObservableCollection<MembershipCategory> MembershipCategories
        {
            get { return _membershipCategories; }
            set
            {
                if (_membershipCategories == value) return;
                _membershipCategories = value;
                RaisePropertyChanged(() => MembershipCategories);
            }
        }
        public ObservableCollection<MembershipCategory> NonMembershipCategories
        {
            get { return _nonMembershipCategories; }
            set
            {
                if (_nonMembershipCategories == value) return;
                _nonMembershipCategories = value;
                RaisePropertyChanged(() => NonMembershipCategories);
            }
        }
        public RelayCommand SaveCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public AddMembershipLinkTypeViewModel(MembershipLinkTypeModel membershipLinkType)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);

            ProcessMembershipLinkType(membershipLinkType);
        }

        #endregion  Constructor

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.MembershipCategoriesRepository.Refresh();

            var categories = await _adminDataUnit.MembershipCategoriesRepository.GetAllCategoriesWithItsTabsDataAsync();
            MembershipCategories = new ObservableCollection<MembershipCategory>(categories.Where(category => category.MembershipGroupStyle.ClassifiedAsMember).OrderBy(category => category.Name));

            NonMembershipCategories = new ObservableCollection<MembershipCategory>(categories.Where(category => !category.MembershipGroupStyle.ClassifiedAsMember).OrderBy(category => category.Name));

            IsBusy = false;
        }

        private void ProcessMembershipLinkType(MembershipLinkTypeModel membershipLinkType)
        {
            _isEditMode = (membershipLinkType != null);

            MembershipLinkType = membershipLinkType ?? GetNewMembershipLinkType();

            MembershipLinkType.PropertyChanged += OnMembershipLinkTypePropertyChanged;
        }

        private MembershipLinkTypeModel GetNewMembershipLinkType()
        {
            var membershipLinkType = new MembershipLinkTypeModel(new MembershipLinkType()
            {
                ID = Guid.NewGuid()
            });
            return membershipLinkType;
        }

        private void OnMembershipLinkTypePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion Methods

        #region Commands

        private void SaveCommandExecuted()
        {
            IsBusy = true;

            if (!_isEditMode)
                _adminDataUnit.MembershipLinkTypesRepository.Add(MembershipLinkType.MembershipLinkType);

            if (!MembershipLinkType.AutoRenew)
                MembershipLinkType.AutoRenewCategory = null;
            if (!MembershipLinkType.AutoResign)
                MembershipLinkType.AutoResignCategory = null;

            _adminDataUnit.SaveChanges();

            IsBusy = false;
        }

        private bool SaveCommandCanExecute()
        {
            return !MembershipLinkType.HasErrors;
        }

        #endregion Commands
    }
}
