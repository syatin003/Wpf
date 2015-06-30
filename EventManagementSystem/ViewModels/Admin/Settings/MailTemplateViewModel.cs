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
using EventManagementSystem.Views.Admin.Settings;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Model;
using System.Text;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class MailTemplateViewModel : ViewModelBase
    {
        MailTemplateModel model;
        #region Fields

        private bool _isBusy;
        private bool _isEdit;
        private readonly IAdminDataUnit _adminDataUnit;
        private MailTemplateModel _template;
        private ObservableCollection<MailTemplateCategory> _mailTemplateCategories;
        private List<MailTemplateType> _mailTemplateTypes;
        private ObservableCollection<EmailHeader> _emailHeaders;
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

        public string Field { get; set; }

        public ImageInline Image { get; set; }

        public MailTemplateModel Template
        {
            get { return _template; }
            set
            {
                if (_template == value) return;
                _template = value;
                RaisePropertyChanged(() => Template);
            }
        }


        public ObservableCollection<MailTemplateCategory> MailTemplateCategories
        {
            get { return _mailTemplateCategories; }
            set
            {
                if (_mailTemplateCategories == value) return;
                _mailTemplateCategories = value;
                RaisePropertyChanged(() => MailTemplateCategories);
            }
        }

        public ObservableCollection<EmailHeader> EmailHeaders
        {
            get { return _emailHeaders; }
            set
            {
                if (_emailHeaders == value) return;
                _emailHeaders = value;
                RaisePropertyChanged(() => EmailHeaders);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand AddTemplateImageCommand { get; private set; }
        public RelayCommand InsertFieldCommand { get; private set; }

        #endregion

        #region Constructor

        public MailTemplateViewModel(MailTemplateModel model)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            InsertFieldCommand = new RelayCommand(InsertFieldCommandExecuted);

            AddTemplateImageCommand = new RelayCommand(AddTemplateImageCommandExecuted);

            ProcessTemplate(model);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;
            var types = await _adminDataUnit.MailTemplateTypesRepository.GetAllAsync();
            _mailTemplateTypes = new List<MailTemplateType>(types);

            var categories = await _adminDataUnit.MailTemplateCategoriesRepository.GetAllAsync();
            MailTemplateCategories = new ObservableCollection<MailTemplateCategory>(categories);

            _adminDataUnit.EmailHeadersRepository.Refresh();
            var headers = await _adminDataUnit.EmailHeadersRepository.GetAllAsync();

            if (_template != null && _template.EmailHeader != null)
                EmailHeaders = new ObservableCollection<EmailHeader>(headers.Where(x => x.IsEnabled || x.ID == _template.EmailHeader.ID));
            else
                EmailHeaders = new ObservableCollection<EmailHeader>(headers.Where(x => x.IsEnabled));

            if (_isEdit)
            {
                var desiredTemplate = await _adminDataUnit.MailTemplatesRepository.GetUpdatedMailTemplate(_template.MailTemplate.ID);
                // Check if we have new changes
                if (desiredTemplate != null && _template.LoadedTime < desiredTemplate.LastUpdatedDate)
                {
                    Template = new MailTemplateModel(desiredTemplate);
                }
            }

            IsBusy = false;
        }

        private void ProcessTemplate(MailTemplateModel model)
        {
            _isEdit = (model != null);

            Template = (_isEdit) ? model : GetTemplate();
            Template.PropertyChanged += TemplateOnPropertyChanged;
        }

        private MailTemplateModel GetTemplate()
        {
            return new MailTemplateModel(new MailTemplate()
            {
                ID = Guid.NewGuid(),
                Name = "New Template",
                Template = "",
                IsEnabled = true,
                LastUpdatedDate = DateTime.Now,
                WhoByID = AccessService.Current.User.ID,
            });
        }

        private void TemplateOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void SaveCommandExecuted()
        {
            if (!_isEdit)
            {
                _template.MailTemplate.TypeID = _mailTemplateTypes.First(x => x.Name == "Email").ID;
                _adminDataUnit.MailTemplatesRepository.Add(_template.MailTemplate);
            }
            else
                _template.MailTemplate.LastUpdatedDate = DateTime.Now;

            _adminDataUnit.SaveChanges();
        }

        private bool SaveCommandCanExecute()
        {
            return !Template.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            if (_isEdit)
            {
                _adminDataUnit.RevertChanges();
            }
            else
            {
                Template = null;
            }
        }

        private void InsertFieldCommandExecuted()
        {
            var window = new MailFieldsView();
            window.ShowDialog();

            if (window.DialogResult == null || window.DialogResult != true || string.IsNullOrWhiteSpace(window.ViewModel.SelectedField)) return;

            Field = EmailService.GetMailField(window.ViewModel.SelectedField);

            RaisePropertyChanged("InsertText");
        }

        private void AddTemplateImageCommandExecuted()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog() { Multiselect = false, Filter = "Image Files (*.bmp,*.png, *.jpg)|*.bmp;*.png;*.jpg" };
            var result = dialog.ShowDialog();
            if (result == true)
            { 
                Image = new ImageInline
                {
                    UriSource = new Uri(dialog.FileName, UriKind.Absolute),
                    Width = 580,
                    Height = 150,
                };
                RaisePropertyChanged("InsertImage");
            }
        }
        #endregion
    }
}
