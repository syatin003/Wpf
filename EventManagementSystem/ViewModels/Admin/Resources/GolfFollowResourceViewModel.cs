using System.Collections.ObjectModel;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Services;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using System.Linq;
using EventManagementSystem.Core.Commands;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Practices.ObjectBuilder2;
using EventManagementSystem.CommonObjects.Comparers;
using EventManagementSystem.Data.Model;
using System;

namespace EventManagementSystem.ViewModels.Admin.Resources
{
    public class GolfFollowResourceViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<GolfModel> _golfFollowResources;
        private ObservableCollection<GolfModel> _availableGolfResources;
        private GolfModel _golf;
        private string _selectedField;
        private readonly IAdminDataUnit _adminDataUnit;

        #endregion



        #region Properties

        public ObservableCollection<GolfModel> GolfFollowResources
        {
            get { return _golfFollowResources; }
            set
            {
                if (_golfFollowResources == value) return;
                _golfFollowResources = value;
                RaisePropertyChanged(() => GolfFollowResources);
            }
        }

        public ObservableCollection<GolfModel> AvailableGolfResources
        {
            get { return _availableGolfResources; }
            set
            {
                if (_availableGolfResources == value) return;
                _availableGolfResources = value;
                RaisePropertyChanged(() => AvailableGolfResources);
            }
        }

        public GolfModel Golf
        {
            get { return _golf; }
            set
            {
                if (_golf == value) return;
                _golf = value;
                RaisePropertyChanged(() => Golf);
            }
        }
        public RelayCommand SubmitCommand { get; set; }

        #endregion

        #region Constructor

        public GolfFollowResourceViewModel(GolfModel golfModel)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted);
            Golf = golfModel;
            GolfFollowResources = new ObservableCollection<GolfModel>();
            GolfFollowResources.CollectionChanged += GolfFollowResources_CollectionChanged;
        }

        #endregion



        #region Methods

        private void GolfFollowResources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        private void GolfOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            var golfs = await _adminDataUnit.GolfsRepository.GetAllAsync();
            AvailableGolfResources = new ObservableCollection<GolfModel>(golfs.Select(x => new GolfModel(x)).OrderBy(x => x.Name));
            GolfFollowResources = new ObservableCollection<GolfModel>(Golf.Golf.GolfFollowResources.Select(x => AvailableGolfResources.Where(p => p.Golf.ID == x.FollowResourceID).FirstOrDefault()));
            RaisePropertyChanged("AddSelectedItems");
        }

        #endregion

        #region Commands

        private async void SubmitCommandExecuted()
        {
            var OriginalResources = new ObservableCollection<GolfModel>(Golf.Golf.GolfFollowResources.Select(x => new GolfModel(AvailableGolfResources.Where(p => p.Golf.ID == x.FollowResourceID).FirstOrDefault().Golf)));
            var DeletedObjects = OriginalResources.ToList().Except(GolfFollowResources.ToList(), new GolfFollowResourcesComparer()).ToList();
            var AddedObjects = GolfFollowResources.ToList().Except(OriginalResources.ToList(), new GolfFollowResourcesComparer()).ToList();

            var OrginalFollowGolfResources = await _adminDataUnit.GolfFollowResourceRepository.GetAllAsync(x => x.GolfID == Golf.Golf.ID);

            DeletedObjects.ForEach(deletedResources =>
            {
                _adminDataUnit.GolfFollowResourceRepository.Delete(OrginalFollowGolfResources.Where(p => p.FollowResourceID == deletedResources.Golf.ID).FirstOrDefault());
            });

            AddedObjects.ForEach(addedObject =>
            {
                var obj = new GolfFollowResource()
                       {
                           ID = Guid.NewGuid(),
                           GolfID = Golf.Golf.ID,
                           FollowResourceID = addedObject.Golf.ID
                       };

                _adminDataUnit.GolfFollowResourceRepository.Add(obj);

            });
            //await _adminDataUnit.SaveChanges();
        }

        private bool SubmitCommandCanExecute()
        {
            return (Golf.Golf.GolfFollowResources.Count > 0);
        }

        #endregion


        internal void SetGolfFollowResources(IList selectedGolfs)
        {
            GolfFollowResources = new ObservableCollection<GolfModel>();

            foreach (var item in selectedGolfs)
            {
                var golfModel = item as GolfModel;
                GolfFollowResources.Add(golfModel);
            }

        }
    }
}
