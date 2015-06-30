using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Admin.Members.Style;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Members.Style
{
    public class MembershipGroupStylesViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;

        private ObservableCollection<MembershipGroupStyleModel> _membershipGroupStyles;

        #endregion

        #region Properties

        public List<MembershipGroupStyleModel> AllMembershipGroupStyles { get; set; }

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

        public ObservableCollection<MembershipGroupStyleModel> MembershipGroupStyles
        {
            get { return _membershipGroupStyles; }
            set
            {
                if (_membershipGroupStyles == value) return;
                _membershipGroupStyles = value;
                RaisePropertyChanged(() => MembershipGroupStyles);
            }
        }
        public RelayCommand<MembershipGroupStyleModel> DeleteMembershipGroupStyleCommand { get; private set; }
        public RelayCommand<MembershipGroupStyleModel> EditMembershipGroupStyleCommand { get; private set; }

        #endregion

        #region Constructors

        public MembershipGroupStylesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            DeleteMembershipGroupStyleCommand = new RelayCommand<MembershipGroupStyleModel>(DeleteMembershipGroupStyleCommandExecuted);
            EditMembershipGroupStyleCommand = new RelayCommand<MembershipGroupStyleModel>(EditMembershipGroupStyleCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.MembershipGroupStylesRepository.Refresh();

            var membershipGroupStyles = await _adminDataUnit.MembershipGroupStylesRepository.GetAllAsync();
            AllMembershipGroupStyles = new List<MembershipGroupStyleModel>(membershipGroupStyles.Select(x => new MembershipGroupStyleModel(x)));

            RefreshMembershipGroupStyles();

            IsBusy = false;
        }
        public void RefreshMembershipGroupStyles()
        {
            MembershipGroupStyles = new ObservableCollection<MembershipGroupStyleModel>(AllMembershipGroupStyles);
        }

        #endregion

        #region Commands

        private async void DeleteMembershipGroupStyleCommandExecuted(MembershipGroupStyleModel membershipGroupStyle)
        {
            if (membershipGroupStyle == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            // Check if Category Style has dependencies
            if (membershipGroupStyle.MembershipGroupStyle.MembershipCategories.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine("Sorry, we can't delete this category style :(");
                sb.AppendLine("This category style already in use by these categories:");

                membershipGroupStyle.MembershipGroupStyle.MembershipCategories.Select(x => x.Name).ForEach(x => sb.AppendLine(string.Format("- {0}", x)));

                RaisePropertyChanged("DisableParentWindow");

                RadWindow.Alert(sb.ToString());

                RaisePropertyChanged("EnableParentWindow");

                return;
            }

            // Delete MembershipGroupStyle
            _adminDataUnit.MembershipGroupStylesRepository.Delete(membershipGroupStyle.MembershipGroupStyle);

            await _adminDataUnit.SaveChanges();

            MembershipGroupStyles.Remove(membershipGroupStyle);
        }

        private void EditMembershipGroupStyleCommandExecuted(MembershipGroupStyleModel membershipGroupStyle)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddMembershipGroupStyleView(membershipGroupStyle);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true)
            {
                _adminDataUnit.MembershipGroupStylesRepository.Refresh();
            }
        }

        #endregion
    }
}
