using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.Activity;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Services;
using EventManagementSystem.Properties;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs
{
    public class EnquiryActivityViewModel : ViewModelBase
    {
        #region Fields

        private EnquiryModel _enquiry;
        private ActivityModel _originalActivity;
        private readonly ICrmDataUnit _crmDataUnit;

        #endregion

        #region Properties

        public EnquiryModel Enquiry
        {
            get { return _enquiry; }
            set
            {
                if (_enquiry == value) return;
                _enquiry = value;
                RaisePropertyChanged(() => Enquiry);
            }
        }

        public bool CanDeleteActivity { get; private set; }
        public bool CanEditEveryoneActivities { get; private set; }
        public bool CanEditOwnActivities { get; private set; }

        public RelayCommand AddActivityCommand { get; private set; }
        public RelayCommand<ActivityModel> DeleteActivityCommand { get; private set; }
        public RelayCommand<ActivityModel> EditActivityCommand { get; private set; }

        #endregion

        #region Constructor

        public EnquiryActivityViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            CanDeleteActivity = AccessService.Current.UserHasPermissions(Resources.PERMISSION_DELETE_ACTIVITY_ALLOWED);
            CanEditEveryoneActivities = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_ACTIVITY_ALLOWED);
            CanEditOwnActivities = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_ACTIVITY_ALLOWED);

            AddActivityCommand = new RelayCommand(AddActivityCommandExecuted);
            DeleteActivityCommand = new RelayCommand<ActivityModel>(DeleteActivityCommandExecuted);
            EditActivityCommand = new RelayCommand<ActivityModel>(EditActivityCommandExecuted);
        }

        #endregion

        #region Commands

        private void AddActivityCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddActivityView(Enquiry);
            window.ShowDialog();
            if (window.DialogResult != null && window.DialogResult.Value)
            {
                Enquiry.Activities = new ObservableCollection<ActivityModel>(Enquiry.Activities.OrderByDescending(x => x.Date));
            }
            RaisePropertyChanged("EnableParentWindow");
        }

        private void EditActivityCommandExecuted(ActivityModel item)
        {
            RaisePropertyChanged("DisableParentWindow");
            _originalActivity = item.Clone();
            var window = new AddActivityView(Enquiry, item);
            window.ShowDialog();
            if (window.DialogResult != null && !window.DialogResult.Value)
            {
                item.ActivityType.ID = _originalActivity.ActivityType.ID;
                item.Details = _originalActivity.Details;
                item.Direction = _originalActivity.Direction;
                item.Length = _originalActivity.Length;
                item.Date = _originalActivity.Date;
                //item.FollowUp = _originalActivity.FollowUp;
                
                item.Refresh();
            }
            else
            {
                Enquiry.Activities = new ObservableCollection<ActivityModel>(Enquiry.Activities.OrderByDescending(x => x.Date));
            }
            RaisePropertyChanged("EnableParentWindow");
        }

        private void DeleteActivityCommandExecuted(ActivityModel item)
        {
            Enquiry.Activities.Remove(item);
            _crmDataUnit.ActivitiesRepository.Delete(item.Activity);
        }

        #endregion
    }
}
