using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Membership.MembershipTabs
{
    public class MemberUpdateDetailsViewModel : ViewModelBase
    {
        #region Fields

        private MemberModel _member;
        private bool _isBusy;
        private readonly IMembershipDataUnit _membershipDataUnit;

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

        #endregion

        #region Constructor

        public MemberUpdateDetailsViewModel(MemberModel member)
        {
            Member = member;
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            //if (!Member.MembershipUpdates.Any())
            //{
                _membershipDataUnit.MembershipUpdatesRepository.Refresh();
                var updates = await _membershipDataUnit.MembershipUpdatesRepository.GetAllAsync(x => x.MemberID == Member.Member.ID);
                Member.MembershipUpdates = new ObservableCollection<MembershipUpdate>(updates.OrderByDescending(x => x.Date));
            //}
            IsBusy = false;
        }

        #endregion
    }
}
