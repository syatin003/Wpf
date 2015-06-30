using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Enums.Membership;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Core.Contacts;
using Microsoft.Practices.Unity;
using EventManagementSystem.Services;
using EventManagementSystem.Core.Serialization;

namespace EventManagementSystem.ViewModels.Membership
{
    public class AddMemberViewModel : ViewModelBase
    {
        #region Fields

        private readonly IMembershipDataUnit _membershipDataUnit;
        private bool _isBusy;
        private ObservableCollection<ContactTitle> _contactTitles;
        private MemberModel _member;
        private MemberModel _originalMember;
        private ObservableCollection<MembershipCategory> _memberCategories;

        private bool _isEditMode;
        private bool _isIgnored;
        private bool _isExistingContact;
        private bool _isExistingMember;
        private bool _isOkButtonClick;

        private SystemSetting _clubCodeSetting;
        private SystemSetting _currentMemberNumberSetting;

        #endregion

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

        public ObservableCollection<ContactTitle> ContactTitles
        {
            get { return _contactTitles; }
            set
            {
                if (_contactTitles == value) return;
                _contactTitles = value;
                RaisePropertyChanged(() => ContactTitles);
            }
        }

        public MemberModel Member
        {
            get { return _member; }
            set
            {
                if (_member == value) return;
                _member = value;
                RaisePropertyChanged(() => Member);
            }
        }

        public ObservableCollection<MembershipCategory> MemberCategories
        {
            get { return _memberCategories; }
            set
            {
                if (_memberCategories == value) return;
                _memberCategories = value;
                RaisePropertyChanged(() => MemberCategories);
            }
        }

        public List<Status> Statuses
        {
            get
            { return Enum.GetValues(typeof(Status)).Cast<Status>().ToList(); }
        }

        public bool IsIgnored
        {
            get { return _isIgnored; }
            set
            {
                if (_isIgnored == value) return;
                _isIgnored = value;
                RaisePropertyChanged(() => IsIgnored);
            }
        }
        public bool IsExistingContact
        {
            get { return _isExistingContact; }
            set
            {
                if (_isExistingContact == value) return;
                _isExistingContact = value;
                RaisePropertyChanged(() => IsExistingContact);
            }
        }
        public bool IsExistingMember
        {
            get { return _isExistingMember; }
            set
            {
                if (_isExistingMember == value) return;
                _isExistingMember = value;
                RaisePropertyChanged(() => IsExistingMember);
            }
        }
        public bool IsOkButtonClick
        {
            get { return _isOkButtonClick; }
            set
            {
                if (_isOkButtonClick == value) return;
                _isOkButtonClick = value;
                RaisePropertyChanged(() => IsOkButtonClick);
            }
        }

        public SystemSetting ClubCodeSetting
        {
            get { return _clubCodeSetting; }
            set
            {
                if (_clubCodeSetting == value) return;
                _clubCodeSetting = value;
                RaisePropertyChanged(() => ClubCodeSetting);
            }
        }

        public SystemSetting CurrentMemberNumberSetting
        {
            get { return _currentMemberNumberSetting; }
            set
            {
                if (_currentMemberNumberSetting == value) return;
                _currentMemberNumberSetting = value;
                RaisePropertyChanged(() => CurrentMemberNumberSetting);
            }
        }

        public RelayCommand ShowFindContactWindowCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        #endregion

        #region Constructors

        public AddMemberViewModel(MemberModel member)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();

            ShowFindContactWindowCommand = new RelayCommand(ShowFindContactWindowCommandExecuted);
            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);

