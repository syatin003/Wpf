using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Admin.Settings;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Settings
{
    public class TemplatesViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private List<MailTemplateType> _mailTemplateTypes;
        private List<MailTemplateCategoryModel> _mailTemplateCategories;
        private ObservableCollection<MailTemplateModel> _templates;
        private MailTemplateModel _selectedTemplate;
        private MailTemplateCategoryModel _selectedTemplateCategory;

        #endregion

        #region Properties

        public List<MailTemplateModel> AllTemplates { get; set; }

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

        public List<MailTemplateType> MailTemplateTypes
        {
            get { return _mailTemplateTypes; }
            set
            {
                if (_mailTemplateTypes == value) return;
                _mailTemplateTypes = value;
                RaisePropertyChanged(() => MailTemplateTypes);
            }
        }

        public List<MailTemplateCategoryModel> MailTemplateCategories
        {
            get { return _mailTemplateCategories; }
            set
            {
                if (_mailTemplateCategories == value) return;
                _mailTemplateCategories = value;
                RaisePropertyChanged(() => MailTemplateCategories);
            }
        }

        public ObservableCollection<MailTemplateModel> Templates
        {
            get { return _templates; }
            set
            {
                if (_templates == value) return;
                _templates = value;
                RaisePropertyChanged(() => Templates);
            }
        }

        public MailTemplateModel SelectedTemplate
        {
            get { return _selectedTemplate; }
            set
            {
                if (_selectedTemplate == value) return;
                _selectedTemplate = value;
                RaisePropertyChanged(() => SelectedTemplate);

                DeleteTemplateCommand.RaiseCanExecuteChanged();
                EditTemplateCommand.RaiseCanExecuteChanged();
            }
        }

        public MailTemplateCategoryModel SelectedMailTemplateCategory
        {
            get { return _selectedTemplateCategory; }
            set
            {
                if (_selectedTemplateCategory == value) return;
                _selectedTemplateCategory = value;
                RaisePropertyChanged(() => SelectedMailTemplateCategory);

                RefreshTemplates();
            }
        }

        public RelayCommand DeleteTemplateCommand { get; private set; }
        public RelayCommand<MailTemplateModel> EditTemplateCommand { get; private set; }

        #endregion

        #region Constructors

        public TemplatesViewModel(MailTemplateCategoryModel category)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            _selectedTemplateCategory = category;

            DeleteTemplateCommand = new RelayCommand(DeleteTemplateCommandExecuted, DeleteTemplateCommandCanExecute);
            EditTemplateCommand = new RelayCommand<MailTemplateModel>(EditTemplateCommandExecuted, EditTemplateCommandCanExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.MailTemplatesRepository.Refresh();
            var templates = await _adminDataUnit.MailTemplatesRepository.GetAllAsync();
            AllTemplates = new List<MailTemplateModel>(templates.Where(x => x.MailTemplateType.Name != "MainEmailTemplate").OrderBy(x => x.Name).Select(x => new MailTemplateModel(x)));

            RefreshTemplates();

            IsBusy = false;
        }

        public void RefreshTemplates()
        {
            Templates = new ObservableCollection<MailTemplateModel>(
                (SelectedMailTemplateCategory != null) ? AllTemplates.Where(x => x.MailTemplateCategory == SelectedMailTemplateCategory.MailTemplateCategory) : AllTemplates);
        }

        #endregion

        #region Commands

        private void DeleteTemplateCommandExecuted()
        {
            if (SelectedTemplate == null) return;

            RaisePropertyChanged("DisableParentWindow");

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            _adminDataUnit.MailTemplatesRepository.Delete(_selectedTemplate.MailTemplate);

            _adminDataUnit.SaveChanges();

            AllTemplates.Remove(SelectedTemplate);
            RefreshTemplates();
        }

        private bool DeleteTemplateCommandCanExecute()
        {
            return SelectedTemplate != null;
        }

        private void EditTemplateCommandExecuted(MailTemplateModel model)
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new MailTemplateView(model);
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true)
            {
                RefreshTemplates();
            }
        }

        private bool EditTemplateCommandCanExecute(MailTemplateModel model)
        {
            return model != null;
        }

        #endregion
    }
}
