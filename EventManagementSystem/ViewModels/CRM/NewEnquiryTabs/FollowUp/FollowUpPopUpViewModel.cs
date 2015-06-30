using System;
using System.Collections.ObjectModel;
using System.Windows;
using EventManagementSystem.CommonObjects;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Core;
using EventManagementSystem.Views.CRM;
using EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System.Linq;

namespace EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.FollowUp
{
    public class FollowUpPopUpViewModel : ViewModelBase
    {
        #region Fields

        private readonly ICrmDataUnit _crmDataUnit;
        private bool _isBusy;
        private FollowUpModel _followUp;
        private ObservableCollection<User> _users;

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

        public string EnquiryName
        {
            get
            {
                if (FollowUp.FollowUp.Enquiry != null)
                    return new EnquiryModel(FollowUp.FollowUp.Enquiry).EnquiryString;
                else
                {
                    return String.Empty;
                }
            }
        }

        public EnquiryModel SelectedEnquiry
        {
            get { return new EnquiryModel(FollowUp.FollowUp.Enquiry); }
        }

        public bool AreEnquiriesVisible
        {
            get { return FollowUp.FollowUp.Enquiry != null; }
        }

        public bool IsTodo
        {
            get { return FollowUp.FollowUp.Enquiry == null; }
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

        public CorresponcenceType CorresponcenceType { get; set; }

        public RelayCommand SnoozeCommand { get; private set; }
        public RelayCommand DeleteFollowUpCommand { get; private set; }

        public RelayCommand OpenEnquiryCommand { get; private set; }

        #endregion

        #region Constructor

        public FollowUpPopUpViewModel(FollowUpModel followUpModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _crmDataUnit = dataUnitLocator.ResolveDataUnit<ICrmDataUnit>();

            FollowUp = followUpModel;

            SnoozeCommand = new RelayCommand(SnoozeCommandExecute);
            OpenEnquiryCommand = new RelayCommand(OpenEnquiryCommandExecute, OpenEnquiryCommandCanExecute);
            DeleteFollowUpCommand = new RelayCommand(DeleteFollowUpCommandExecute);
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            IsBusy = true;

            RaisePropertyChanged(() => FollowUp);

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void OpenEnquiryCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new NewEnquiryView(new EnquiryModel(FollowUp.FollowUp.Enquiry));
            window.ShowDialog();
            if (window.DialogResult != null && window.DialogResult.Value)
            {
                var isToDo = _followUp.IsToDo;
                var IsCurrentFollowUpDeleted = true;

                if (window.ViewModel.Enquiry.FollowUps.Where(followUp => followUp.FollowUp.ID == _followUp.FollowUp.ID).Count() > 0)
                    IsCurrentFollowUpDeleted = false;

                if (IsCurrentFollowUpDeleted)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var currentPopUp = Application.Current.MainWindow;
                        var viewModel = currentPopUp.DataContext as MainWindowModel;
                        var workspaceView = viewModel.WindowContent as WorkspaceView;
                        var tile = workspaceView.RootTileView.MaximizedItem as Tile;
                        if (tile.Name == "CRM")
                        {
                            var crmview = tile.Content as CRMView;
                            var crmvm = crmview.DataContext as CRMViewModel;

                            if (isToDo)
                                crmvm.ReloadFollowUps();
                            else
                            {
                                crmvm.ReloadFollowUpsAndEnquiries();
                            }
                        }
                    }));
                    RaisePropertyChanged("CloseParentWindow");
                }
                else
                {
                    RaisePropertyChanged("EnableParentWindow");
                }
            }
            else
            {
                RaisePropertyChanged("EnableParentWindow");
            }
        }

        private bool OpenEnquiryCommandCanExecute()
        {
            return true;
        }

        private void SnoozeCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var snoozeWindow = new SnoozeView(FollowUp);
            snoozeWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            snoozeWindow.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (snoozeWindow.DialogResult == null || snoozeWindow.DialogResult != true) return;

            var viewModel = snoozeWindow.DataContext as SnoozeViewModel;
            switch (viewModel.SnoozeTime)
            {
                case "5 mins":
                    FollowUp.DateDue = DateTime.Now + TimeSpan.FromMinutes(5);
                    break;
                case "10 mins":
                    FollowUp.DateDue = DateTime.Now + TimeSpan.FromMinutes(10);
                    break;
                case "30 mins":
                    FollowUp.DateDue = DateTime.Now + TimeSpan.FromMinutes(30);
                    break;
                case "1 Hour":
                    FollowUp.DateDue = DateTime.Now + TimeSpan.FromHours(1);
                    break;
                case "2 Hours":
                    FollowUp.DateDue = DateTime.Now + TimeSpan.FromHours(2);
                    break;
                case "1 Day":
                    FollowUp.DateDue = DateTime.Now + TimeSpan.FromDays(1);
                    break;
                case "7 Days":
                    FollowUp.DateDue = DateTime.Now + TimeSpan.FromDays(7);
                    break;
            }

            _crmDataUnit.SaveChanges();

            var window = Application.Current.MainWindow;
            var mainViewModel = window.DataContext as MainWindowModel;
            var workspaceView = mainViewModel.WindowContent as WorkspaceView;
            var tile = workspaceView.RootTileView.MaximizedItem as Tile;
            if (tile.Name == "CRM")
            {
                var crmview = tile.Content as CRMView;
                var crmvm = crmview.DataContext as CRMViewModel;
                crmvm.FollowUps.ForEach(x => x.Refresh());
            }
        }

        private async void DeleteFollowUpCommandExecute()
        {
            var isToDo = _followUp.IsToDo;
            _crmDataUnit.FollowUpsRepository.Delete(_followUp.FollowUp);
            var result = await _crmDataUnit.SaveChanges().ConfigureAwait(false);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var window = Application.Current.MainWindow;
                var viewModel = window.DataContext as MainWindowModel;
                var workspaceView = viewModel.WindowContent as WorkspaceView;
                var tile = workspaceView.RootTileView.MaximizedItem as Tile;
                if (tile.Name == "CRM")
                {
                    var crmview = tile.Content as CRMView;
                    var crmvm = crmview.DataContext as CRMViewModel;

                    if (isToDo)
                        crmvm.ReloadFollowUps();
                    else
                    {
                        crmvm.ReloadFollowUpsAndEnquiries();
                    }
                }
            }));
        }

        #endregion
    }
}