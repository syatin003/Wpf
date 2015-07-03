using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using EventManagementSystem.Views.ContactManager.ContactManagerTabs;
using EventManagementSystem.Views.CRM;
using EventManagementSystem.Views.Membership;
using EventManagementSystem.Views.Membership.MembershipTabs;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Data.Model;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using System.Text.RegularExpressions;
using Telerik.Windows.Controls;
using System.Windows;

namespace EventManagementSystem.ViewModels.Membership
{
    public class MembershipViewModel : ViewModelBase
    {
        #region Fields

        private readonly IMembershipDataUnit _membershipDataUnit;
        private ObservableCollection<MemberModel> _members;
        private List<MemberModel> _allMembers;
        private bool _isBusy;
        private MemberModel _selectedMember;
        private MemberModel _originalMember;

        private ContentControl _contactDetailsContent;
        private ContentControl _correspondenceContent;
        private ContentControl _activityContent;
        private ContentControl _accountsContent;
        private ContentControl _memberDetailsContent;
        private ContentControl _memberNotesContent;
        private ContentControl _memberUpdateDetailsContent;
        private int _selectedTab;
        private bool _isAllMembersChecked;
        private bool _includeInEmailPropertyChanged;

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

        public ContentControl ContactDetailsContent
        {
            get { return _contactDetailsContent; }
            set
            {
                if (Equals(_contactDetailsContent, value)) return;
                _contactDetailsContent = value;
                RaisePropertyChanged(() => ContactDetailsContent);
            }
        }

        public ContentControl CorrespondenceContent
        {
            get { return _correspondenceContent; }
            set
            {
                if (_correspondenceContent == value) return;
                _correspondenceContent = value;
                RaisePropertyChanged(() => CorrespondenceContent);
            }
        }

        public ContentControl ActivityContent
        {
            get { return _activityContent; }
            set
            {
                if (_activityContent == value) return;
                _activityContent = value;
                RaisePropertyChanged(() => ActivityContent);
            }
        }

        public ContentControl AccountsContent
        {
            get { return _accountsContent; }
            set
            {
                if (_accountsContent == value) return;
                _accountsContent = value;
                RaisePropertyChanged(() => AccountsContent);
            }
        }

        public ContentControl MemberDetailsContent
        {
            get { return _memberDetailsContent; }
            set
            {
                if (_memberDetailsContent == value) return;
                _memberDetailsContent = value;
                RaisePropertyChanged(() => MemberDetailsContent);
            }
        }

        public ContentControl MemberNotesContent
        {
            get { return _memberNotesContent; }
            set
            {
                if (_memberNotesContent == value) return;
                _memberNotesContent = value;
                RaisePropertyChanged(() => MemberNotesContent);
            }
        }

        public ContentControl MemberUpdateDetailsContent
        {
            get { return _memberUpdateDetailsContent; }
            set
            {
                if (_memberUpdateDetailsContent == value) return;
                _memberUpdateDetailsContent = value;
                RaisePropertyChanged(() => MemberUpdateDetailsContent);
            }
        }
        public ObservableCollection<MemberModel> Members
        {
            get { return _members; }
            set
            {
                if (_members == value) return;
                _members = value;
                RaisePropertyChanged(() => Members);
                SendEmailCommand.RaiseCanExecuteChanged();
            }
        }

        public MemberModel SelectedMember
        {
            get
            { return _selectedMember; }
            set
            {
                if (_selectedMember == value) return;
                _selectedMember = value;
                RaisePropertyChanged(() => SelectedMember);

                ShowSelectedContactTabs(SelectedMember);
            }
        }

