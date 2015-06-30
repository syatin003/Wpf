using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Views.Admin.CRM;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Enums.Admin;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class CRMViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private ContentControl _description;
        private ContentControl _options;
        private object _selectedObject;
        private bool _isCRMPropertySelected;
        private ObservableCollection<FollowUpStatus> _followUpStatuses;
        private ObservableCollection<EnquiryStatus> _enquiryStatuses;
        private bool _yesNo;

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

        public bool IsEnquiryStatusSelected
        {
            get
            {
                return SelectedObject is EnquiryStatus;
            }
        }

        public ContentControl Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public ContentControl Options
        {
            get { return _options; }
            set
            {
                if (_options == value) return;
                _options = value;
                RaisePropertyChanged(() => Options);
            }
        }

        public bool IsCRMPropertySelected
        {
            get { return _isCRMPropertySelected; }
            set
            {
                _isCRMPropertySelected = value;
                RaisePropertyChanged(() => IsCRMPropertySelected);
            }
        }

        public object SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (_selectedObject == value) return;
                _selectedObject = value;
                RaisePropertyChanged(() => SelectedObject);
                RaisePropertyChanged(() => IsEnquiryStatusSelected);
                DeleteEnquiryStatusCommand.RaiseCanExecuteChanged();
                if (value is FollowUpStatus)
                {
                    Description = new StatusOfFollowUpView((FollowUpStatus)value);
                    Options = null;
                    IsCRMPropertySelected = true;
                    return;
                }
                if (value is EnquiryStatus)
                {
                    Description = new EnquiryStatusView((EnquiryStatus)value);
                    Options = new EnquiryStatusOptionsView((EnquiryStatus)value);
                    IsCRMPropertySelected = true;
                    return;
                }
                RadTreeViewItem selectedItem = (RadTreeViewItem)value;

                if (String.Equals(selectedItem.Header.ToString(), "How Received"))
                {
                    Description = new ReceivedMethodView();
                    Options = null;
                    IsCRMPropertySelected = true;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Number of days"))
                {
                    Description = new NumberOfDaysView();
                    Options = null;
                    IsCRMPropertySelected = true;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Email Notifications"))
                {
                    Description = new EmailNotificationsView();
                    Options = null;
                    IsCRMPropertySelected = true;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Default Web Enquiry Status"))
                {
                    Description = new DefaultWebEnquiryStatus();
                    Options = null;
                    IsCRMPropertySelected = true;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Documents"))
                {
                    Description = new DocumentsView();
                    Options = null;
                    IsCRMPropertySelected = true;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Email Settings"))
                {
                    Description = new EmailSettingsView();
                    Options = null;
                    IsCRMPropertySelected = true;
                }
                else
                {
                    IsCRMPropertySelected = false;
                }
            }
        }

        public ObservableCollection<EnquiryStatus> EnquiryStatuses
        {
            get { return _enquiryStatuses; }
            set
            {
                if (_enquiryStatuses == value) return;
                _enquiryStatuses = value;
                RaisePropertyChanged(() => EnquiryStatuses);
            }
        }

        public ObservableCollection<FollowUpStatus> FollowUpStatuses
        {
            get { return _followUpStatuses; }
            set
            {
                _followUpStatuses = value;
                RaisePropertyChanged(() => FollowUpStatuses);
            }
        }

        public RelayCommand AddEnquiryStatusCommand { get; private set; }

        public RelayCommand DeleteEnquiryStatusCommand { get; private set; }

        #endregion

        #region Constructor

        public CRMViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            AddEnquiryStatusCommand = new RelayCommand(AddEnquiryStatusCommandExecute, AddEnquiryStatusCommandCanExecute);
            DeleteEnquiryStatusCommand = new RelayCommand(DeleteEnquiryStatusCommandExecute,
                DeleteEnquiryStatusCommandCanExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var statysType = Convert.ToInt32(StatusType.FollowUpStatus);
            var followUpStatuses = await _adminDataUnit.FollowUpStatusesRepository.GetAllAsync(followStatus => followStatus.StatusType == statysType);
            FollowUpStatuses = new ObservableCollection<FollowUpStatus>(followUpStatuses.OrderBy(x => x.NumberOfDays));

            var enquiryStatuses = await _adminDataUnit.EnquiryStatusesRepository.GetAllAsync();
            EnquiryStatuses = new ObservableCollection<EnquiryStatus>(enquiryStatuses.OrderBy(x => x.Status));

            IsBusy = false;
        }

        private EnquiryStatus GetNewEnquiryStatus()
        {
            return new EnquiryStatus()
            {
                ID = Guid.NewGuid(),
                Status = "New Enquiry Status",
                IsEnabled = false
            };
        }

        #endregion

        #region Commands

        private void AddEnquiryStatusCommandExecute()
        {
            var newEnquiryStatus = GetNewEnquiryStatus();
            _adminDataUnit.EnquiryStatusesRepository.Add(newEnquiryStatus);
            _adminDataUnit.SaveChanges();
            EnquiryStatuses.Add(newEnquiryStatus);
        }

        private bool AddEnquiryStatusCommandCanExecute()
        {
            return EnquiryStatuses == null || EnquiryStatuses.Count < 10;
        }

        private bool DeleteEnquiryStatusCommandCanExecute()
        {
            return _selectedObject is EnquiryStatus;
        }

        private void DeleteEnquiryStatusCommandExecute()
        {
            var dp = new DialogParameters { Content = "Are you sure you want to delete " + ((EnquiryStatus)SelectedObject).Status + "?" };
            dp.Content += "\n";
            dp.Header = "Warning!";
            dp.OkButtonContent = "Yes";
            dp.CancelButtonContent = "No";
            dp.Closed = ConfirmDeleteClose;
            dp.Owner = Application.Current.MainWindow;

            RadWindow.Confirm(dp);
            if (_yesNo)
            {
                _adminDataUnit.EnquiryStatusesRepository.Delete((EnquiryStatus)SelectedObject);
                _adminDataUnit.SaveChanges();
                EnquiryStatuses.Remove((EnquiryStatus)SelectedObject);
                _yesNo = false;
            }

            SelectedObject = null;
        }

        private void ConfirmDeleteClose(object sender, WindowClosedEventArgs e)
        {
            _yesNo = e.DialogResult == true;
        }

        #endregion
    }
}
