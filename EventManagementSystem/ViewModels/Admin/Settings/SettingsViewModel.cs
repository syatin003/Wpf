using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
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
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private readonly IAdminDataUnit _adminDataUnit;
        private ObservableCollection<MailTemplateCategoryModel> _mailTemplateCategories;
        private object _selectedItem;
        private ContentControl _content;

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

        public ObservableCollection<MailTemplateCategoryModel> MailTemplateCategories
        {
            get { return _mailTemplateCategories; }
            set
            {
                if (_mailTemplateCategories == value) return;
                _mailTemplateCategories = value;
                RaisePropertyChanged(() => MailTemplateCategories);
            }
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);

                if (_selectedItem.ToString() == "Email Settings")
                    Content = new EmailSettingsView();
                else if (_selectedItem.ToString() == "Club Information")
                    Content = new ClubInformationView();
                else if (_selectedItem.ToString() == "System Settings")
                    Content = new SystemSettingsView();
                else if (_selectedItem.ToString() == "Unlock Events")
                    Content = new UnlockEventsView();
                else if (_selectedItem.ToString() == "Templates")
                    Content = new TemplatesView(null);
                else if (_selectedItem is MailTemplateCategoryModel)
                    Content = new TemplatesView(_selectedItem as MailTemplateCategoryModel);
                else if (_selectedItem.ToString() == "Email Headers")
                Content = new EmailHeadersView();

                RemoveTemplateCommand.RaiseCanExecuteChanged();
            }
        }

        public ContentControl Content
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;
                RaisePropertyChanged(() => Content);
            }
        }

        public RelayCommand AddTemplateCommand { get; private set; }
        public RelayCommand RemoveTemplateCommand { get; private set; }
        public RelayCommand AddEmailHeaderCommand { get; private set; }

        #endregion

        #region Constructors

        public SettingsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            AddTemplateCommand = new RelayCommand(AddTemplateCommandExecuted);
            RemoveTemplateCommand = new RelayCommand(RemoveTemplateCommandExecuted, RemoveTemplateCommandCanExecute);
            AddEmailHeaderCommand = new RelayCommand(AddEmailHeaderCommandExecuted);

        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var categories = await _adminDataUnit.MailTemplateCategoriesRepository.GetAllAsync();
            MailTemplateCategories = new ObservableCollection<MailTemplateCategoryModel>(categories.Select(x => new MailTemplateCategoryModel(x)));

            IsBusy = false;
        }

        #endregion

        private void AddTemplateCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new MailTemplateView(null);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult == true)
            {
                _adminDataUnit.SaveChanges();

                if (Content is TemplatesView)
                {
                    var view = Content as TemplatesView;
                    var viewModel = view.DataContext as TemplatesViewModel;
                    viewModel.AllTemplates.Add(window.ViewModel.Template);
                    viewModel.RefreshTemplates();
                }
            }
        }

        private void RemoveTemplateCommandExecuted()
        {
            var template = SelectedItem as MailTemplateModel;

            if (template == null) return;

            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            if (dialogResult != true) return;

            _adminDataUnit.MailTemplatesRepository.Delete(template.MailTemplate);
            _adminDataUnit.SaveChanges();
        }

        private bool RemoveTemplateCommandCanExecute()
        {
            return SelectedItem is MailTemplateModel;
        }

        private void AddEmailHeaderCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addEmailHeaderWindow = new AddEmailHeaderView(null);
            addEmailHeaderWindow.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (addEmailHeaderWindow.DialogResult != null && addEmailHeaderWindow.DialogResult == true)
            {
                if (Content is EmailHeadersView)
                {
                    var view = Content as EmailHeadersView;
                    var viewModel = view.DataContext as EmailHeadersViewModel;
                    viewModel.AllEmailHeaders.Add(addEmailHeaderWindow.ViewModel.EmailHeader.EmailHeader);
                    viewModel.RefreshEmailHeaders();
                }
            }
        }
    }
}
