using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Enums.Membership;
using EventManagementSystem.Models;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;

namespace EventManagementSystem.ViewModels.Membership.MembershipTabs
{
    public class MemberDetailsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IMembershipDataUnit _membershipDataUnit;
        private bool _isBusy;
        private ObservableCollection<ContactTitle> _contactTitles;
        private MemberModel _member;
        private MemberModel _originalMember;
        private ObservableCollection<MembershipCategory> _memberCategories;

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

        public RelayCommand SaveChangesCommand { get; private set; }

        public bool CanEdit { get; private set; }

        #endregion

        #region Constructors

        public MemberDetailsViewModel(MemberModel model)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();

            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecute, SaveChangesCommandCanExecute);

            CanEdit = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_MEMBER_ALLOWED);

            ProcessMember(model);
        }

        #endregion

        #region Methods

        private void ProcessMember(MemberModel member)
        {
            Member = member;
            _originalMember = Member.Clone();
            Member.PropertyChanged += MemberModelOnPropertyChanged;
        }

        public async void LoadData()
        {
            IsBusy = true;

            _membershipDataUnit.MembershipCategoriesRepository.Refresh();
            var categories = await _membershipDataUnit.MembershipCategoriesRepository.GetAllAsync(category => category.MembershipGroupStyle.ClassifiedAsMember);
            MemberCategories = new ObservableCollection<MembershipCategory>(categories.OrderBy(category => category.Name));

            var titles = await _membershipDataUnit.ContactTitlesRepository.GetAllAsync();
            ContactTitles = new ObservableCollection<ContactTitle>(titles.OrderBy(x => x.Title));

            IsBusy = false;
        }

        private void MemberModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
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
            ProcessUpdates();
        }

        private bool SaveChangesCommandCanExecute()
        {
            return !Member.HasErrors;
        }

        #endregion
    }
}
