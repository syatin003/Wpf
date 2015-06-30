using EventManagementSystem.Core.Commands;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Views.Admin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class EmailHeadersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private ObservableCollection<EmailHeaderModel> _emailHeaders;
        private EmailHeaderModel _selectedEmailHeader;

        #endregion

        #region Properties

        public List<EmailHeader> AllEmailHeaders { get; set; }

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

        public ObservableCollection<EmailHeaderModel> EmailHeaders
        {
            get { return _emailHeaders; }
            set
            {
                if (_emailHeaders == value) return;
                _emailHeaders = value;
                RaisePropertyChanged(() => EmailHeaders);
            }
        }

        public EmailHeaderModel SelectedEmailHeader
        {
            get { return _selectedEmailHeader; }
            set
            {
                if (_selectedEmailHeader == value) return;
                _selectedEmailHeader = value;
                RaisePropertyChanged(() => SelectedEmailHeader);

            }
        }

        public RelayCommand<EmailHeaderModel> DeleteEmailHeaderCommand { get; private set; }
        public RelayCommand<EmailHeaderModel> EditEmailHeaderCommand { get; private set; }

        #endregion

        #region Constructors

        public EmailHeadersViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            DeleteEmailHeaderCommand = new RelayCommand<EmailHeaderModel>(DeleteEmailHeaderCommandExecuted);
            EditEmailHeaderCommand = new RelayCommand<EmailHeaderModel>(EditEmailHeaderCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.EmailHeadersRepository.Refresh();
            var emailHeaders = await _adminDataUnit.EmailHeadersRepository.GetAllAsync();
            AllEmailHeaders = new List<EmailHeader>(emailHeaders.OrderBy(x => x.Name));

            RefreshEmailHeaders();

            IsBusy = false;
        }

        public void RefreshEmailHeaders()
        {
            EmailHeaders = new ObservableCollection<EmailHeaderModel>(AllEmailHeaders.Select(x => new EmailHeaderModel(x)));
        }

        #endregion

        #region Commands

        private void DeleteEmailHeaderCommandExecuted(EmailHeaderModel emailHeader)
        {
            if (emailHeader == null) return;

            RaisePropertyChanged("DisableParentWindow");

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            _adminDataUnit.EmailHeadersRepository.Delete(emailHeader.EmailHeader);

            _adminDataUnit.SaveChanges();

            AllEmailHeaders.Remove(emailHeader.EmailHeader);

            RefreshEmailHeaders();
        }

        private void EditEmailHeaderCommandExecuted(EmailHeaderModel emailHeader)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddEmailHeaderView(emailHeader);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
            if (view.DialogResult != null && view.DialogResult == true)
            {
                _adminDataUnit.EmailHeadersRepository.Refresh();
            }
        }

        #endregion
    }
}
