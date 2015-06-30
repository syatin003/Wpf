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

namespace EventManagementSystem.ViewModels.Admin.Members.Tokens
{
    public class MembershipTokensViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private List<MembershipToken> _allMembershipTokens;
        private ObservableCollection<MembershipToken> _membershipTokens;
        private string _newMembershipToken;
        private MembershipToken _selectedMembershipToken;
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

        public ObservableCollection<MembershipToken> MembershipTokens
        {
            get { return _membershipTokens; }
            set
            {
                if (_membershipTokens == value) return;
                _membershipTokens = value;
                RaisePropertyChanged(() => MembershipTokens);
            }
        }

        public string NewMembershipToken
        {
            get { return _newMembershipToken; }
            set
            {
                if (_newMembershipToken == value) return;
                _newMembershipToken = value;
                if (_newMembershipToken == string.Empty)
                    if (IsEditMode)
                        IsEditMode = !IsEditMode;
                RaisePropertyChanged(() => NewMembershipToken);
                AddNewMembershipTokenCommand.RaiseCanExecuteChanged();
            }
        }

        public MembershipToken SelectedMembershipToken
        {
            get { return _selectedMembershipToken; }
            set
            {
                if (_selectedMembershipToken == value) return;
                _selectedMembershipToken = value;
                RaisePropertyChanged(() => SelectedMembershipToken);
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

        public RelayCommand AddNewMembershipTokenCommand { get; private set; }

        public RelayCommand<MembershipToken> DeleteMembershipTokenCommand { get; private set; }
        public RelayCommand<MembershipToken> EditMembershipTokenCommand { get; private set; }

        #endregion

        #region Constructor

        public MembershipTokensViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            AddNewMembershipTokenCommand = new RelayCommand(AddNewMembershipTokenCommandExecuted, AddNewMembershipTokenCommandCanExecute);
            DeleteMembershipTokenCommand = new RelayCommand<MembershipToken>(DeleteMembershipTokenCommandExecuted);
            EditMembershipTokenCommand = new RelayCommand<MembershipToken>(EditMembershipTokenCommandExecuted);
        }

        #endregion

        #region Methods

        private MembershipToken GetNewMembershipToken()
        {
            return new MembershipToken()
            {
                ID = Guid.NewGuid(),
                Description = NewMembershipToken
            };
        }

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.MembershipTokensRepository.Refresh();
            var tokens = await _adminDataUnit.MembershipTokensRepository.GetAllAsync();

            _allMembershipTokens = new List<MembershipToken>(tokens);
            RefreshMembershipTokens();

            IsBusy = false;
        }

        private void RefreshMembershipTokens()
        {
            MembershipTokens = new ObservableCollection<MembershipToken>(_allMembershipTokens.OrderBy(x => x.Description));
        }

        #endregion

        #region Commands

        private bool AddNewMembershipTokenCommandCanExecute()
        {
            return !String.IsNullOrEmpty(NewMembershipToken);
        }

        private void AddNewMembershipTokenCommandExecuted()
        {
            IsBusy = true;

            if (!IsEditMode)
            {
                var newMembershipToken = GetNewMembershipToken();
                _adminDataUnit.MembershipTokensRepository.Add(newMembershipToken);
                _allMembershipTokens.Add(newMembershipToken);
            }
            else
                SelectedMembershipToken.Description = NewMembershipToken;
            _adminDataUnit.SaveChanges();
            RefreshMembershipTokens();
            NewMembershipToken = String.Empty;

            IsBusy = false;
        }

        private void EditMembershipTokenCommandExecuted(MembershipToken membershipToken)
        {
            if (membershipToken == null) return;
            SelectedMembershipToken = membershipToken;
            IsEditMode = true;
            NewMembershipToken = membershipToken.Description;
            RaisePropertyChanged("SetFocusOnTokenText");
        }

        private void DeleteMembershipTokenCommandExecuted(MembershipToken membershipToken)
        {
            if (membershipToken == null) return;

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

            _adminDataUnit.MembershipTokensRepository.Delete(membershipToken);
            _adminDataUnit.SaveChanges();
            MembershipTokens.Remove(membershipToken);
            NewMembershipToken = string.Empty;
        }

        #endregion
    }
}
