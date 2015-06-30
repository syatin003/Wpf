using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Admin.Members.LinkTypes;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Members.LinkTypes
{
    public class MembershipLinkTypesViewModel : ViewModelBase
    {

        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;

        private ObservableCollection<MembershipLinkTypeModel> _membershipLinkTypes;
        public MembershipLinkTypeModel _selectedMembershipLinkType;

        #endregion

        #region Properties

        public List<MembershipLinkTypeModel> AllMembershipLinkTypes { get; set; }

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

        public ObservableCollection<MembershipLinkTypeModel> MembershipLinkTypes
        {
            get { return _membershipLinkTypes; }
            set
            {
                if (_membershipLinkTypes == value) return;
                _membershipLinkTypes = value;
                RaisePropertyChanged(() => MembershipLinkTypes);
            }
        }

        public MembershipLinkTypeModel SelectedMembershipLinkType
        {
            get { return _selectedMembershipLinkType; }
            set
            {
                if (_selectedMembershipLinkType == value) return;
                _selectedMembershipLinkType = value;
                RaisePropertyChanged(() => SelectedMembershipLinkType);
            }
        }

        public RelayCommand<MembershipLinkTypeModel> DeleteMembershipLinkTypeCommand { get; private set; }
        public RelayCommand<MembershipLinkTypeModel> EditMembershipLinkTypeCommand { get; private set; }

        #endregion

        #region Constructors

        public MembershipLinkTypesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            DeleteMembershipLinkTypeCommand = new RelayCommand<MembershipLinkTypeModel>(DeleteMembershipLinkTypeCommandExecuted);
            EditMembershipLinkTypeCommand = new RelayCommand<MembershipLinkTypeModel>(EditMembershipLinkTypeCommandExecuted);

        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            await OnLoadMembershipLinkTypes();

            IsBusy = false;
        }

        private async Task OnLoadMembershipLinkTypes()
        {
            IsBusy = true;

            _adminDataUnit.MembershipLinkTypesRepository.Refresh();
            var membershipLinkTypes = await _adminDataUnit.MembershipLinkTypesRepository.GetAllAsync();
            AllMembershipLinkTypes = new List<MembershipLinkTypeModel>(membershipLinkTypes.Select(x => new MembershipLinkTypeModel(x)));

            RefreshMembershipLinkTypes();

            IsBusy = false;
        }

        public void RefreshMembershipLinkTypes()
        {
            MembershipLinkTypes = new ObservableCollection<MembershipLinkTypeModel>(AllMembershipLinkTypes);
        }

        #endregion

        #region Commands

        private async void DeleteMembershipLinkTypeCommandExecuted(MembershipLinkTypeModel membershipLinkType)
        {
            if (membershipLinkType == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            // Delete MembershipLinkType
            _adminDataUnit.MembershipLinkTypesRepository.Delete(membershipLinkType.MembershipLinkType);

            await _adminDataUnit.SaveChanges();

            MembershipLinkTypes.Remove(membershipLinkType);
        }

        private void EditMembershipLinkTypeCommandExecuted(MembershipLinkTypeModel membershipLinkType)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddMembershipLinkTypeView(membershipLinkType);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true)
            {
                _adminDataUnit.MembershipLinkTypesRepository.Refresh();
                RefreshMembershipLinkTypes();
                SelectedMembershipLinkType = membershipLinkType;
            }
        }
        #endregion
    }
}
