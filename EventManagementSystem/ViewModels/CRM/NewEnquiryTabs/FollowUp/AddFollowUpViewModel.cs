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
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.Views.CRM;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ObjectBuilder2;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Properties;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.FollowUp
{
    public class AddFollowUpViewModel : ViewModelBase
    {
        #region Fields

        private readonly ICrmDataUnit _crmDataUnit;
        private readonly EnquiryModel _enquiry;
        private bool _isBusy;
        private FollowUpModel _followUp;
        private bool _isEditMode;
        private List<EnquiryModel> _enquiries;
        private EnquiryModel _selectedEnquiry;
        private bool _isToDo = false;
        private readonly bool _addToActivity;
        private ObservableCollection<User> _users;
        private EnquiryModel _originalEnquiry;

        private User _takenByUser;
        private User _assignedToUser;
        #endregion

        #region Properties

        public bool ActivityChanged, FoolowUpsChanged;

        public bool CanEditEveryoneFollowUps { get; private set; }
        public bool CanEditOwnFollowUps { get; private set; }
        public bool IsFromCRM { get; private set; }
        
        public User TakenByUser
        {
            get
            {
                return _takenByUser;
            }
            set
            {
                _takenByUser = value;
                RaisePropertyChanged(() => TakenByUser);
            }
        }

        public User AssignedToUser
        {
            get
            {
                return _assignedToUser;
            }
            set
            {
                _assignedToUser = value;
                RaisePropertyChanged(() => AssignedToUser);
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

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

        public FollowUpModel FollowUp
        {
            get { return _followUp; }
            set
            {
                if (_followUp == value) return;
                _followUp = value;
                RaisePropertyChanged(() => FollowUp);
            }
        }

        public List<EnquiryModel> Enquiries
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
            }
        }

        public bool AreEnquiriesVisible
        {
            get { return Enquiries != null; }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                if (_users == value) return;
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand OpenEnquiryCommand { get; private set; }

        #endregion

        #region Constructor

        public AddFollowUpViewModel(FollowUpModel followUpModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
            IsFromCRM = false;
            _isToDo = true;

            if (followUpModel != null)
            {
                ProcessFollowUp(followUpModel);
            }
            else
            {
                FollowUp = GetFollowUpWithoutEnquiry();
                FollowUp.PropertyChanged += FollowUpOnPropertyChanged;
            }
        }

        public AddFollowUpViewModel(EnquiryModel enquiryModel, bool addToActivity, FollowUpModel followUpModel)
        {
            _enquiry = enquiryModel;

            _addToActivity = addToActivity;
            IsFromCRM = false;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessFollowUp(followUpModel);
        }

        public AddFollowUpViewModel(IEnumerable<EnquiryModel> enquiries, FollowUpModel followUpModel)
        {
            Enquiries = enquiries.OrderBy(x => x.EventType.Name).ThenByDescending(x => x.CreationDate).ToList();

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            CanEditEveryoneFollowUps = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_FOLLOWUP_ALLOWED);
            CanEditOwnFollowUps = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_FOLLOWUP_ALLOWED);
            IsFromCRM = true;

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            OpenEnquiryCommand = new RelayCommand(OpenEnquiryCommandExecute, OpenEnquiryCommandCanExecute);

            if (followUpModel != null)
            {
                ProcessFollowUp(followUpModel);
                SelectedEnquiry = Enquiries.FirstOrDefault(x => x.Enquiry == followUpModel.FollowUp.Enquiry);
                _originalEnquiry = SelectedEnquiry.Clone();
            }
            else
            {
                FollowUp = GetFollowUpWithoutEnquiry();
                FollowUp.PropertyChanged += FollowUpOnPropertyChanged;
            }

        }

        #endregion

        #region Methods

        private void ProcessFollowUp(FollowUpModel followUpModel)
        {
            _isEditMode = (followUpModel != null);

            if (_isEditMode)
            {
                TakenByUser = followUpModel.TakenByUser;
                AssignedToUser = followUpModel.AssignedToUser;
            }
            FollowUp = (_isEditMode) ? followUpModel : GetFollowUp();
            FollowUp.PropertyChanged += FollowUpOnPropertyChanged;
        }

        private FollowUpModel GetFollowUp()
        {
            var followUpModel = new FollowUpModel(new Data.Model.FollowUp()
            {
                ID = Guid.NewGuid(),
                EnquiryID = _enquiry.Enquiry.ID,
                DateDue = DateTime.Now
            });

            //  followUpModel.FollowUp.Enquiry = _enquiry.Enquiry;

            return followUpModel;
        }

        private FollowUpModel GetFollowUpWithoutEnquiry()
        {
            var followUpModel = new FollowUpModel(new Data.Model.FollowUp()
            {
                ID = Guid.NewGuid(),
                DateDue = DateTime.Now
            });

            return followUpModel;
        }

        private void FollowUpOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var users = await _crmDataUnit.UsersRepository.GetUsersAsync();
            Users = new ObservableCollection<User>(users);

            OnLoadCurrentUser();

            if (_isEditMode)
            {
                var desiredFollowUp = await _crmDataUnit.FollowUpsRepository.GetUpdatedFollowUp(_followUp.FollowUp.ID);
                // Check if we have new changes
                if (desiredFollowUp != null && desiredFollowUp.LastEditDate != null && _followUp.LoadedTime < desiredFollowUp.LastEditDate)
                {
                    FollowUp = new FollowUpModel(desiredFollowUp);

                    AssignedToUser = desiredFollowUp.User1;
                    // _forceRefreshActivityData = true;
                }
            }

            IsBusy = false;
        }

        private void OnLoadCurrentUser()
        {
            if (_isEditMode) return;

            var user = Users.FirstOrDefault(x => x.ID == AccessService.Current.User.ID);
            TakenByUser = user;
            //  Application.Current.Dispatcher.BeginInvoke(new Action(() => FollowUp.TakenByUser = user));
        }

        #endregion

        #region Commands

        private void OpenEnquiryCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new NewEnquiryView(new EnquiryModel(FollowUp.FollowUp.Enquiry));
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private bool OpenEnquiryCommandCanExecute()
        {
            return _isEditMode;
        }

        private async void SubmitCommandExecuted()
        {
            if (!_isEditMode)
            {
                FollowUp.TakenByUser = TakenByUser;
                FollowUp.AssignedToUser = AssignedToUser;
                if (AreEnquiriesVisible)
                {
                    FollowUp.FollowUp.Enquiry = SelectedEnquiry.Enquiry;

                }
                else if (!_isToDo)
                {
                    _enquiry.FollowUps.Add(FollowUp);
                }

                _crmDataUnit.FollowUpsRepository.Add(FollowUp.FollowUp);

                if (!_isToDo)
                {
                    var primaryContact = FollowUp.FollowUp.Enquiry != null ? FollowUp.FollowUp.Enquiry.Contact == null ? String.Empty : "Primary Contact: " + FollowUp.FollowUp.Enquiry.Contact.FirstName + " "
                        + FollowUp.FollowUp.Enquiry.Contact.LastName : String.Empty;

                    var msg = "Follow-Up" + "\n" + "Created by " + FollowUp.TakenByUser.FirstName + " " +
                              FollowUp.TakenByUser.LastName + " at " + DateTime.Now + "\n" +
                              "Event Name: " + FollowUp.EnquiryName + "\n" + primaryContact + "\n" + FollowUp.WhatToDo;
                    var email = new CorrespondenceModel(new Corresponcence()
                    {
                        ID = Guid.NewGuid(),
                        Date = DateTime.Now,
                        FromAddress = FollowUp.TakenByUser.EmailAddress,
                        ToAddress = FollowUp.AssignedToUser.EmailAddress,
                        Subject = "Follow-Up",
                        Message = msg,
                    });

                    await EmailService.SendEmail(email);
                    if (AreEnquiriesVisible)
                    {
                        _originalEnquiry = SelectedEnquiry.Clone();
                        _selectedEnquiry.FollowUps.Add(FollowUp);
                        var diff = LoggingService.FindDifference(_originalEnquiry, SelectedEnquiry, out ActivityChanged, out FoolowUpsChanged);
                        if (!SelectedEnquiry.EnquiryUpdates.Any())
                        {
                            var updates = await _crmDataUnit.EnquiryUpdatesRepository.GetAllAsync(x => x.EnquiryID == SelectedEnquiry.Enquiry.ID);
                            SelectedEnquiry.EnquiryUpdates = new ObservableCollection<EnquiryUpdate>(updates.OrderByDescending(x => x.Date));
                        }

                        ProcessUpdates(_selectedEnquiry, diff);
                        if (!_addToActivity)
                            await _crmDataUnit.SaveChanges();
                    }

                }
            }
            else
            {
                _followUp.AssignedToUser = AssignedToUser;
                _followUp.FollowUp.UserDueToDoID = AssignedToUser.ID;
                _followUp.FollowUp.LastEditDate = DateTime.Now;
                if (AreEnquiriesVisible)
                {
                    if (_originalEnquiry.Enquiry.ID != _selectedEnquiry.Enquiry.ID)
                    {

                        _originalEnquiry = _selectedEnquiry.Clone();
                    }

                    FollowUp.FollowUp.Enquiry = SelectedEnquiry.Enquiry;
                    SelectedEnquiry.FollowUps.Where(x => x.FollowUp == _followUp.FollowUp).FirstOrDefault().AssignedToUser = AssignedToUser;
                    var diff = LoggingService.FindDifference(_originalEnquiry, SelectedEnquiry, out ActivityChanged, out FoolowUpsChanged);
                    if (!SelectedEnquiry.EnquiryUpdates.Any())
                    {
                        var updates = await _crmDataUnit.EnquiryUpdatesRepository.GetAllAsync(x => x.EnquiryID == SelectedEnquiry.Enquiry.ID);
                        SelectedEnquiry.EnquiryUpdates = new ObservableCollection<EnquiryUpdate>(updates.OrderByDescending(x => x.Date));
                    }
                    ProcessUpdates(_selectedEnquiry, diff);

                    if (!_addToActivity)
                        await _crmDataUnit.SaveChanges();
                }
                else
                    _crmDataUnit.FollowUpsRepository.SetEntityModified(_followUp.FollowUp);
                FollowUp.Refresh();
            }
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
            //    _crmDataUnit.SaveChanges();
        }
        private bool SubmitCommandCanExecute()
        {
            return !FollowUp.HasErrors && (!AreEnquiriesVisible || SelectedEnquiry != null) && (AssignedToUser != null);
        }

        private void CancelCommandExecuted()
        {
            _crmDataUnit.RevertChanges();

            if (_isEditMode)
            {
                FollowUp.Refresh();
            }
            else
            {
                FollowUp = null;
            }
        }

        #endregion
    }
}
