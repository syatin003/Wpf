using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class LeaversDescriptionViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private bool _isBusy;
        private List<MemberModel> _allMembers;
        private ObservableCollection<LeaverModel> _leavers;
        private DateTime _startDate;
        private DateTime _endDate;

        private bool _incResignDate;
        private bool _incLeavingDate;
        private bool _incMembershipNumberLeavers;
        private bool _incMemberNameLeavers;
        private bool _incCategoryNameLeavers;
        private bool _incReason;
        private bool _incNotes;
        private bool _incLinkedMembers;
        private bool _incMembershipStart;
        private bool _incMembershipEnd;
        private bool _incContractPeriodLeavers;
        private bool _incLastDDMonth;

        #endregion Fields

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

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                Properties.Settings.Default.StartDateOptionLeavers = _startDate;
                RaisePropertyChanged(() => StartDate);
                UpdateLeaversData();

            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                Properties.Settings.Default.EndDateOptionLeavers = _endDate;
                RaisePropertyChanged(() => EndDate);
                UpdateLeaversData();
            }
        }

        public bool IncResignDate
        {
            get { return _incResignDate; }
            set
            {
                if (_incResignDate == value) return;
                _incResignDate = value;
                RaisePropertyChanged(() => IncResignDate);

            }
        }

        public bool IncLeavingDate
        {
            get { return _incLeavingDate; }
            set
            {
                if (_incLeavingDate == value) return;
                _incLeavingDate = value;
                RaisePropertyChanged(() => IncLeavingDate);

            }
        }

        public bool IncMembershipNumberLeavers
        {
            get { return _incMembershipNumberLeavers; }
            set
            {
                if (_incMembershipNumberLeavers == value) return;
                _incMembershipNumberLeavers = value;
                RaisePropertyChanged(() => IncMembershipNumberLeavers);

            }
        }

        public bool IncMemberNameLeavers
        {
            get { return _incMemberNameLeavers; }
            set
            {
                if (_incMemberNameLeavers == value) return;
                _incMemberNameLeavers = value;
                RaisePropertyChanged(() => IncMemberNameLeavers);

            }
        }

        public bool IncCategoryNameLeavers
        {
            get { return _incCategoryNameLeavers; }
            set
            {
                if (_incCategoryNameLeavers == value) return;
                _incCategoryNameLeavers = value;
                RaisePropertyChanged(() => IncCategoryNameLeavers);
            }
        }

        public bool IncReason
        {
            get { return _incReason; }
            set
            {
                if (_incReason == value) return;
                _incReason = value;
                RaisePropertyChanged(() => IncReason);
            }
        }

        public bool IncNotes
        {
            get { return _incNotes; }
            set
            {
                if (_incNotes == value) return;
                _incNotes = value;
                RaisePropertyChanged(() => IncNotes);
            }
        }

        public bool IncLinkedMembers
        {
            get { return _incLinkedMembers; }
            set
            {
                if (_incLinkedMembers == value) return;
                _incLinkedMembers = value;
                RaisePropertyChanged(() => IncLinkedMembers);
            }
        }

        public bool IncMembershipStart
        {
            get { return _incMembershipStart; }
            set
            {
                if (_incMembershipStart == value) return;
                _incMembershipStart = value;
                RaisePropertyChanged(() => IncMembershipStart);
            }
        }


        public bool IncMembershipEnd
        {
            get { return _incMembershipEnd; }
            set
            {
                if (_incMembershipEnd == value) return;
                _incMembershipEnd = value;
                RaisePropertyChanged(() => IncMembershipEnd);
            }
        }

        public bool IncContractPeriodLeavers
        {
            get { return _incContractPeriodLeavers; }
            set
            {
                if (_incContractPeriodLeavers == value) return;
                _incContractPeriodLeavers = value;
                RaisePropertyChanged(() => IncContractPeriodLeavers);
            }
        }

        public bool IncLastDDMonth
        {
            get { return _incLastDDMonth; }
            set
            {
                if (_incLastDDMonth == value) return;
                _incLastDDMonth = value;
                RaisePropertyChanged(() => IncLastDDMonth);
            }
        }


        public ObservableCollection<LeaverModel> Leavers
        {
            get { return _leavers; }
            set
            {
                if (_leavers == value) return;
                _leavers = value;
                RaisePropertyChanged(() => Leavers);
            }
        }

        #endregion Properties

        #region Constructor

        public LeaversDescriptionViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();
            _allMembers = new List<MemberModel>();
        }

        #endregion  Constructor

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            LoadOptions();

            _reportsDataUnit.MembersRepository.Refresh();
            var members = await _reportsDataUnit.MembersRepository.GetAllAsync();
            _allMembers = new List<MemberModel>(members.Select(member => new MemberModel(member)));

            UpdateLeaversData();

            IsBusy = false;
        }

        private void UpdateLeaversData()
        {
            var members = (_allMembers.Where(member => member.Member.ResignDate >= StartDate && member.Member.ResignDate <= EndDate));
            Leavers = new ObservableCollection<LeaverModel>();
            foreach (var memberItem in members)
            {
                var leaver = new LeaverModel
                {
                    ResignDate = memberItem.Member.ResignDate,
                    MemberName = string.Format("{0} {1}", (memberItem.Contact.Title) != null ? memberItem.Contact.Title.Title : "", memberItem.Contact.ContactName),
                    CategoryName = memberItem.Category.Name

                    // Other fields be Available when we will add the accounts package

                };
                if (leaver.MembershipStart == default(DateTime))
                    leaver.MembershipStart = null;
                if (leaver.MembershipEnd == default(DateTime))
                    leaver.MembershipEnd = null;
                if (leaver.LastDDMonth == null)
                    leaver.LastDDMonth = "NA";
                Leavers.Add(leaver);
            }
        }

        public void LoadOptions()
        {
            IncResignDate = Properties.Settings.Default.IncResignDate;
            IncLeavingDate = Properties.Settings.Default.IncLeavingDate;
            IncMembershipNumberLeavers = Properties.Settings.Default.IncMembershipNumberLeavers;
            IncMemberNameLeavers = Properties.Settings.Default.IncMemberNameLeavers;
            IncCategoryNameLeavers = Properties.Settings.Default.IncCategoryNameLeavers;
            IncReason = Properties.Settings.Default.IncReason;
            IncNotes = Properties.Settings.Default.IncNotes;
            IncLinkedMembers = Properties.Settings.Default.IncLinkedMembers;
            IncMembershipStart = Properties.Settings.Default.IncMembershipStart;
            IncMembershipEnd = Properties.Settings.Default.IncMembershipEnd;
            IncContractPeriodLeavers = Properties.Settings.Default.IncContractPeriodLeavers;
            IncLastDDMonth = Properties.Settings.Default.IncLastDDMonth;
            if (Properties.Settings.Default.StartDateOptionLeavers == default(DateTime) && Properties.Settings.Default.EndDateOptionLeavers == default(DateTime))
            {
                Properties.Settings.Default.StartDateOptionLeavers = new DateTime(DateTime.Now.Date.Year, 01, 01, 0, 0, 0);
                Properties.Settings.Default.EndDateOptionLeavers = DateTime.Now;
            }
            StartDate = Properties.Settings.Default.StartDateOptionLeavers;
            EndDate = Properties.Settings.Default.EndDateOptionLeavers;
        }

        #endregion  Methods

    }
}
