using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.Views.CRM;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Properties;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.Activity
{
    public class AddActivityViewModel : ViewModelBase
    {
        #region Fields

        private readonly ICrmDataUnit _crmDataUnit;
        private readonly EnquiryModel _enquiry;
        private bool _isBusy;
        private ActivityModel _activity;
        private ActivityModel _originalActivity;
        private bool _isEditMode;
        private ObservableCollection<ActivityType> _activityTypes;
        private ObservableCollection<EnquiryModel> _enquiries;
        private EnquiryModel _selectedEnquiry;

        private User _assignee;
        private ActivityType _activityType;

        #endregion

        #region Properties

        public bool CanEditEveryoneActivities { get; private set; }
        public bool CanEditOwnActivities { get; private set; }
        public bool IsFromCRM { get; private set; }
        public bool CanDeleteFollowUp { get; private set; }
        public bool CanEditEveryoneFollowUps { get; private set; }
        public bool CanEditOwnFollowUps { get; private set; }

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

        public ActivityType ActivityType
        {
            get { return _activityType; }
            set
            {
                _activityType = value;
                RaisePropertyChanged(() => ActivityType);
                SubmitCommand.RaiseCanExecuteChanged();
                AddFollowUpCommand.RaiseCanExecuteChanged();
            }
        }

        public User Assignee
        {
            get { return _assignee; }
            set
            {
                _assignee = value;
                RaisePropertyChanged(() => Assignee);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public ActivityModel Activity
        {
            get { return _activity; }
            set
            {
                if (_activity == value) return;
                _activity = value;
                RaisePropertyChanged(() => Activity);
            }
        }

        public ObservableCollection<EnquiryModel> Enquiries
        {
            get { return _enquiries; }
            set
            {
                if (_enquiries == value) return;
                _enquiries = value;
                RaisePropertyChanged(() => Enquiries);
            }
        }

        public EnquiryModel SelectedEnquiry
        {
            get { return _selectedEnquiry; }
            set
            {
                if (_selectedEnquiry == value) return;
                _selectedEnquiry = value;
                RaisePropertyChanged(() => SelectedEnquiry);
                SubmitCommand.RaiseCanExecuteChanged();
                AddFollowUpCommand.RaiseCanExecuteChanged();
            }
        }

        public bool AreEnquiriesVisible
        {
            get { return Enquiries != null; }
        }

        public ObservableCollection<ActivityType> ActivityTypes
        {
            get { return _activityTypes; }
            set
            {
                if (_activityTypes == value) return;
                _activityTypes = value;
                RaisePropertyChanged(() => ActivityTypes);
            }
        }

        public bool HasActivityFollowUp
        {
            get { return Activity.FollowUp == null; }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand AddFollowUpCommand { get; private set; }
        public RelayCommand<FollowUpModel> DeleteFollowUpCommand { get; private set; }
        public RelayCommand<FollowUpModel> EditFollowUpCommand { get; private set; }

        public RelayCommand OpenEnquiryCommand { get; private set; }

        #endregion

        #region Constructor

        public AddActivityViewModel(EnquiryModel enquiryModel, ActivityModel activityModel)
        {
            _enquiry = enquiryModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            IsFromCRM = false;
            CanDeleteFollowUp = AccessService.Current.UserHasPermissions(Resources.PERMISSION_DELETE_FOLLOWUP_ALLOWED);
            CanEditEveryoneFollowUps = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_FOLLOWUP_ALLOWED);
            CanEditOwnFollowUps = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_FOLLOWUP_ALLOWED);

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
            AddFollowUpCommand = new RelayCommand(AddFollowUpCommandExecuted, AddFollowUpCommandCanExecute);
            EditFollowUpCommand = new RelayCommand<FollowUpModel>(EditFollowUpCommandExecuted);
            DeleteFollowUpCommand = new RelayCommand<FollowUpModel>(DeleteFollowUpCommandExecuted);

            ProcessActivity(activityModel);
        }

        public AddActivityViewModel(IEnumerable<EnquiryModel> enquiries, ActivityModel activityModel)
        {
            Enquiries = new ObservableCollection<EnquiryModel>(enquiries.OrderBy(x => x.EventType.Name).ThenByDescending(x => x.CreationDate));

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();


            CanEditEveryoneActivities = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_FOLLOWUP_ALLOWED);
            CanEditOwnActivities = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_FOLLOWUP_ALLOWED);
            IsFromCRM = true;
            CanDeleteFollowUp = AccessService.Current.UserHasPermissions(Resources.PERMISSION_DELETE_FOLLOWUP_ALLOWED);
            CanEditEveryoneFollowUps = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_FOLLOWUP_ALLOWED);
            CanEditOwnFollowUps = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_FOLLOWUP_ALLOWED);

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
            AddFollowUpCommand = new RelayCommand(AddFollowUpCommandExecuted, AddFollowUpCommandCanExecute);
            EditFollowUpCommand = new RelayCommand<FollowUpModel>(EditFollowUpCommandExecuted);
            DeleteFollowUpCommand = new RelayCommand<FollowUpModel>(DeleteFollowUpCommandExecuted);
            OpenEnquiryCommand = new RelayCommand(OpenEnquiryCommandExecute, OpenEnquiryCommandCanExecute);

            if (activityModel != null)
            {
                ProcessActivity(activityModel);
                SelectedEnquiry = Enquiries.FirstOrDefault(x => x.Enquiry == activityModel.Activity.Enquiry);
            }
            else
            {
                Activity = GetActivityWithoutEnquiry();
                Activity.PropertyChanged += ActivityOnPropertyChanged;
            }
        }

        #endregion

        #region Methods

        private void ProcessActivity(ActivityModel activityModel)
        {
            _isEditMode = (activityModel != null);

            Activity = (_isEditMode) ? activityModel : GetActivity();
            if (_isEditMode)
            {
                _originalActivity = Activity.Clone();
            }
            Activity.PropertyChanged += ActivityOnPropertyChanged;

            if (_isEditMode)
            {
                Assignee = activityModel.Assignee;
                ActivityType = activityModel.ActivityType;
            }

        }

        private ActivityModel GetActivity()
        {
            var activityModel = new ActivityModel(new Data.Model.Activity()
            {
                ID = Guid.NewGuid(),
                EnquiryID = _enquiry.Enquiry.ID,
                Date = DateTime.Now,
            });

            return activityModel;
        }

        private ActivityModel GetActivityWithoutEnquiry()
        {
            var activityModel = new ActivityModel(new Data.Model.Activity()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
            });

            return activityModel;
        }

        private void ActivityOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var types = await _crmDataUnit.ActivityTypesRepository.GetAllAsync();
            ActivityTypes = new ObservableCollection<ActivityType>(types.OrderBy(x => x.Name));

            OnLoadCurrentUser();

            if (_isEditMode)
            {
                var desiredActivity = await _crmDataUnit.ActivitiesRepository.GetUpdatedActivity(_activity.Activity.ID);

                // Check if we have new changes
                if (desiredActivity != null && desiredActivity.LastEditDate != null && _activity.LoadedTime < desiredActivity.LastEditDate)
                {
                    bool hadActivityFollowUp = Activity.HasFollowUp;
                    Activity = new ActivityModel(desiredActivity);

                    if (hadActivityFollowUp && desiredActivity.FollowUp == null)
                    {
                        Activity.FollowUp = null;
                    }
                    else if (desiredActivity.FollowUp != null)
                    {
                        Activity.FollowUp = new FollowUpModel(desiredActivity.FollowUp);
                    }

                    RaisePropertyChanged(() => HasActivityFollowUp);

                    Assignee = desiredActivity.User;
                    ActivityType = desiredActivity.ActivityType;
                }
            }

            IsBusy = false;
        }


        private async void OnLoadCurrentUser()
        {
            if (_isEditMode) return;

            var user = (await _crmDataUnit.UsersRepository.GetUsersAsync(x => x.ID == AccessService.Current.User.ID)).FirstOrDefault();
            Assignee = user;
        }

        #endregion

        #region Commands

        private void OpenEnquiryCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new NewEnquiryView(new EnquiryModel(Activity.Activity.Enquiry));
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private bool OpenEnquiryCommandCanExecute()
        {
            return _isEditMode;
        }

        private void SubmitCommandExecuted()
        {
            if (!_isEditMode)
            {
                Activity.ActivityType = ActivityType;
                Activity.Assignee = Assignee;

                if (AreEnquiriesVisible)
                    Activity.Activity.Enquiry = SelectedEnquiry.Enquiry;
                else
                {
                    _enquiry.Activities.Add(Activity);
                }

                _crmDataUnit.ActivitiesRepository.Add(Activity.Activity);
            }
            else
            {
                _activity.Activity.ActivityTypeID = ActivityType.ID;
                if (AreEnquiriesVisible)
                    Activity.Activity.Enquiry = SelectedEnquiry.Enquiry;
                _activity.Activity.LastEditDate = DateTime.Now;
                _crmDataUnit.ActivitiesRepository.SetEntityModified(Activity.Activity);
            }

            if (AreEnquiriesVisible)
                _crmDataUnit.SaveChanges();
        }

        private bool SubmitCommandCanExecute()
        {
            return !Activity.HasErrors && (!AreEnquiriesVisible || SelectedEnquiry != null);
        }

        private void CancelCommandExecuted()
        {
            _crmDataUnit.RevertChanges();

            if (_isEditMode)
            {
                if (_originalActivity.FollowUp != null && Activity.FollowUp == null)
                {
                    Activity.FollowUp = _originalActivity.FollowUp.Clone();
                    Activity.Activity.FollowUp = _originalActivity.FollowUp.FollowUp.Clone();
                    _crmDataUnit.FollowUpsRepository.Add(_originalActivity.FollowUp.FollowUp.Clone());
                    RaisePropertyChanged(() => HasActivityFollowUp);
                }
                Activity.Refresh();
            }
        }

        private void AddFollowUpCommandExecuted()
        {
            var enquiry = _enquiry ?? SelectedEnquiry;

            RaisePropertyChanged("DisableParentWindow");

            var window = new AddFollowUpView(enquiry, true);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult == true)
            {
                Activity.FollowUp = window.ViewModel.FollowUp;
                Activity.Activity.FollowUp = window.ViewModel.FollowUp.FollowUp;
            }

            RaisePropertyChanged(() => HasActivityFollowUp);
        }

        private bool AddFollowUpCommandCanExecute()
        {
            return HasActivityFollowUp && (_enquiry != null || SelectedEnquiry != null) && ActivityType != null;
        }

        private void EditFollowUpCommandExecuted(FollowUpModel item)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddFollowUpView(_enquiry, true, item);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void DeleteFollowUpCommandExecuted(FollowUpModel item)
        {
            Activity.FollowUp = null;
            _crmDataUnit.FollowUpsRepository.Delete(item.FollowUp);
            RaisePropertyChanged(() => HasActivityFollowUp);
        }

        #endregion
    }
}