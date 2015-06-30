using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;

namespace EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs
{
    public class ContactDetailsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IContactsDataUnit _contactsDataUnit;
        private readonly IMembershipDataUnit _membershipDataUnit;

        private bool _isBusy;
        private ObservableCollection<ContactTitle> _contactTitles;
        private ContactModel _contactModel;
        private bool _isFromMembership;
        private MemberModel _originalMember;
        private MemberModel _member;

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

        public ContactModel ContactModel
        {
            get { return _contactModel; }
            set
            {
                if (_contactModel == value) return;
                _contactModel = value;
                RaisePropertyChanged(() => ContactModel);
            }
        }

        public bool IsFromMembership
        {
            get { return _isFromMembership; }
            set
            {
                _isFromMembership = value;
                RaisePropertyChanged(() => _isFromMembership);
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
        public RelayCommand SaveChangesCommand { get; private set; }

        public bool CanEdit { get; private set; }

        #endregion

        #region Constructors

        public ContactDetailsViewModel(ContactModel model, bool isFromMembership, MemberModel member)
        {
            IsFromMembership = isFromMembership;

            if (member != null)
            {
                _originalMember = member.Clone();
                Member = member;
            }

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            if (IsFromMembership)
                _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();
            else
                _contactsDataUnit = dataUnitLocator.ResolveDataUnit<IContactsDataUnit>();

            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecute, SaveChangesCommandCanExecute);

            if (IsFromMembership)
                CanEdit = AccessService.Current.UserHasPermissions(Resources.PERMISSION_MEMBERSHIP_EDIT_CONTACT_ALLOWED);
            else
                CanEdit = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_CONTACT_ALLOWED);

            if (IsFromMembership)
                ProcessContact(Member.Contact);
            else
                ProcessContact(model);
        }

        #endregion

        #region Methods

        private void ProcessContact(ContactModel model)
        {
            ContactModel = model;
            ContactModel.PropertyChanged += ContactModelOnPropertyChanged;
        }

        public async void LoadData()
        {
            IsBusy = true;
            if (IsFromMembership)
            {
                var titles = await _membershipDataUnit.ContactTitlesRepository.GetAllAsync();
                ContactTitles = new ObservableCollection<ContactTitle>(titles.OrderBy(x => x.Title));
            }
            else
            {
                var titles = await _contactsDataUnit.ContactTitlesRepository.GetAllAsync();
                ContactTitles = new ObservableCollection<ContactTitle>(titles.OrderBy(x => x.Title));
            }
            IsBusy = false;
        }

        private void ContactModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private async void ProcessUpdates()
        {
            if (!Member.MembershipUpdates.Any())
            {
                var updates = await _membershipDataUnit.MembershipUpdatesRepository.GetAllAsync(x => x.MemberID == Member.Member.ID);
                Member.MembershipUpdates = new ObservableCollection<MembershipUpdate>(updates.OrderByDescending(x => x.Date));
            }
            var membershipUpdates = LoggingService.FindDifference(_originalMember, Member, "Member");
            membershipUpdates.ForEach(update =>
            {
                Member.MembershipUpdates.Insert(0, update);
                _membershipDataUnit.MembershipUpdatesRepository.Add(update);
            });

            Member.MembershipUpdates = new ObservableCollection<MembershipUpdate>(Member.MembershipUpdates.OrderByDescending(x => x.Date));

            await _membershipDataUnit.SaveChanges();
            _originalMember = Member.Clone();
        }
        #endregion

        #region Commands

        private void SaveChangesCommandExecute()
        {
            if (IsFromMembership)
                ProcessUpdates();
            else
                _contactsDataUnit.SaveChanges();
        }

        private bool SaveChangesCommandCanExecute()
        {
            return !_contactModel.HasErrors && CanEdit;
        }

        #endregion
    }
}
