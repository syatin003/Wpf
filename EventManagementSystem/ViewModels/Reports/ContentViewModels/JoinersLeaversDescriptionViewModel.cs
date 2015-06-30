using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Reports.ContentViewModels
{
    public class JoinersLeaversDescriptionViewModel : ViewModelBase
    {

        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private bool _isBusy;
        private List<MemberModel> _allMembers;
        private ObservableCollection<JoinerLeaverModel> _joinersLeavers;
        private DateTime _startDate;
        private DateTime _endDate;

        private bool _incOpening;
        private bool _incJoiners;
        private bool _incLeavers;
        private bool _incTransfersIn;
        private bool _incTransfersOut;
        private bool _incClosing;

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

        public ObservableCollection<JoinerLeaverModel> JoinersLeavers
        {
            get { return _joinersLeavers; }
            set
            {
                if (_joinersLeavers == value) return;
                _joinersLeavers = value;
                RaisePropertyChanged(() => JoinersLeavers);
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                Properties.Settings.Default.StartDateOptionJoinersLeavers = _startDate;
                RaisePropertyChanged(() => StartDate);
                UpdateJoinersLeaversData();

            }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                Properties.Settings.Default.EndDateOptionJoinersLeavers = _endDate;
                RaisePropertyChanged(() => EndDate);
                UpdateJoinersLeaversData();
            }
        }

        public bool IncOpening
        {
            get { return _incOpening; }
            set
            {
                if (_incOpening == value) return;
                _incOpening = value;
                RaisePropertyChanged(() => IncOpening);
                RaisePropertyChanged(() => IsDisplayTotal);

            }
        }

        public bool IncJoiners
        {
            get { return _incJoiners; }
            set
            {
                if (_incJoiners == value) return;
                _incJoiners = value;
                RaisePropertyChanged(() => IncJoiners);
                RaisePropertyChanged(() => IsDisplayTotal);

            }
        }

        public bool IncLeavers
        {
            get { return _incLeavers; }
            set
            {
                if (_incLeavers == value) return;
                _incLeavers = value;
                RaisePropertyChanged(() => IncLeavers);
                RaisePropertyChanged(() => IsDisplayTotal);

            }
        }

        public bool IncTransfersIn
        {
            get { return _incTransfersIn; }
            set
            {
                if (_incTransfersIn == value) return;
                _incTransfersIn = value;
                RaisePropertyChanged(() => IncTransfersIn);
                RaisePropertyChanged(() => IsDisplayTotal);

            }
        }

        public bool IncTransfersOut
        {
            get { return _incTransfersOut; }
            set
            {
                if (_incTransfersOut == value) return;
                _incTransfersOut = value;
                RaisePropertyChanged(() => IncTransfersOut);
                RaisePropertyChanged(() => IsDisplayTotal);
            }
        }

        public bool IncClosing
        {
            get { return _incClosing; }
            set
            {
                if (_incClosing == value) return;
                _incClosing = value;
                RaisePropertyChanged(() => IncClosing);
                RaisePropertyChanged(() => IsDisplayTotal);
            }
        }


        public bool IsDisplayTotal
        {
            get
            {
                if (IncOpening)
                    return true;
                if (IncJoiners)
                    return true;
                if (IncLeavers)
                    return true;
                if (IncTransfersIn)
                    return true;
                if (IncTransfersOut)
                    return true;
                if (IncClosing)
                    return true;
                return false;
            }
        }

        #endregion  Properties

        #region Constructor

        public JoinersLeaversDescriptionViewModel()
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

            UpdateJoinersLeaversData();

            IsBusy = false;
        }

        private void UpdateJoinersLeaversData()
        {
            var memberGroups = (_allMembers.GroupBy(member => member.Member.MembershipCategory));
            JoinersLeavers = new ObservableCollection<JoinerLeaverModel>();
            foreach (var memberGroup in memberGroups)
            {
                var memberByCategory = new JoinerLeaverModel
                {
                    CategoryName = memberGroup.Key.Name,
                    MemberShipGroup = memberGroup.Key.MembershipGroup.Name,
                    Opening = memberGroup.Count(member => member.StartDate < StartDate && member.RenewalDate > StartDate && member.Member.ResignDate == null),
                    Joiners = memberGroup.Count(member => member.StartDate >= StartDate && member.StartDate < EndDate),
                    Leavers = memberGroup.Count(member => member.Member.ResignDate > StartDate && member.Member.ResignDate < EndDate),
                    Closing = memberGroup.Count(member => member.StartDate < EndDate && member.RenewalDate > EndDate && (member.Member.ResignDate == null || member.Member.ResignDate > EndDate))
                };
                JoinersLeavers.Add(memberByCategory);
            }
        }

        public void LoadOptions()
        {
            IncOpening = Properties.Settings.Default.IncOpeningOption;
            IncJoiners = Properties.Settings.Default.IncJoinersOption;
            IncLeavers = Properties.Settings.Default.IncLeaversOption;
            IncTransfersIn = Properties.Settings.Default.IncTransfersInOption;
            IncTransfersOut = Properties.Settings.Default.IncTransfersOutOption;
            IncClosing = Properties.Settings.Default.IncClosingOption;
            if (Properties.Settings.Default.StartDateOptionJoinersLeavers == default(DateTime) && Properties.Settings.Default.EndDateOptionJoinersLeavers == default(DateTime))
            {
                Properties.Settings.Default.StartDateOptionJoinersLeavers = new DateTime(DateTime.Now.Date.Year, 01, 01, 0, 0, 0);
                Properties.Settings.Default.EndDateOptionJoinersLeavers = DateTime.Now;
            }
            StartDate = Properties.Settings.Default.StartDateOptionJoinersLeavers;
            EndDate = Properties.Settings.Default.EndDateOptionJoinersLeavers;
        }
        #endregion  Methods
    }
}
