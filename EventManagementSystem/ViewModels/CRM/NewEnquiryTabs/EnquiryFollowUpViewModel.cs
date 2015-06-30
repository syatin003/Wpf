using System.Collections.Generic;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Serialization;
using EventManagementSystem.Services;
using EventManagementSystem.Properties;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs
{
    public class EnquiryFollowUpViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private EnquiryModel _enquiry;
        private FollowUpModel _originalFollowUp;

        private readonly ICrmDataUnit _crmDataUnit;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

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

        public bool CanDeleteFollowUp { get; private set; }
        public bool CanEditEveryoneFollowUps { get; private set; }
        public bool CanEditOwnFollowUps { get; private set; }

        public RelayCommand AddFollowUpCommand { get; private set; }
        public RelayCommand<FollowUpModel> DeleteFollowUpCommand { get; private set; }
        public RelayCommand<FollowUpModel> EditFollowUpCommand { get; private set; }

        public static List<FollowUpStatus> FollowUpStatuses { get; set; }

        #endregion

        #region Constructor

        public EnquiryFollowUpViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            CanDeleteFollowUp = AccessService.Current.UserHasPermissions(Resources.PERMISSION_DELETE_FOLLOWUP_ALLOWED);
            CanEditEveryoneFollowUps = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_EVERYONE_FOLLOWUP_ALLOWED);
            CanEditOwnFollowUps = AccessService.Current.UserHasPermissions(Resources.PERMISSION_EDIT_OWN_FOLLOWUP_ALLOWED);


            AddFollowUpCommand = new RelayCommand(AddFollowUpCommandExecuted);
            DeleteFollowUpCommand = new RelayCommand<FollowUpModel>(DeleteFollowUpCommandExecuted);
            EditFollowUpCommand = new RelayCommand<FollowUpModel>(EditFollowUpCommandExecuted);
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            IsBusy = true;

            //FollowUpStatuses = await _crmDataUnit.FollowUpStatusesRepository.GetAllAsync();

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void AddFollowUpCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddFollowUpView(Enquiry, false);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private void EditFollowUpCommandExecuted(FollowUpModel item)
        {
            RaisePropertyChanged("DisableParentWindow");
            _originalFollowUp = item.Clone();
            var window = new AddFollowUpView(Enquiry, false, item);
            window.ShowDialog();
            if (window.DialogResult != null && !window.DialogResult.Value)
            {
                item.FollowUp.DateDue = _originalFollowUp.FollowUp.DateDue;
                item.FollowUp.WhatToDo = _originalFollowUp.FollowUp.WhatToDo;
                item.FollowUp.UserDueToDoID = _originalFollowUp.FollowUp.UserDueToDoID;
                item.Refresh();
            }
            RaisePropertyChanged("EnableParentWindow");
        }

        private void DeleteFollowUpCommandExecuted(FollowUpModel item)
        {
            if (Enquiry.Activities.Any(x => x.FollowUp == item))
            {
                Enquiry.Activities.First(x => x.FollowUp == item).FollowUp = null;
            }

            Enquiry.FollowUps.Remove(item);
            _crmDataUnit.FollowUpsRepository.Delete(item.FollowUp);
        }

        #endregion
    }
}
