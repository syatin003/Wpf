using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class ReceivedMethodViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private ObservableCollection<EnquiryReceiveMethod> _enquiryReceivedMethods;
        private string _newEnquiryReceivedMethod;
        private EnquiryReceiveMethod _selectedEnquiryReceivedMethod;
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

        public ObservableCollection<EnquiryReceiveMethod> EnquiryReceivedMethods
        {
            get { return _enquiryReceivedMethods; }
            set
            {
                if (_enquiryReceivedMethods == value) return;
                _enquiryReceivedMethods = value;
                RaisePropertyChanged(() => EnquiryReceivedMethods);
            }
        }

        public string NewEnquiryReceivedMethod
        {
            get { return _newEnquiryReceivedMethod; }
            set
            {
                if (_newEnquiryReceivedMethod == value) return;
                _newEnquiryReceivedMethod = value;
                RaisePropertyChanged(() => NewEnquiryReceivedMethod);
                AddNewEnquiryReceivedMethodCommand.RaiseCanExecuteChanged();               
            }
        }

        public EnquiryReceiveMethod SelectedEnquiryReceivedMethod
        {
            get { return _selectedEnquiryReceivedMethod; }
            set
            {
                if (_selectedEnquiryReceivedMethod == value) return;
                _selectedEnquiryReceivedMethod = value;
                RaisePropertyChanged(() => SelectedEnquiryReceivedMethod);
            }
        }
 
        public RelayCommand AddNewEnquiryReceivedMethodCommand { get; private set; }

        public RelayCommand DeleteEnquiryReceivedMethodCommand { get; private set; }

        #endregion

        #region Constructor

        public ReceivedMethodViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();
         
            AddNewEnquiryReceivedMethodCommand = new RelayCommand(AddNewEnquiryReceivedMethodCommandExecute, AddNewEnquiryReceivedMethodCommandCanExecute);
            DeleteEnquiryReceivedMethodCommand = new RelayCommand(DeleteEnquiryReceivedMethodCommandExecute);           
        }

        #endregion

        #region Methods

        private EnquiryReceiveMethod GetNewEnquiryReceivedMethod()
        {
            return new EnquiryReceiveMethod()
            {
                ID = Guid.NewGuid(),
                ReceiveMethod = NewEnquiryReceivedMethod
            };
        }

        public async void LoadData()
        {
            IsBusy = true;

            var methods = await _adminDataUnit.EnquiryReceiveMethodsRepository.GetAllAsync();
            EnquiryReceivedMethods = new ObservableCollection<EnquiryReceiveMethod>(methods.OrderBy(x => x.ReceiveMethod));

            IsBusy = false;
        }

        #endregion

        #region Commands
      
        private bool AddNewEnquiryReceivedMethodCommandCanExecute()
        {
            return !String.IsNullOrEmpty(NewEnquiryReceivedMethod);
        }

        private void AddNewEnquiryReceivedMethodCommandExecute()
        {
            var newEnquiryReceivedMethod = GetNewEnquiryReceivedMethod();
            _adminDataUnit.EnquiryReceiveMethodsRepository.Add(newEnquiryReceivedMethod);
            _adminDataUnit.SaveChanges();
            EnquiryReceivedMethods.Add(newEnquiryReceivedMethod);
            NewEnquiryReceivedMethod = String.Empty;
        }       

        private void DeleteEnquiryReceivedMethodCommandExecute()
        {
            var dp = new DialogParameters { Content = "Are you sure you want to delete " + SelectedEnquiryReceivedMethod.ReceiveMethod + " ?" };
            dp.Content += "\n";          
            dp.Header = "Warning!";
            dp.OkButtonContent = "Yes";
            dp.CancelButtonContent = "No";
            dp.Closed = ConfirmDeleteClose;
            dp.Owner = Application.Current.MainWindow;

            RadWindow.Confirm(dp);
            if (_yesNo)
            {
                _adminDataUnit.EnquiryReceiveMethodsRepository.Delete(SelectedEnquiryReceivedMethod);
                _adminDataUnit.SaveChanges();
                EnquiryReceivedMethods.Remove(SelectedEnquiryReceivedMethod);
                _yesNo = false;
            }
        }

        private void ConfirmDeleteClose(object sender, WindowClosedEventArgs e)
        {
            _yesNo = e.DialogResult == true;
        }

        #endregion
    }
}

