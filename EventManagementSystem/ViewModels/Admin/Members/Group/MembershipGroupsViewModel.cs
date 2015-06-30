using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Admin.Members.Group;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Members.Group
{
    public class MembershipGroupsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;

        private ObservableCollection<MembershipGroupModel> _membershipGroups;

        #endregion

        #region Properties

        public List<MembershipGroupModel> AllMembershipGroups { get; set; }

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

        public ObservableCollection<MembershipGroupModel> MembershipGroups
        {
            get { return _membershipGroups; }
            set
            {
                if (_membershipGroups == value) return;
                _membershipGroups = value;
                RaisePropertyChanged(() => MembershipGroups);
            }
        }
        public RelayCommand<MembershipGroupModel> DeleteMembershipGroupCommand { get; private set; }
        public RelayCommand<MembershipGroupModel> EditMembershipGroupCommand { get; private set; }

        #endregion

        #region Constructors

        public MembershipGroupsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            DeleteMembershipGroupCommand = new RelayCommand<MembershipGroupModel>(DeleteMembershipGroupCommandExecuted);
            EditMembershipGroupCommand = new RelayCommand<MembershipGroupModel>(EditMembershipGroupCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            await OnLoadMembershipGroups();

            IsBusy = false;
        }

        private async Task OnLoadMembershipGroups()
        {
            IsBusy = true;

            _adminDataUnit.MembershipGroupsRepository.Refresh();
            var membershipGroups = await _adminDataUnit.MembershipGroupsRepository.GetAllAsync();
            AllMembershipGroups = new List<MembershipGroupModel>(membershipGroups.Select(x => new MembershipGroupModel(x)));

            RefreshMembershipGroups();

            IsBusy = false;
        }

        public void RefreshMembershipGroups()
        {
            MembershipGroups = new ObservableCollection<MembershipGroupModel>(AllMembershipGroups);
        }

        #endregion

        #region Commands

        private async void DeleteMembershipGroupCommandExecuted(MembershipGroupModel membershipGroup)
        {
            if (membershipGroup == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            // Check if Category Group has dependencies
            if (membershipGroup.MembershipGroup.MembershipCategories.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine("Sorry, we can't delete this category group :(");
                sb.AppendLine("This category group already in use by these categories:");

                membershipGroup.MembershipGroup.MembershipCategories.Select(x => x.Name).ForEach(x => sb.AppendLine(string.Format("- {0}", x)));

                RaisePropertyChanged("DisableParentWindow");

                RadWindow.Alert(sb.ToString());

                RaisePropertyChanged("EnableParentWindow");

                return;
            }

            // Delete MembershipGroupEPOS
            _adminDataUnit.MembershipGroupEPOSRepository.Delete(membershipGroup.MembershipGroup.MembershipGroupEPOS);

            // Delete MembershipGroup
            _adminDataUnit.MembershipGroupsRepository.Delete(membershipGroup.MembershipGroup);

            await _adminDataUnit.SaveChanges();

            MembershipGroups.Remove(membershipGroup);
        }

        private void EditMembershipGroupCommandExecuted(MembershipGroupModel membershipGroup)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddMembershipGroupView(membershipGroup);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true)
            {
                _adminDataUnit.MembershipGroupsRepository.Refresh();
                RefreshMembershipGroups();
            }
        }

        #endregion
    }
}