            ProcessMember(member);
        }

        #endregion

        #region Methods

        private void ProcessMember(MemberModel member)
        {
            _isEditMode = (member != null);

            Member = member ?? GetMember();

            _originalMember = Member.Clone();

            Member.PropertyChanged += MemberOnPropertyChanged;

            Member.Contact.PropertyChanged += Contact_PropertyChanged;
        }

        private MemberModel GetMember()
        {
            var newMember = new MemberModel(new Member
            {
                ID = Guid.NewGuid(),
            });
            newMember.Contact = new ContactModel(new Contact
            {
                ID = newMember.Member.ID
            });
            return newMember;
        }

        public async void LoadData()
        {
            IsBusy = true;

            _membershipDataUnit.MembershipCategoriesRepository.Refresh();
            var categories = await _membershipDataUnit.MembershipCategoriesRepository.GetAllAsync(category => category.MembershipGroupStyle.ClassifiedAsMember);
            MemberCategories = new ObservableCollection<MembershipCategory>(categories.OrderBy(category => category.Name));

            var titles = await _membershipDataUnit.ContactTitlesRepository.GetAllAsync();
            ContactTitles = new ObservableCollection<ContactTitle>(titles.OrderBy(x => x.Title));

            var clubCodeSetting = await _membershipDataUnit.SystemSettingsRepository.GetSettingByName("ClubCode");
            ClubCodeSetting = clubCodeSetting;

            var currentMemberNumberSetting = await _membershipDataUnit.SystemSettingsRepository.GetSettingByName("CurrentMemberNumber");
            CurrentMemberNumberSetting = currentMemberNumberSetting;

            IsBusy = false;
        }

        private void MemberOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void Contact_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "LastName")
            {
                IsIgnored = false;
            }
            if (args.PropertyName == "Email")
            {
                IsIgnored = false;
            }
            SaveCommand.RaiseCanExecuteChanged();
        }

        private async void ProcessUpdates()
        {
            if (!Member.MembershipUpdates.Any())
            {
                var updates = await _membershipDataUnit.MembershipUpdatesRepository.GetAllAsync(x => x.MemberID == Member.Member.ID);
                Member.MembershipUpdates = new ObservableCollection<MembershipUpdate>(updates.OrderByDescending(x => x.Date));
            }

            var membershipUpdates = new List<MembershipUpdate>();

            if (_isEditMode)
            {
                membershipUpdates = LoggingService.FindDifference(_originalMember, Member, "Member");
            }
            else
            {
                membershipUpdates = LoggingService.FindDifference(_originalMember, Member, "Member", true);
            }

            membershipUpdates.ForEach(update =>
            {
                Member.MembershipUpdates.Insert(0, update);
                _membershipDataUnit.MembershipUpdatesRepository.Add(update);
            });

            Member.MembershipUpdates = new ObservableCollection<MembershipUpdate>(Member.MembershipUpdates.OrderByDescending(x => x.Date));
            await _membershipDataUnit.SaveChanges();
        }

        public async Task ShowReleventContact()
        {
            var allcontacts =
                       await _membershipDataUnit.ContactsRepository.GetAllAsyncWithoutRefresh(contact =>
                           contact.LastName == Member.Contact.LastName && contact.Email == Member.Contact.Email);

            var contacts = new ObservableCollection<ContactModel>(
                    allcontacts.OrderBy(contact => contact.LastName).Select(contact => new ContactModel(contact)));
            if (contacts.Count > 0)
            {
                RaisePropertyChanged("DisableParentWindow");

                var relevantContactsView = new ReleventContactsView(contacts);
                relevantContactsView.ShowDialog();

                RaisePropertyChanged("EnableParentWindow");

                if (relevantContactsView.DialogResult != null && relevantContactsView.DialogResult.Value)
                {
                    if (IsExistingContact)
                        _membershipDataUnit.ContactsRepository.Refresh(Member.Contact.Contact);
                    if (relevantContactsView.ViewModel.Contact.Contact.Member != null)
                    {
                        Member = new MemberModel(relevantContactsView.ViewModel.Contact.Contact.Member)
                        {
                            Contact = relevantContactsView.ViewModel.Contact,
                            Category = MemberCategories.FirstOrDefault(category => category.ID == relevantContactsView.ViewModel.Contact.Contact.Member.MembershipCategory.ID)
                        };
                        Member.Contact.PropertyChanged += Contact_PropertyChanged;
                        _isEditMode = true;
                        IsExistingMember = true;
                        _originalMember = Member.Clone();
                        SaveCommand.RaiseCanExecuteChanged();
                    }
                    else
                    {
                        Member.Contact = relevantContactsView.ViewModel.Contact;
                        Member.Member.ID = Member.Contact.Contact.ID;
                        Member.Contact.PropertyChanged += Contact_PropertyChanged;
                    }
                    IsExistingContact = true;
                    IsIgnored = true;
                    if (IsOkButtonClick)
                    {
                        if (!_isEditMode)
                            SaveContactChangesAddMember();
                    }
                }
                if (relevantContactsView.DialogResult == null || !relevantContactsView.DialogResult.Value)
                {
                    if (IsOkButtonClick)
                    {
                        _membershipDataUnit.ContactsRepository.Add(Member.Contact.Contact);
                        SaveContactChangesAddMember();
                    }
                    else
                        IsIgnored = true;
                }
            }
            else
            {
                if (IsOkButtonClick)
                {
                    if (!IsExistingContact)
                        _membershipDataUnit.ContactsRepository.Add(Member.Contact.Contact);
                    SaveContactChangesAddMember();
                }
            }
        }

        private void SaveContactChangesAddMember()
        {
            if (!_isEditMode)
            {
                Member.Member.MemberReference = string.Concat(ClubCodeSetting.Value, CurrentMemberNumberSetting.Value);
                CurrentMemberNumberSetting.Value = Convert.ToString(Convert.ToInt32(CurrentMemberNumberSetting.Value) + 1);
                _membershipDataUnit.MembersRepository.Add(Member.Member);
            }
            ProcessUpdates();
            RaisePropertyChanged("CloseDialog");
        }

        #endregion

        #region Commands

        private async void SaveCommandExecuted()
        {
            IsBusy = true;

            IsOkButtonClick = true;
            if (IsExistingContact)
            {
                if (IsIgnored)
                {
                    if (!_isEditMode)
                        Member.Member.ID = Member.Contact.Contact.ID;
                    SaveContactChangesAddMember();

                }
                else
                    if (!string.IsNullOrEmpty(Member.Contact.Email))
                        await ShowReleventContact();
                    else
                        SaveContactChangesAddMember();
            }

            else
            {
                if (IsIgnored)
                {
                    _membershipDataUnit.ContactsRepository.Add(Member.Contact.Contact);
                    SaveContactChangesAddMember();
                }
                else
                {
                    if (!string.IsNullOrEmpty(Member.Contact.Email))
                        await ShowReleventContact();
                    else
                    {
                        _membershipDataUnit.ContactsRepository.Add(Member.Contact.Contact);
                        SaveContactChangesAddMember();

                    }
                }
            }

            IsBusy = false;
        }

        private void ShowFindContactWindowCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new ContactsListView(true);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult.Value && view.ViewModel.SelectedContact != null)
            {
                Member.Contact = view.ViewModel.SelectedContact;
                Member.Member.ID = Member.Contact.Contact.ID;
                IsExistingContact = true;
                IsIgnored = true;
                Member.Contact.PropertyChanged += Contact_PropertyChanged;
            }
        }

        private bool SaveCommandCanExecute()
        {
            if (IsBusy)
                return false;
            return !Member.HasErrors && !Member.Contact.HasErrors;
        }

        #endregion


    }
}