        private void ShowSelectedContactTabs(MemberModel member)
        {
            if (member != null)
            {
                _originalMember = member.Clone();
                if (CanViewContactDetails)
                    ContactDetailsContent = new ContactDetailsView(member.Contact, true, member);
                if (CanViewCorrespondence)
                    CorrespondenceContent = new CorrespondenceView(member.Contact,"Member");
                if (CanViewActivity)
                    ActivityContent = new ActivityView(member.Contact);
                if (CanViewAccounts)
                    AccountsContent = new AccountsView(member.Contact);
                if (CanViewMemberDetails)
                    MemberDetailsContent = new MemberDetailsView(member);
                if (CanViewNoteDetails)
                    MemberNotesContent = new MemberNotesView(member);
                if (CanViewUpdateDetails)
                    MemberUpdateDetailsContent = new MemberUpdateDetailsView(member);
            }
        }

        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab == value) return;
                _selectedTab = value;
            }
        }

        public bool CanViewContactDetails { get; private set; }

        public bool CanViewCorrespondence { get; private set; }

        public bool CanViewActivity { get; private set; }

        public bool CanViewAccounts { get; private set; }

        public bool CanViewMemberDetails { get; private set; }

        public bool CanViewNoteDetails { get; private set; }

        public bool CanViewUpdateDetails { get; private set; }

        public bool IsAllMembersChecked
        {
            get { return _isAllMembersChecked; }
            set
            {
                _isAllMembersChecked = value;
                RaisePropertyChanged(() => IsAllMembersChecked);
                if (!_includeInEmailPropertyChanged)
                    IncludeExcludeMembersForEmail();
                else
                    _includeInEmailPropertyChanged = false;
            }
        }

        public RelayCommand AddMemberCommand { get; private set; }
        public RelayCommand SendEmailCommand { get; private set; }

        #endregion

        #region Constructor

        public MembershipViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();

            AddMemberCommand = new RelayCommand(AddMemberCommandExecuted, AddMemberCommandCanExecute);
            SendEmailCommand = new RelayCommand(SendEmailCommandExecuted, SendEmailCommandCanExecute);

            CanViewAccounts = AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBERSHIP_ACCOUNTS_TAB_ALLOWED);
            CanViewActivity = AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBERSHIP_ACTIVITY_TAB_ALLOWED);
            CanViewContactDetails = AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBERSHIP_CONTACTS_TAB_ALLOWED);
            CanViewCorrespondence = AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBERSHIP_CORRESPONDENCE_TAB_ALLOWED);
            CanViewMemberDetails = AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBER_DETAILS_TAB_ALLOWED);
            CanViewNoteDetails = AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBERSHIP_NOTES_TAB_ALLOWED);
            CanViewUpdateDetails = AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBER_UPDATE_DETAILS_TAB_ALLOWED);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _membershipDataUnit.MembersRepository.Refresh();
            var members = await _membershipDataUnit.MembersRepository.GetAllAsync();
            _allMembers = new List<MemberModel>(members.OrderBy(member => member.Contact.LastName).Select(member => new MemberModel(member)));
            Members = new ObservableCollection<MemberModel>(_allMembers);

            Members.ForEach(member =>
                member.PropertyChanged += member_PropertyChanged);

            IsBusy = false;
        }

        private void member_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IncludeInEmail")
            {
                bool isAllMembersIncluded = true;
                _includeInEmailPropertyChanged = true;
                foreach (var member in Members)
                {
                    bool includeInEmail = member.IncludeInEmail;
                    if (!includeInEmail)
                    {
                        isAllMembersIncluded = false;
                        break;
                    }
                }
                IsAllMembersChecked = isAllMembersIncluded;

                SendEmailCommand.RaiseCanExecuteChanged();
            }
        }

        public void ChangeSelectedMember()
        {
            ContactDetailsContent = null;
            CorrespondenceContent = null;
            ActivityContent = null;
            AccountsContent = null;
            MemberDetailsContent = null;
            MemberNotesContent = null;
        }

        private void ProcessUpdates(MemberModel member, List<MembershipUpdate> membershipUpdates)
        {
            membershipUpdates.ForEach(update =>
            {
                member.MembershipUpdates.Insert(0, update);
                _membershipDataUnit.MembershipUpdatesRepository.Add(update);
            });

            member.MembershipUpdates = new ObservableCollection<MembershipUpdate>(member.MembershipUpdates.OrderByDescending(x => x.Date));
        }

        private void IncludeExcludeMembersForEmail()
        {
            if (IsAllMembersChecked)
            {
                Members.ForEach(member =>
                    {
                        member.IncludeInEmail = true;
                    });
            }
            else
            {
                Members.ForEach(member =>
                {
                    member.IncludeInEmail = false;
                });
            }
        }

        #endregion

        #region Commands

        private void AddMemberCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addMemberView = new AddMemberView();
            addMemberView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (addMemberView.DialogResult != null && addMemberView.DialogResult == true)
            {
                if (!addMemberView.ViewModel.IsExistingMember)
                    _allMembers.Add(addMemberView.ViewModel.Member);
                Members = new ObservableCollection<MemberModel>(_allMembers.OrderBy(x => x.Contact.LastName));
                SelectedMember = addMemberView.ViewModel.IsExistingMember ? Members.FirstOrDefault(member => member.Member == addMemberView.ViewModel.Member.Member) : addMemberView.ViewModel.Member;
                RaisePropertyChanged("ScrollToSelectedItem");
            }
        }

        private bool AddMemberCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Resources.PERMISSION_ADD_MEMBER_ALLOWED);
        }

        private void SendEmailCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            var memberHavingValidEmail = new ObservableCollection<MemberModel>(Members.Where(member =>
                member.IncludeInEmail && !string.IsNullOrWhiteSpace(member.Contact.Email) && regex.IsMatch(member.Contact.Email)));
            if (!memberHavingValidEmail.Any())
            {
                string confirmText = "None of the selected members having valid email!";

                RadWindow.Alert(new DialogParameters
                  {
                      Owner = Application.Current.MainWindow,
                      Content = confirmText
                  });

                RaisePropertyChanged("EnableParentWindow");

                return;
            }

            var membersIncludeInEmail = new ObservableCollection<MemberModel>(Members.Where(member => member.IncludeInEmail));
            var sendEmailView = new SendEmailView(membersIncludeInEmail);
            sendEmailView.ShowDialog();
            RaisePropertyChanged("EnableParentWindow");

            if (sendEmailView.DialogResult != null && sendEmailView.DialogResult == true)
            {

            }
        }

        private bool SendEmailCommandCanExecute()
        {
            return Members != null && Members.Any(member => member.IncludeInEmail);
        }

        #endregion
    }
}
