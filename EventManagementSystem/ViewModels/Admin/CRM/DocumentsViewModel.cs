using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using EventManagementSystem.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Services;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.CRM
{
    public class DocumentsViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Document> _documents;
        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isDirty;

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

        public ObservableCollection<Document> Documents
        {
            get { return _documents; }
            set
            {
                if (_documents == value) return;
                _documents = value;
                RaisePropertyChanged(() => Documents);
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
        public RelayCommand<Document> ShowDocumentCommand { get; private set; }

        public RelayCommand AddDocumentCommand { get; private set; }
        public RelayCommand<Document> DeleteDocumentCommand { get; private set; }
        public RelayCommand SaveChangesCommand { get; private set; }

        #endregion

        #region Constructor

        public DocumentsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            ShowDocumentCommand = new RelayCommand<Document>(ShowDocumentCommandExecuted, report => true);
            AddDocumentCommand = new RelayCommand(AddDocumentCommandExecuted);
            DeleteDocumentCommand = new RelayCommand<Document>(DeleteDocumentCommandExecuted);
            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.DocumentsRepository.Refresh();
            var documents = await _adminDataUnit.DocumentsRepository.GetAllAsync(x => x.IsCommon);
            Documents = new ObservableCollection<Document>(documents);

            Documents.ForEach(x => x.PropertyChanged += DocumentOnPropertyChanged);


            IsBusy = false;
        }

        private void DocumentOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            IsDirty = true;
        }

        private void AddFileFromHdd(string path)
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");
            var filePath = Path.GetFileName(path);
            var fullPath = string.Concat(appPath, filePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            File.Copy(path, fullPath);

            var document = new Document()
            {
                ID = Guid.NewGuid(),
                Name = Path.GetFileNameWithoutExtension(path),
                Path = filePath,
                IsEnabled = true,
                IsCommon = true
            };

            Documents.Add(document);

            _adminDataUnit.DocumentsRepository.Add(document);
            _adminDataUnit.SaveChanges();
        }

        #endregion

        #region Commands

        private void ShowDocumentCommandExecuted(Document obj)
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");
            var exportPath = string.Concat(appPath, obj.Path);

            if (File.Exists(exportPath))
            {
                Process.Start(exportPath);
            }
            else
            {
                PopupService.ShowMessage("Document not found", MessageType.Failed);
            }
        }

        private void AddDocumentCommandExecuted()
        {
            var dialog = new OpenFileDialog() { Multiselect = true };
            var result = dialog.ShowDialog();

            if (result == true)
                dialog.FileNames.ForEach(AddFileFromHdd);
        }

        private void DeleteDocumentCommandExecuted(Document obj)
        {
            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RadWindow.Confirm(new DialogParameters()
            {
                Owner = Application.Current.MainWindow,
                Content = confirmText,
                Closed = (sender, args) => { dialogResult = args.DialogResult; }
            });

            if (dialogResult != true) return;

            var appPath = (string)ApplicationSettings.Read("DocumentsPath");
            var path = string.Concat(appPath, obj.Path);

            if (obj.CorrespondenceDocuments.Any())
            {
                MessageBox.Show("The document can't be removed because it is used in correspondence.");
                return;
            }
            if (File.Exists(path))
                File.Delete(path);

            obj.PropertyChanged -= DocumentOnPropertyChanged;
            Documents.Remove(obj);

            _adminDataUnit.DocumentsRepository.Delete(obj);
            _adminDataUnit.SaveChanges();
        }

        private void SaveChangesCommandExecuted()
        {
            IsDirty = false;
            _adminDataUnit.SaveChanges();
        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsDirty;
        }

        #endregion
    }
}
