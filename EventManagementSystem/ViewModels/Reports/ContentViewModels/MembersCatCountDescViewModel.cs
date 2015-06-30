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
    public class MembersCatCountDescViewModel : ViewModelBase
    {
        #region Fields

        private readonly IReportsDataUnit _reportsDataUnit;
        private bool _isBusy;
        private ObservableCollection<MemberModel> _members;
        private List<MemberModel> AllMembers { get; set; }

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

        public ObservableCollection<MemberModel> Members
        {
            get { return _members; }
            set
            {
                if (_members == value) return;
                _members = value;
                RaisePropertyChanged(() => Members);
            }
        }

        #endregion  Properties

        #region Constructor

        public MembersCatCountDescViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _reportsDataUnit = dataUnitLocator.ResolveDataUnit<IReportsDataUnit>();

        }

        #endregion  Constructor

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _reportsDataUnit.MembersRepository.Refresh();
            var members = await _reportsDataUnit.MembersRepository.GetAllAsync();
            AllMembers = new List<MemberModel>(members.Select(member => new MemberModel(member)));
            Members = new ObservableCollection<MemberModel>(AllMembers);

            IsBusy = false;
        }

        #endregion  Methods

    }
}
