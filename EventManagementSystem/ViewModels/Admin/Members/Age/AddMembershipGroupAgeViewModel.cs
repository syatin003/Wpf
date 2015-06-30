using System;
using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Members.Age
{
    public class AddMembershipGroupAgeViewModel : ViewModelBase
    {

        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isEditMode;
        private MembershipGroupAgeModel _membershipGroupAge;
        private bool _isExpanded = true;

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

        public MembershipGroupAgeModel MembershipGroupAge
        {
            get { return _membershipGroupAge; }
            set
            {
                if (_membershipGroupAge == value) return;
                _membershipGroupAge = value;
                RaisePropertyChanged(() => MembershipGroupAge);
            }
        }
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                RaisePropertyChanged(() => IsExpanded);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand ExpandCollapseButtonCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public AddMembershipGroupAgeViewModel(MembershipGroupAgeModel membershipGroupAge)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            ExpandCollapseButtonCommand = new RelayCommand(ExpandCollapseButtonCommandExecuted);

            ProcessMembershipGroupAge(membershipGroupAge);
        }

        #endregion  Constructor

        #region Methods

        private void ProcessMembershipGroupAge(MembershipGroupAgeModel membershipGroupAge)
        {
            _isEditMode = (membershipGroupAge != null);

            MembershipGroupAge = membershipGroupAge ?? GetNewMembershipGroupAge();

            MembershipGroupAge.PropertyChanged += OnMembershipGroupAgePropertyChanged;
        }

        private MembershipGroupAgeModel GetNewMembershipGroupAge()
        {
            var membershipGroupAge = new MembershipGroupAgeModel(new MembershipGroupAge()
            {
                ID = Guid.NewGuid()
            });
            return membershipGroupAge;
        }

        private void OnMembershipGroupAgePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion Methods

        #region Commands

        private void SaveCommandExecuted()
        {
            IsBusy = true;

            if (!_isEditMode)
                _adminDataUnit.MembershipGroupAgesRepository.Add(MembershipGroupAge.MembershipGroupAge);

            _adminDataUnit.SaveChanges();

            IsBusy = false;
        }

        private bool SaveCommandCanExecute()
        {
            return !MembershipGroupAge.HasErrors;
        }

        private void ExpandCollapseButtonCommandExecuted()
        {
            IsExpanded = !IsExpanded;
        }

        #endregion Commands
    }
}
