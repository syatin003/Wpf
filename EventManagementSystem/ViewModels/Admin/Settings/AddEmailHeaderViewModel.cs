using EventManagementSystem.Core.Unity;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Commands;
using System.ComponentModel;
using Microsoft.Win32;
using EventManagementSystem.Services;
using System.IO;
using System.Drawing;
using Telerik.Windows.Controls;
using System.Windows;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class AddEmailHeaderViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isEdit;
        private EmailHeaderModel _emailHeader;

        #endregion Fields

        #region Properties

        public bool IsBusy
        {
            get
            { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public EmailHeaderModel EmailHeader
        {
            get
            { return _emailHeader; }
            set
            {
                if (_emailHeader == value) return;
                _emailHeader = value;
                RaisePropertyChanged(() => EmailHeader);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand AddHeaderImageCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public AddEmailHeaderViewModel(EmailHeaderModel emailHeader)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
            AddHeaderImageCommand = new RelayCommand(AddHeaderImageCommandExecuted);

            ProcessHeader(emailHeader);
        }

        #endregion Constructor

        #region Methods

        private void ProcessHeader(EmailHeaderModel emailHeader)
        {
            _isEdit = emailHeader != null;
            EmailHeader = _isEdit ? emailHeader : GetNewHeader();

            if (_isEdit)
            {
                var appPath = (string)ApplicationSettings.Read("ImagesPath");
                if (!string.IsNullOrWhiteSpace(appPath))
                    EmailHeader.HeaderImageUrl = string.Concat(appPath, _emailHeader.ImageName);
            }
            EmailHeader.PropertyChanged += EmailHeaderViewModel_PropertyChanged;
        }

        private void EmailHeaderViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private EmailHeaderModel GetNewHeader()
        {
            return new EmailHeaderModel(new EmailHeader
            {
                ID = Guid.NewGuid(),
                IsEnabled = true,
            });
        }

        #endregion Methods

        #region Commands

        private void CancelCommandExecuted()
        {
            if (_isEdit)
                _adminDataUnit.RevertChanges();
            else
                EmailHeader = null;
        }

        private bool SaveCommandCanExecute()
        {
            if (IsBusy)
                return false;
            return !_emailHeader.HasErrors;
        }

        private async void SaveCommandExecuted()
        {
            try
            {
                IsBusy = true;

                var appPath = (string)ApplicationSettings.Read("ImagesPath");

                var fullHeaderImagePath = string.Concat(appPath, Guid.NewGuid().ToString(), Path.GetExtension(_emailHeader.HeaderImageUrl));

                if (File.Exists(_emailHeader.HeaderImageUrl))
                {
                    File.Copy(_emailHeader.HeaderImageUrl, fullHeaderImagePath);
                    _emailHeader.ImageName = Path.GetFileName(fullHeaderImagePath);
                }
                if (!_isEdit)
                    _adminDataUnit.EmailHeadersRepository.Add(_emailHeader.EmailHeader);
                else
                    _emailHeader.EmailHeader.LastUpdatedDate = DateTime.Now;
                await _adminDataUnit.SaveChanges();

                IsBusy = false;
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void AddHeaderImageCommandExecuted()
        {
            var dialog = new OpenFileDialog() { Multiselect = false, Filter = "Image Files (*.bmp,*.png, *.jpg)|*.bmp;*.png;*.jpg" };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                FileInfo file = new FileInfo(dialog.FileName);
                Bitmap img = new Bitmap(dialog.FileName);
                if (img.Width < 600 || img.Width > 600 || img.Height < 150 || img.Height > 150)
                {
                    bool? dialogResult = null;
                    string confirmText = "The agreed size of the image is 600px x 150px.Do you want to continue?";

                    RaisePropertyChanged("DisableParentWindow");

                    RadWindow.Confirm(new DialogParameters()
                    {
                        Owner = Application.Current.MainWindow,
                        Content = confirmText,
                        Header = "Warning!",
                        Closed = (sender, args) => { dialogResult = args.DialogResult; }
                    });

                    RaisePropertyChanged("EnableParentWindow");

                    if (dialogResult != null && dialogResult.Value)
                    {
                        _emailHeader.HeaderImageUrl = dialog.FileName;
                    }
                }
                else
                    _emailHeader.HeaderImageUrl = dialog.FileName;
            }
        }
        #endregion Commands
    }
}
