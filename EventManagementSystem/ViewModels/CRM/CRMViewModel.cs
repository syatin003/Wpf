using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Properties;
using EventManagementSystem.Services;
using EventManagementSystem.Views.CRM;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.Activity;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.CRM
{
    public class CRMViewModel : ViewModelBase
    {
        #region Fields

        private readonly ICrmDataUnit _crmDataUnit;
        private ObservableCollection<EnquiryModel> _enquiries;
        private List<EnquiryModel> _allEnquiries;
        private ObservableCollection<CampaignModel> _campaigns;
        private List<ActivityModel> _allActivities;
        private ObservableCollection<ActivityModel> _activities;
        private List<FollowUpModel> _allFollowUps;
        private ObservableCollection<FollowUpModel> _followUps;
        private bool _isBusy;
        private EnquiryModel _selectedEnquiry;
        private bool _activeOnly;


        #endregion

        #region Properties

        public static List<FollowUpStatus> FollowUpStatuses { get; set; }

        public List<CampaignType> CampainTypes { get; set; }

        public List<User> Users { get; set; }

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

        public ObservableCollection<CampaignModel> Campaigns
        {
            get { return _campaigns; }
            set
            {
                if (_campaigns == value) return;
                _campaigns = value;
                RaisePropertyChanged(() => Campaigns);
            }
        }

        public ObservableCollection<ActivityModel> Activities
        {
            get { return _activities; }
            set
            {
                if (_activities == value) return;
                _activities = value;
                RaisePropertyChanged(() => Activities);
            }
        }

        public ObservableCollection<FollowUpModel> FollowUps
        {
            get { return _followUps; }
            set
            {
                if (_followUps == value) return;
                _followUps = value;
                RaisePropertyChanged(() => FollowUps);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public bool ActiveOnly
        {
            get { return _activeOnly; }
            set
            {
                _activeOnly = value;
                RaisePropertyChanged(() => ActiveOnly);
            }
        }

        public EnquiryModel SelectedEnquiry
        {
            get
            {
                return _selectedEnquiry;
            }
            set
            {
                if (_selectedEnquiry == value) return;
                _selectedEnquiry = value;
                RaisePropertyChanged(() => SelectedEnquiry);
            }
        }

        public RelayCommand AddEnquiryCommand { get; private set; }

        public RelayCommand AddActivityCommand { get; private set; }

        public RelayCommand AddFollowUpCommand { get; private set; }

        public RelayCommand AddCampaignCommand { get; private set; }

        public RelayCommand AllEnquiriesCommand { get; private set; }
        public RelayCommand MyEnquiriesCommand { get; private set; }

        public RelayCommand AllActivitiesCommand { get; private set; }
        public RelayCommand MyActivitiesCommand { get; private set; }

        public RelayCommand AllFollowUpsCommand { get; private set; }
        public RelayCommand MyFollowUpsCommand { get; private set; }

        public RelayCommand<EnquiryModel> EditEnquiryCommand { get; private set; }
        public RelayCommand<ActivityModel> EditActivityCommand { get; private set; }
        public RelayCommand<FollowUpModel> EditFollowUpCommand { get; private set; }
        public RelayCommand<CampaignModel> EditCampaignCommand { get; private set; }

        public RelayCommand RefreshCRMCommand { get; private set; }

        public RelayCommand AddToDoCommand { get; private set; }

        #endregion

        #region Constructor

        public CRMViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            AddEnquiryCommand = new RelayCommand(AddEnquiryCommandExecuted);
            AddActivityCommand = new RelayCommand(AddActivityCommandExecuted, AddActivityCommandCanExecute);
            AddFollowUpCommand = new RelayCommand(AddFollowUpCommandExecuted, AddFollowUpCommandCanExecute);
            AddCampaignCommand = new RelayCommand(AddCampaignCommandExecuted);

            AllEnquiriesCommand = new RelayCommand(SeeAllEnquiriesCommandExecute, SeeAllEnquiriesCommandCanExecute);
            MyEnquiriesCommand = new RelayCommand(SeeMyEnquiriesCommandExecute);

            AllActivitiesCommand = new RelayCommand(SeeAllActivitiesCommandExecute, SeeAllActivitiesCommandCanExecute);
            MyActivitiesCommand = new RelayCommand(SeeMyActivitiesCommandExecute);

            AllFollowUpsCommand = new RelayCommand(SeeAllFollowUpsCommandExecute, SeeAllFollowUpsCommandCanExecute);
            MyFollowUpsCommand = new RelayCommand(SeeMyFollowUpsCommandExecute);

            EditEnquiryCommand = new RelayCommand<EnquiryModel>(EditEnquiryCommandExecute);
            EditActivityCommand = new RelayCommand<ActivityModel>(EditActivityCommandExecute);
            EditFollowUpCommand = new RelayCommand<FollowUpModel>(EditFollowUpCommandExecute);
            EditCampaignCommand = new RelayCommand<CampaignModel>(EditCampaignCommandExecute);

            RefreshCRMCommand = new RelayCommand(RefreshCRMCommandExecute);

            AddToDoCommand = new RelayCommand(AddToDoCommandExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            Users = await _crmDataUnit.UsersRepository.GetUsersAsync();

            FollowUpStatuses = await _crmDataUnit.FollowUpStatusesRepository.GetAllAsync();

            CampainTypes = await _crmDataUnit.CampaignTypesRepository.GetAllAsync();

            var enquiries = await _crmDataUnit.EnquiriesRepository.GetLightEnquiriesAsync();
            _allEnquiries = new List<EnquiryModel>(enquiries.Select(x => new EnquiryModel(x)));
            Enquiries = new ObservableCollection<EnquiryModel>(_allEnquiries);

            var correspondence = await _crmDataUnit.CorresponcencesRepository.GetAllAsync(x => x.OwnerType == "Enquiry");
            _allEnquiries.ForEach(x => x.Correspondences = new ObservableCollection<CorrespondenceModel>(
                correspondence.Where(y => y.OwnerID == x.Enquiry.ID).OrderBy(y => y.Date).Select(y => new CorrespondenceModel(y))));

            var documents = await _crmDataUnit.DocumentsRepository.GetAllAsync(x => x.IsCommon);

            foreach (var enquiryModel in _allEnquiries)
            {
                foreach (var enquiryCorresponcence in enquiryModel.Correspondences)
                {
                    foreach (var cd in enquiryCorresponcence.Correspondence.CorrespondenceDocuments)
                    {
                        enquiryCorresponcence.Documents.Add(documents.First(x => x.ID == cd.DocumentID));
                    }
                }
            }


            var followUps = await _crmDataUnit.FollowUpsRepository.GetAllAsync();
            _allFollowUps = new List<FollowUpModel>(followUps.Select(x => new FollowUpModel(x)));
            SortFollowUpsByPriority();
            FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps);

            var activities = await _crmDataUnit.ActivitiesRepository.GetAllAsync();
            _allActivities = new List<ActivityModel>(activities.Select(x => new ActivityModel(x)));
            Activities = new ObservableCollection<ActivityModel>(_allActivities);

            var campaigns = await _crmDataUnit.CampaignsRepository.GetAllAsync();
            Campaigns = new ObservableCollection<CampaignModel>(campaigns.OrderByDescending(x => x.EndDate).Select(x => new CampaignModel(x)));

            IsBusy = false;
        }

        private void ProcessUpdates(EnquiryModel model, IEnumerable<string> diff)
        {
            diff.ForEach(difference =>
            {
                var update = new EnquiryUpdate()
                {
                    ID = Guid.NewGuid(),
                    Date = DateTime.Now,
                    EnquiryID = model.Enquiry.ID,
                    UserID = AccessService.Current.User.ID,
                    Message = difference
                };

                model.EnquiryUpdates.Add(update);
                _crmDataUnit.EnquiryUpdatesRepository.Add(update);
            });

            model.EnquiryUpdates = new ObservableCollection<EnquiryUpdate>(model.EnquiryUpdates.OrderByDescending(x => x.Date));

            _crmDataUnit.SaveChanges();
        }

        private void SetFollowUpPriority(FollowUpModel followUp)
        {
            var statuses = FollowUpStatuses.OrderByDescending(x => x.NumberOfDays);
            foreach (var status in statuses)
            {
                if ((followUp.DateDue.Date - DateTime.Today).TotalDays >= status.NumberOfDays)
                {
                    followUp.Priority = status.Priority;
                    break;
                }
            }
            if (followUp.Priority == 0)
                followUp.Priority = statuses.Last().Priority;
        }

        private void SortFollowUpsByPriority()
        {
            foreach (var followUp in _allFollowUps)
            {
                SetFollowUpPriority(followUp);
            }
        }

        #endregion

        #region Commands

        private void AddEnquiryCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new NewEnquiryView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult == true)
            {
                _allEnquiries.Add(window.ViewModel.Enquiry);
                Enquiries = new ObservableCollection<EnquiryModel>(_allEnquiries.OrderBy(x => x.CreationDate));

                if (window.ViewModel.Enquiry.Activities.Any())
                {
                    foreach (var activity in window.ViewModel.Enquiry.Activities)
                    {
                        _allActivities.Add(activity);
                    }
                    Activities = new ObservableCollection<ActivityModel>(_allActivities);
                }

                if (window.ViewModel.Enquiry.FollowUps.Any())
                {
                    foreach (var followUp in window.ViewModel.Enquiry.FollowUps)
                    {
                        SetFollowUpPriority(followUp);
                        _allFollowUps.Add(followUp);
                    }
                    FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps);
                }
            }
        }

        private void AddActivityCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddActivityView(_allEnquiries, null);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult == true)
            {
                _allActivities.Add(window.ViewModel.Activity);
                Activities = new ObservableCollection<ActivityModel>(_allActivities);

                if (window.ViewModel.Activity.HasFollowUp)
                    FollowUps.Add(window.ViewModel.Activity.FollowUp);

                var enquiry =
                _allEnquiries.First(x => x.Enquiry.ID == window.ViewModel.Activity.Activity.EnquiryID);
                enquiry.Activities.Add(window.ViewModel.Activity);

            }
        }

        private bool AddActivityCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Resources.PERMISSION_ADD_ACTIVITY_ALLOWED);
        }

        private void AddFollowUpCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddFollowUpView(_allEnquiries);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult == true)
            {
                SetFollowUpPriority(window.ViewModel.FollowUp);
                _allFollowUps.Add(window.ViewModel.FollowUp);
                FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps);
                var enquiry =
                    _allEnquiries.First(x => x.Enquiry.ID == window.ViewModel.FollowUp.FollowUp.EnquiryID);
                enquiry.FollowUps.Add(window.ViewModel.FollowUp);
            }
        }

        private bool AddFollowUpCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Resources.PERMISSION_ADD_FOLLOWUP_ALLOWED);
        }

        private void AddCampaignCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddCampaignView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult == true)
            {
                Campaigns.Add(window.ViewModel.Campaign);
            }
        }

        private void SeeAllEnquiriesCommandExecute()
        {
            Enquiries = ActiveOnly ? new ObservableCollection<EnquiryModel>(_allEnquiries.Where(x => x.EnquiryStatus.DefinedAsActive)) : new ObservableCollection<EnquiryModel>(_allEnquiries);
        }

        private bool SeeAllEnquiriesCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Resources.PERMISSION_SEE_ALL_ENQUIRIES_ALLOWED);
        }

        private void SeeMyEnquiriesCommandExecute()
        {
            if (ActiveOnly)
                Enquiries = new ObservableCollection<EnquiryModel>(_allEnquiries.Where(x => x.AssignedToUser.ID == AccessService.Current.User.ID && x.EnquiryStatus.DefinedAsActive));
            else
            {
                Enquiries =
                    new ObservableCollection<EnquiryModel>(
                        _allEnquiries.Where(x => x.AssignedToUser.ID == AccessService.Current.User.ID));
            }
        }

        private void SeeAllActivitiesCommandExecute()
        {
            Activities = new ObservableCollection<ActivityModel>(_allActivities);
        }

        private bool SeeAllActivitiesCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Resources.PERMISSION_SEE_ALL_ACTIVITIES_ALLOWED);
        }

        private void SeeMyActivitiesCommandExecute()
        {
            Activities = new ObservableCollection<ActivityModel>(_allActivities.Where(x => x.Assignee.ID == AccessService.Current.User.ID));
        }

        private void SeeAllFollowUpsCommandExecute()
        {
            FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps.OrderBy(x => x.Priority));
        }

        private bool SeeAllFollowUpsCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Resources.PERMISSION_SEE_ALL_FOLLOWUPS_ALLOWED);
        }

        private void SeeMyFollowUpsCommandExecute()
        {
            FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps.Where(x => x.AssignedToUser.ID == AccessService.Current.User.ID).OrderBy(x => x.Priority).ThenByDescending(x => x.DateDue));
        }

        private void EditEnquiryCommandExecute(EnquiryModel item)
        {

            RaisePropertyChanged("DisableParentWindow");
            var currentEnquiry = item.Clone();
            item.Refresh();
            var window = new NewEnquiryView(item);
            window.ShowDialog();
            RaisePropertyChanged("EnableParentWindow");
            if (window.DialogResult == null || window.DialogResult != true)
            {
                item.Refresh();
                return;
            }
            item.Refresh();
            var viewModel = window.DataContext as NewEnquiryViewModel;
            bool activityChanged = viewModel.ActivityChanged;
            bool followUpsChanged = viewModel.FoolowUpsChanged;

            //  var diff = LoggingService.FindDifference(currentEnquiry, item, out activityChanged, out followUpsChanged);
            //  ProcessUpdates(item, diff);

            if (activityChanged)
            {
                _allActivities.RemoveAll(x => x.Activity.EnquiryID == item.Enquiry.ID);

                foreach (var activity in window.ViewModel.Enquiry.Activities)
                {
                    _allActivities.Add(activity);
                }
                Activities = new ObservableCollection<ActivityModel>(_allActivities);
            }
            else
            {
                Activities.ForEach(x => x.Refresh());
            }
            if (followUpsChanged)
            {
                _allFollowUps.RemoveAll(
                    x => x.FollowUp.EnquiryID != null && (Guid)x.FollowUp.EnquiryID == item.Enquiry.ID);

                _allFollowUps.RemoveAll(x => x.TakenByUser == null);

                foreach (var followUp in window.ViewModel.Enquiry.FollowUps)
                {
                    SetFollowUpPriority(followUp);
                    _allFollowUps.Add(followUp);
                }

                FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps);
            }
            else
            {
                FollowUps.ForEach(x => x.Refresh());
            }
        }

        public async void ReloadFollowUps()
        {
            _crmDataUnit.FollowUpsRepository.Refresh();

            var followUps = await _crmDataUnit.FollowUpsRepository.GetAllAsync().ConfigureAwait(false);
            _allFollowUps = new List<FollowUpModel>(followUps.Select(x => new FollowUpModel(x)));
            SortFollowUpsByPriority();
            FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps);
        }

        public async void ReloadFollowUpsAndEnquiries()
        {
            _crmDataUnit.FollowUpsRepository.Refresh();

            var followUps = await _crmDataUnit.FollowUpsRepository.GetAllAsync().ConfigureAwait(false);
            _allFollowUps = new List<FollowUpModel>(followUps.Select(x => new FollowUpModel(x)));
            SortFollowUpsByPriority();
            FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps);

            _crmDataUnit.EnquiriesRepository.Refresh();

            var enquiries = await _crmDataUnit.EnquiriesRepository.GetLightEnquiriesAsync().ConfigureAwait(false);
            _allEnquiries = new List<EnquiryModel>(enquiries.Select(x => new EnquiryModel(x)));
            Enquiries = new ObservableCollection<EnquiryModel>(_allEnquiries);
        }

        public async void ReloadEnquiries()
        {
            _crmDataUnit.EnquiriesRepository.Refresh();

            var enquiries = await _crmDataUnit.EnquiriesRepository.GetLightEnquiriesAsync().ConfigureAwait(false);
            _allEnquiries = new List<EnquiryModel>(enquiries.Select(x => new EnquiryModel(x)));
            Enquiries = new ObservableCollection<EnquiryModel>(_allEnquiries);
        }

        private void EditActivityCommandExecute(ActivityModel item)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddActivityView(_allEnquiries, item);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            item.Refresh();

            if (window.DialogResult == null || window.DialogResult != true) return;

            ReloadEnquiries();
        }

        private void EditFollowUpCommandExecute(FollowUpModel item)
        {
            RaisePropertyChanged("DisableParentWindow");

            AddFollowUpView window;

            if (item.IsToDo)
            {
                window = new AddFollowUpView(item);
                window.ShowDialog();
            }
            else
            {
                window = new AddFollowUpView(_allEnquiries, item);
                window.ShowDialog();
            }

            RaisePropertyChanged("EnableParentWindow");

            item.Refresh();

            if (window.DialogResult == null || window.DialogResult != true) return;

            ReloadEnquiries();
        }

        private void EditCampaignCommandExecute(CampaignModel item)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddCampaignView(item);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            item.Refresh();
        }

        private void RefreshCRMCommandExecute()
        {
            _crmDataUnit.CampaignsRepository.Refresh();
            _crmDataUnit.EnquiriesRepository.Refresh();
            _crmDataUnit.FollowUpsRepository.Refresh();
            _crmDataUnit.ActivitiesRepository.Refresh();

            LoadData();
        }

        private void AddToDoCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddFollowUpView();
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult == true)
            {
                SetFollowUpPriority(window.ViewModel.FollowUp);
                _allFollowUps.Add(window.ViewModel.FollowUp);
                FollowUps = new ObservableCollection<FollowUpModel>(_allFollowUps);
            }
        }

        #endregion
    }
}
