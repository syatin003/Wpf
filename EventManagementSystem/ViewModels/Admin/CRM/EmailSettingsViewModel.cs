using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class EmailSettingsViewModel : ViewModelBase
    {
        #region Fields

        private string _headerImageUrl;
        private string _footerImageUrl;
        private string _headerContent;
        private string _footerContent;
        private string _templateColor;
        private bool _isDataLoadedOnce;
        private bool _isDirty;

        #endregion

        #region Properties

        public string HeaderImageUrl
        {
            get { return _headerImageUrl; }
            set
            {
                if (_headerImageUrl == value) return;
                _headerImageUrl = value;
                RaisePropertyChanged(() => HeaderImageUrl);
            }
        }
        public string FooterImageUrl
        {
            get { return _footerImageUrl; }
            set
            {
                if (_footerImageUrl == value) return;
                _footerImageUrl = value;
                RaisePropertyChanged(() => FooterImageUrl);
            }
        }
        public string HeaderContent
        {
            get { return _headerContent; }
            set
            {
                if (_headerContent == value) return;
                _headerContent = value;
                RaisePropertyChanged(() => HeaderContent);
            }
        }

        public string FooterContent
        {
            get { return _footerContent; }
            set
            {
                if (_footerContent == value) return;
                _footerContent = value;
                RaisePropertyChanged(() => FooterContent);
            }
        }
        public string TemplateColor
        {
            get
            {
                if (_templateColor != String.Empty)
                    return _templateColor;
                return Colors.Purple.ToString();
            }
            set
            {
                if (value == String.Empty)
                {
                    _templateColor = Colors.Purple.ToString();
                }
                if (_templateColor == value) return;
                _templateColor = value;
                RaisePropertyChanged(() => TemplateColor);
            }
        }
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                RaisePropertyChanged(() => IsDirty);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand SaveChangesCommand { get; private set; }
        public RelayCommand AddHeaderImageCommand { get; private set; }
        public RelayCommand AddFooterImageCommand { get; private set; }

        #endregion

        #region Constructor

        public EmailSettingsViewModel()
        {
            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);
            AddHeaderImageCommand = new RelayCommand(AddHeaderImageCommandExecuted);
            AddFooterImageCommand = new RelayCommand(AddFooterImageCommandExecuted);

            LoadSettings();
            this.PropertyChanged += EmailSettingsViewModel_PropertyChanged;
        }

        #endregion

        #region Methods

        private void LoadSettings()
        {
            var appPath = (string)ApplicationSettings.Read("ImagesPath");
            HeaderImageUrl = Properties.Settings.Default.CRMEmailHeaderImage != String.Empty ? string.Concat(appPath, Properties.Settings.Default.CRMEmailHeaderImage) : String.Empty;
            FooterImageUrl = Properties.Settings.Default.CRMEmailFooterImage != String.Empty ? string.Concat(appPath, Properties.Settings.Default.CRMEmailFooterImage) : String.Empty;
            HeaderContent = Properties.Settings.Default.CRMEmailHeader;
            FooterContent = Properties.Settings.Default.CRMEmailFooter;
            TemplateColor = Properties.Settings.Default.CRMEmailTemplateColor;
            _isDataLoadedOnce = true;
        }


        private void EmailSettingsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsDirty")
                if (_isDataLoadedOnce)
                    IsDirty = true;
        }

        #endregion

        #region Commands

        private void SaveChangesCommandExecuted()
        {
            try
            {
                IsDirty = false;

                var appPath = (string)ApplicationSettings.Read("ImagesPath");

                var fullHeaderImagePath = string.Concat(appPath, Guid.NewGuid().ToString(), Path.GetExtension(HeaderImageUrl));
                var fullFooterImagePath = string.Concat(appPath, Guid.NewGuid().ToString(), Path.GetExtension(FooterImageUrl));

                if (File.Exists(HeaderImageUrl))
                {
                    File.Copy(HeaderImageUrl, fullHeaderImagePath);
                    Properties.Settings.Default.CRMEmailHeaderImage = Path.GetFileName(fullHeaderImagePath);
                }
                if (File.Exists(FooterImageUrl))
                {
                    File.Copy(FooterImageUrl, fullFooterImagePath);
                    Properties.Settings.Default.CRMEmailFooterImage = Path.GetFileName(fullFooterImagePath);
                }
                Properties.Settings.Default.CRMEmailHeader = HeaderContent;
                Properties.Settings.Default.CRMEmailFooter = FooterContent;
                Properties.Settings.Default.CRMEmailTemplateColor = TemplateColor;


                Properties.Settings.Default.Save();

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsDirty;
        }

        private void AddHeaderImageCommandExecuted()
        {
            var dialog = new OpenFileDialog() { Multiselect = false, Filter = "Image Files (*.bmp,*.png, *.jpg)|*.bmp;*.png;*.jpg" };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                HeaderImageUrl = dialog.FileName;
            }
        }
        private void AddFooterImageCommandExecuted()
        {
            var dialog = new OpenFileDialog() { Multiselect = false, Filter = "Image Files (*.bmp,*.png, *.jpg)|*.bmp;*.png;*.jpg" };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                FooterImageUrl = dialog.FileName;
            }
        }
        #endregion
    }
}
