using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Models.Custom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EventManagementSystem.Views.Membership;

namespace EventManagementSystem.ViewModels.Membership
{
    public class MembershipUpdatesViewModel : ViewModelBase
    {
        #region Fields

        private readonly IMembershipDataUnit _membershipModuleDataUnit;
        private bool _isBusy;
        private ObservableCollection<MembershipUpdateModel> _allMembershipUpdates;
        private DateTime _selectedDate;
        private List<MembershipUpdate> _membershipUpdatesList;
        private ObservableCollection<MembershipUpdate> _membershipUpdate;
        private bool _isDataLoadedOnce;


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

        public ObservableCollection<MembershipUpdateModel> AllMembershipUpdates
        {
            get
            {
                return _allMembershipUpdates;
            }
            set
            {
                if (_allMembershipUpdates == value) return;
                _allMembershipUpdates = value;
                RaisePropertyChanged(() => AllMembershipUpdates);
            }
        }
        public bool IsDataLoadedOnce
        {
            get { return _isDataLoadedOnce; }
            set
            {
                if (_isDataLoadedOnce == value) return;
                _isDataLoadedOnce = value;
                RaisePropertyChanged(() => IsDataLoadedOnce);
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                RaisePropertyChanged(() => SelectedDate);
                if (IsDataLoadedOnce)
                    OnSelectedDateChanged();
            }
        }
        public ObservableCollection<MembershipUpdate> MembershipUpdates
        {
            get
            {
                return _membershipUpdate;
            }
            set
            {
                if (_membershipUpdate == value) return;
                _membershipUpdate = value;
                RaisePropertyChanged(() => MembershipUpdates);
            }
        }

        public RelayCommand<MembershipUpdateModel> ShowHistoryCommand { get; private set; }

        #endregion

        #region Constructor

        public MembershipUpdatesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _membershipModuleDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();
            ShowHistoryCommand = new RelayCommand<MembershipUpdateModel>(ShowHistoryCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, 0, 0, 0);
            var enddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

            _membershipModuleDataUnit.MembershipUpdatesRepository.Refresh();

            var updates = await _membershipModuleDataUnit.MembershipUpdatesRepository.GetUpdatesByDate(startdate, enddate);
            _membershipUpdatesList = new List<MembershipUpdate>(updates.OrderByDescending(membershipUpdate => membershipUpdate.Date));
            MembershipUpdates = new ObservableCollection<MembershipUpdate>(_membershipUpdatesList.Where(x => x.Field != "Notes"));
            AllMembershipUpdates = new ObservableCollection<MembershipUpdateModel>(MembershipUpdates.Select(x => new MembershipUpdateModel(x)));
            ProcessNotesUpdates();

            RaisePropertyChanged("OnDataLoaded");

            IsDataLoadedOnce = true;
        }

        private void ProcessNotesUpdates()
        {
            var updatesgroups = _membershipUpdatesList.Where(x => x.Field == "Notes").OrderByDescending(x => x.Date).GroupBy(x => x.ItemId);

            foreach (var updatesgroup in updatesgroups)
            {
                var updateModel = new MembershipUpdateModel(updatesgroup.FirstOrDefault());
                var updateHiistList = updatesgroup.Select(membershipUpdate => new MembershipUpdatesHistoryModel()
                {
                    MembershipUpdate = membershipUpdate

                }).ToList();
                if (updateModel.MembershipUpdate != null)
                {
                    updateModel.MembershipUpdatesHistory = updateHiistList.OrderBy(membershipUpdate => membershipUpdate.MembershipUpdate.Date).ToList();
                    AllMembershipUpdates.Add(updateModel);
                }
            }
            AllMembershipUpdates = new ObservableCollection<MembershipUpdateModel>(AllMembershipUpdates.OrderByDescending(membershipUpdate => membershipUpdate.MembershipUpdate.Date));
        }

        private async void OnSelectedDateChanged()
        {
            IsBusy = true;

            var startdate = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, 0, 0, 0);
            var enddate = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, 23, 59, 59);
            var updates = await _membershipModuleDataUnit.MembershipUpdatesRepository.GetUpdatesByDate(startdate, enddate);
            _membershipUpdatesList = new List<MembershipUpdate>(updates.OrderByDescending(membershipUpdate => membershipUpdate.Date));
            MembershipUpdates = new ObservableCollection<MembershipUpdate>(_membershipUpdatesList.Where(x => x.Field != "Notes"));
            AllMembershipUpdates = new ObservableCollection<MembershipUpdateModel>(MembershipUpdates.Select(x => new MembershipUpdateModel(x)));
            ProcessNotesUpdates();

            RaisePropertyChanged("OnDataLoaded");
        }

        #endregion

        #region Commands

        private void ShowHistoryCommandExecuted(MembershipUpdateModel membershipUpdate)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new MembershipUpdatesHistoryView(membershipUpdate.MembershipUpdatesHistory);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        #endregion Commands
    }
}
