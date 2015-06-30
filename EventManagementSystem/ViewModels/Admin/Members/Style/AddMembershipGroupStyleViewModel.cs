using System;
using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Members.Style
{
    public class AddMembershipGroupStyleViewModel : ViewModelBase
    {

        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isEditMode;
        private MembershipGroupStyleModel _membershipGroupStyle;
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

        public MembershipGroupStyleModel MembershipGroupStyle
        {
            get { return _membershipGroupStyle; }
            set
            {
                if (_membershipGroupStyle == value) return;
                _membershipGroupStyle = value;
                RaisePropertyChanged(() => MembershipGroupStyle);
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

        public AddMembershipGroupStyleViewModel(MembershipGroupStyleModel membershipGroupStyle)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            ExpandCollapseButtonCommand = new RelayCommand(ExpandCollapseButtonCommandExecuted);

            ProcessMembershipGroupStyle(membershipGroupStyle);
        }

        #endregion  Constructor

        #region Methods

        private void ProcessMembershipGroupStyle(MembershipGroupStyleModel membershipGroupStyle)
        {
            _isEditMode = (membershipGroupStyle != null);

            MembershipGroupStyle = membershipGroupStyle ?? GetNewMembershipGroupStyle();
            MembershipGroupStyle.PropertyChanged += OnMembershipGroupStylePropertyChanged;
        }

        private MembershipGroupStyleModel GetNewMembershipGroupStyle()
        {
            var membershipGroupStyle = new MembershipGroupStyleModel(new MembershipGroupStyle()
            {
                ID = Guid.NewGuid()
            });
            return membershipGroupStyle;
        }

        private void OnMembershipGroupStylePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion Methods

        #region Commands

        private void SaveCommandExecuted()
        {
            IsBusy = true;

            if (!_isEditMode)
                _adminDataUnit.MembershipGroupStylesRepository.Add(MembershipGroupStyle.MembershipGroupStyle);

            _adminDataUnit.SaveChanges();

            IsBusy = false;
        }

        private bool SaveCommandCanExecute()
        {
            return !MembershipGroupStyle.HasErrors;
        }


        private void ExpandCollapseButtonCommandExecuted()
        {
            IsExpanded = !IsExpanded;
        }

        #endregion Commands
    }
}
