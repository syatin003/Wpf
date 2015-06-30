using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Members.OptionBoxes
{
    public class MembershipOptionBoxReasonsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private List<MembershipOptionBoxReason> _allMembershipOptionBoxReasons;
        private ObservableCollection<MembershipOptionBoxReason> _membershipOptionBoxReasons;
        private string _newMembershipOptionBoxReason;
        private MembershipOptionBoxReason _selectedMembershipOptionBoxReason;
        private MembershipOptionBox _membershipOptionBox;
        private bool _isEditMode;

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

        public ObservableCollection<MembershipOptionBoxReason> MembershipOptionBoxReasons
        {
            get { return _membershipOptionBoxReasons; }
            set
            {
                if (_membershipOptionBoxReasons == value) return;
                _membershipOptionBoxReasons = value;
                RaisePropertyChanged(() => MembershipOptionBoxReasons);
            }
        }

        public string NewMembershipOptionBoxReason
        {
            get { return _newMembershipOptionBoxReason; }
            set
            {
                if (_newMembershipOptionBoxReason == value) return;
                _newMembershipOptionBoxReason = value;
                if (_newMembershipOptionBoxReason == string.Empty)
                    if (IsEditMode)
                        IsEditMode = !IsEditMode;
                RaisePropertyChanged(() => NewMembershipOptionBoxReason);
                AddNewMembershipOptionBoxReasonCommand.RaiseCanExecuteChanged();
            }
        }

        public MembershipOptionBoxReason SelectedOptionBoxReason
        {
            get { return _selectedMembershipOptionBoxReason; }
            set
            {
                if (_selectedMembershipOptionBoxReason == value) return;
                _selectedMembershipOptionBoxReason = value;
                RaisePropertyChanged(() => SelectedOptionBoxReason);
            }
        }

        public MembershipOptionBox MembershipOptionBox
        {
            get { return _membershipOptionBox; }
            set
            {
                if (_membershipOptionBox == value) return;
                _membershipOptionBox = value;
                RaisePropertyChanged(() => MembershipOptionBox);
            }
        }
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (_isEditMode == value) return;
                _isEditMode = value;
                RaisePropertyChanged(() => IsEditMode);
            }
        }

        public RelayCommand AddNewMembershipOptionBoxReasonCommand { get; private set; }

        public RelayCommand<MembershipOptionBoxReason> DeleteMembershipOptionBoxReasonCommand { get; private set; }
        public RelayCommand<MembershipOptionBoxReason> EditMembershipOptionBoxReasonCommand { get; private set; }

        #endregion

        #region Constructor

        public MembershipOptionBoxReasonsViewModel(MembershipOptionBox membershipOptionBox)
        {
            MembershipOptionBox = membershipOptionBox;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            AddNewMembershipOptionBoxReasonCommand = new RelayCommand(AddNewMembershipOptionBoxReasonCommandExecuted, AddNewMembershipOptionBoxReasonCommandCanExecute);
            DeleteMembershipOptionBoxReasonCommand = new RelayCommand<MembershipOptionBoxReason>(DeleteMembershipOptionBoxReasonCommandExecuted);
            EditMembershipOptionBoxReasonCommand = new RelayCommand<MembershipOptionBoxReason>(EditMembershipOptionBoxReasonCommandExecuted);
        }

        #endregion

        #region Methods

        private MembershipOptionBoxReason GetNewMembershipOptionBoxReason()
        {
            return new MembershipOptionBoxReason()
            {
                ID = Guid.NewGuid(),
                Reason = NewMembershipOptionBoxReason,
                MembershipOptionBox = MembershipOptionBox,
                OptionBoxID = MembershipOptionBox.ID
            };
        }

        public async void LoadData()
        {
            IsBusy = true;

           _adminDataUnit.MembershipOptionBoxReasonsRepository.Refresh();
            var optionBoxReasons = await _adminDataUnit.MembershipOptionBoxReasonsRepository.GetAllAsync(optionBoxReason => optionBoxReason.OptionBoxID == MembershipOptionBox.ID);

            _allMembershipOptionBoxReasons = new List<MembershipOptionBoxReason>(optionBoxReasons);
            RefreshMembershipOptionBoxReasons();

            IsBusy = false;
        }

        private void RefreshMembershipOptionBoxReasons()
        {
            MembershipOptionBoxReasons = new ObservableCollection<MembershipOptionBoxReason>(_allMembershipOptionBoxReasons.OrderBy(x => x.Reason));
        }

        #endregion

        #region Commands

        private bool AddNewMembershipOptionBoxReasonCommandCanExecute()
        {
            return !String.IsNullOrEmpty(NewMembershipOptionBoxReason);
        }

        private void AddNewMembershipOptionBoxReasonCommandExecuted()
        {
            IsBusy = true;

            if (!IsEditMode)
            {
                var newMembershipOptionBoxReason = GetNewMembershipOptionBoxReason();
                _adminDataUnit.MembershipOptionBoxReasonsRepository.Add(newMembershipOptionBoxReason);
                _allMembershipOptionBoxReasons.Add(newMembershipOptionBoxReason);
            }
            else
                SelectedOptionBoxReason.Reason = NewMembershipOptionBoxReason;
            _adminDataUnit.SaveChanges();
            RefreshMembershipOptionBoxReasons();
            NewMembershipOptionBoxReason = String.Empty;

            IsBusy = false;
        }

        private void EditMembershipOptionBoxReasonCommandExecuted(MembershipOptionBoxReason membershipOptionBoxReason)
        {
            if (membershipOptionBoxReason == null) return;
            SelectedOptionBoxReason = membershipOptionBoxReason;
            IsEditMode = true;
            NewMembershipOptionBoxReason = membershipOptionBoxReason.Reason;
            RaisePropertyChanged("SetFocusOnOptionBoxReasonText");
        }

        private void DeleteMembershipOptionBoxReasonCommandExecuted(MembershipOptionBoxReason membershipOptionBoxReason)
        {
            if (membershipOptionBoxReason == null) return;

            bool? dialogResult = null;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(new DialogParameters
            {
                Content = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM,
                Header = "Warning!",
                OkButtonContent = "Yes",
                CancelButtonContent = "No",
                Owner = Application.Current.MainWindow,
                Closed = (sender, args) => { dialogResult = args.DialogResult; }
            });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            _adminDataUnit.MembershipOptionBoxReasonsRepository.Delete(membershipOptionBoxReason);
            _adminDataUnit.SaveChanges();
            MembershipOptionBoxReasons.Remove(membershipOptionBoxReason);
            NewMembershipOptionBoxReason = String.Empty;
        }

        #endregion
    }
}
