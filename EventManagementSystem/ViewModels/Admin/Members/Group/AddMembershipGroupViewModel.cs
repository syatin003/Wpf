using System;
using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Members.Group
{
    public class AddMembershipGroupViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isEditMode;
        private MembershipGroupModel _membershipGroup;

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

        public MembershipGroupModel MembershipGroup
        {
            get { return _membershipGroup; }
            set
            {
                if (_membershipGroup == value) return;
                _membershipGroup = value;
                RaisePropertyChanged(() => MembershipGroup);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand ExpandCollapseButtonCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public AddMembershipGroupViewModel(MembershipGroupModel membershipGroup)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);

            ProcessMembershipGroup(membershipGroup);
        }

        #endregion  Constructor

        #region Methods

        private void ProcessMembershipGroup(MembershipGroupModel membershipGroup)
        {
            _isEditMode = (membershipGroup != null);

            MembershipGroup = membershipGroup ?? GetNewMembershipGroup();
            MembershipGroup.PropertyChanged += OnMembershipGroupPropertyChanged;
        }

        private MembershipGroupModel GetNewMembershipGroup()
        {
            var membershipGroup = new MembershipGroupModel(new MembershipGroup()
            {
                ID = Guid.NewGuid(),
                MembershipGroupEPOS = new MembershipGroupEPOS()
                {
                    AllowedClubCard = true
                }
            });
            return membershipGroup;
        }

        private void OnMembershipGroupPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion Methods

        #region Commands

        private void SaveCommandExecuted()
        {
            IsBusy = true;

            if (!_isEditMode)
            {
                _adminDataUnit.MembershipGroupEPOSRepository.Add(MembershipGroup.MembershipGroup.MembershipGroupEPOS);
                _adminDataUnit.MembershipGroupsRepository.Add(MembershipGroup.MembershipGroup);
            }

            _adminDataUnit.SaveChanges();

            IsBusy = false;
        }

        private bool SaveCommandCanExecute()
       {
        //    if (IsBusy)
        //        return false;
            return !MembershipGroup.HasErrors;
        }
        #endregion Commands
    }
}
