using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Admin.Resources;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Admin.Resources
{
    public class ResourcesViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private ObservableCollection<GolfModel> _golfs;
        private ObservableCollection<RoomModel> _rooms;
        private bool _isBusy;
        private object _selectedResource;
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

        public ObservableCollection<RoomModel> Rooms
        {
            get { return _rooms; }
            set
            {
                if (_rooms == value) return;
                _rooms = value;
                RaisePropertyChanged(() => Rooms);
            }
        }

        public ObservableCollection<GolfModel> Golfs
        {
            get { return _golfs; }
            set
            {
                if (_golfs == value) return;
                _golfs = value;
                RaisePropertyChanged(() => Golfs);
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

        public object SelectedResource
        {
            get { return _selectedResource; }
            set
            {
                if (_selectedResource == value) return;
                _selectedResource = value;
                RaisePropertyChanged(() => SelectedResource);

                if (value is RoomModel)
                {
                    Content = new RoomView(value as RoomModel);
                }
                else if (value is GolfModel)
                {
                    Content = new GolfView(value as GolfModel);
                }
            }
        }

        public RelayCommand AddRoomCommand { get; set; }
        public RelayCommand AddGolfCommand { get; set; }
        public RelayCommand DeleteResourceCommand { get; set; }

        #endregion

        #region Constructors

        public ResourcesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            AddRoomCommand = new RelayCommand(AddRoomCommandExecuted, AddRoomCommandCanExecute);
            AddGolfCommand = new RelayCommand(AddGolfCommandExecuted, AddGolfCommandCanExecute);
            DeleteResourceCommand = new RelayCommand(DeleteResourceCommandExecuted, DeleteResourceCommandCanExecute);
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            IsBusy = true;
            OnLoadRooms();
        }

        private async void OnLoadRooms()
        {
            _adminDataUnit.RoomsRepository.Refresh();

            var rooms = await _adminDataUnit.RoomsRepository.GetAllAsync();
            Rooms = new ObservableCollection<RoomModel>(rooms.Select(x => new RoomModel(x)).OrderBy(x => x.Name));

            OnLoadGolfs();
        }

        private async void OnLoadGolfs()
        {
            _adminDataUnit.GolfsRepository.Refresh();

            var golfs = await _adminDataUnit.GolfsRepository.GetAllAsync();
            Golfs = new ObservableCollection<GolfModel>(golfs.Select(x => new GolfModel(x)).OrderBy(x => x.Name));

            foreach (var golf in Golfs)
            {
                SetAvailableGolfs(golf);
                SetGolfTurnDefault(golf);
            }

            OnDataLoaded();
        }

        private void SetAvailableGolfs(GolfModel golf)
        {
            golf.AvailableGolfs = new ObservableCollection<GolfModel>(golf.Golf.GolfFollowResources.Select(x => Golfs.Where(p => p.Golf.ID == x.FollowResourceID).FirstOrDefault()));
        }

        private void SetGolfTurnDefault(GolfModel model)
        {
            if (model.Golf.TurnDefault != null)
            {
                if (Golfs.Any(golf => golf.Golf.ID == model.Golf.TurnDefault))
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        new Action(
                            () =>
                            {
                                model.TurnDefaultGolf =
                                    Golfs.FirstOrDefault(golf => golf.Golf.ID == model.Golf.TurnDefault);
                            }));
                }
            }
        }

        private void OnDataLoaded()
        {
            IsBusy = false;
        }

        #endregion

        #region Commands

        private void AddRoomCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddRoomView();
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true && view.ViewModel.Room != null)
            {
                Rooms.Add(view.ViewModel.Room);
                SelectedResource = view.ViewModel.Room;
            }
        }

        private void AddGolfCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var view = new AddGolfView();
            view.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (view.DialogResult != null && view.DialogResult == true && view.ViewModel.Golf != null)
            {
                //view.ViewModel.Golf.AvailableGolfs =new ObservableCollection<GolfModel>(_golfs.Except(Enumerable.Repeat(view.ViewModel.Golf, 1)));

                Golfs.Add(view.ViewModel.Golf);
                SelectedResource = view.ViewModel.Golf;
            }
        }

        private void DeleteResourceCommandExecuted()
        {
            bool? dialogResult = null;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM,
                (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            if (SelectedResource is RoomModel)
            {
                var room = SelectedResource as RoomModel;

                Rooms.Remove(room);

                // Before delete the room we should remove all Room Facilies
                _adminDataUnit.RoomFacilitiesRepository.Delete(room.Room.RoomFacilities.ToList());

                _adminDataUnit.RoomsRepository.Delete(room.Room);
                _adminDataUnit.SaveChanges();
            }
            else if (SelectedResource is GolfModel)
            {
                var golf = SelectedResource as GolfModel;

                Golfs.Remove(golf);

                _adminDataUnit.GolfsRepository.Delete(golf.Golf);
                _adminDataUnit.SaveChanges();
            }

            Content = null;
        }

        private bool AddGolfCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_ADD_RESOURCE_ALLOWED);
        }

        private bool AddRoomCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_ADD_RESOURCE_ALLOWED);
        }

        private bool DeleteResourceCommandCanExecute()
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_DELETE_RESOURCE_ALLOWED);
        }

        #endregion
    }
}