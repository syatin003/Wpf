using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Admin.Members.Age;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Members.Age
{
    public class MembershipGroupAgesViewModel : ViewModelBase
    {

        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;

        private ObservableCollection<MembershipGroupAgeModel> _membershipGroupAges;

        #endregion

        #region Properties

        public List<MembershipGroupAgeModel> AllMembershipGroupAges { get; set; }

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

        public ObservableCollection<MembershipGroupAgeModel> MembershipGroupAges
        {
            get { return _membershipGroupAges; }
            set
            {
                if (_membershipGroupAges == value) return;
                _membershipGroupAges = value;
                RaisePropertyChanged(() => MembershipGroupAges);
            }
        }
        public RelayCommand<MembershipGroupAgeModel> DeleteMembershipGroupAgeCommand { get; private set; }
        public RelayCommand<MembershipGroupAgeModel> EditMembershipGroupAgeCommand { get; private set; }

        #endregion

        #region Constructors

        public MembershipGroupAgesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            DeleteMembershipGroupAgeCommand = new RelayCommand<MembershipGroupAgeModel>(DeleteMembershipGroupAgeCommandExecuted);
            EditMembershipGroupAgeCommand = new RelayCommand<MembershipGroupAgeModel>(EditMembershipGroupAgeCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.MembershipGroupAgesRepository.Refresh();

            var membershipGroupAges = await _adminDataUnit.MembershipGroupAgesRepository.GetAllAsync();
            AllMembershipGroupAges = new List<MembershipGroupAgeModel>(membershipGroupAges.Select(x => new MembershipGroupAgeModel(x)));

            RefreshMembershipGroupAges();

            IsBusy = false;
        }
        public void RefreshMembershipGroupAges()
        {
            MembershipGroupAges = new ObservableCollection<MembershipGroupAgeModel>(AllMembershipGroupAges);
        }

        #endregion

        #region Commands

        private async void DeleteMembershipGroupAgeCommandExecuted(MembershipGroupAgeModel membershipGroupAge)
        {
            if (membershipGroupAge == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            // Check if Category Age has dependencies
            if (membershipGroupAge.MembershipGroupAge.MembershipCategories.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine("Sorry, we can't delete this category age :(");
                sb.AppendLine("This category age already in use by these categories:");

                membershipGroupAge.MembershipGroupAge.MembershipCategories.Select(x => x.Name).ForEach(x => sb.AppendLine(string.Format("- {0}", x)));

                RaisePropertyChanged("DisableParentWindow");

                RadWindow.Alert(sb.ToString());

                RaisePropertyChanged("EnableParentWindow");

                return;
            }

            // Delete MembershipGroupAge
            _adminDataUnit.MembershipGroupAgesRepository.Delete(membershipGroupAge.MembershipGroupAge);

            await _adminDataUnit.SaveChanges();

            MembershipGroupAges.Remove(membershipGroupAge);
        }

        private void EditMembershipGroupAgeCommandExecuted(MembershipGroupAgeModel membershipGroupAge)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddMembershipGroupAgeView(membershipGroupAge);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true)
            {
                _adminDataUnit.MembershipGroupAgesRepository.Refresh();
            }
        }

        #endregion
    }
}
