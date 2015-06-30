using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Enums.Admin;
using EventManagementSystem.Views.Admin.Events;

namespace EventManagementSystem.ViewModels.Admin.Events
{
    public class EventsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private bool _isEventPropertySelected;
        private ObservableCollection<EventTypeModel> _eventTypes;
        private ObservableCollection<EventStatusModel> _eventStatuses;
        private ObservableCollection<FollowUpStatus> _todoStatuses;
        private object _selectedObject;
        private EventTypeModel _selectedEventType;
        private EventStatusModel _selectedEventStatus;
        private FollowUpStatus _selectedTODOStatus;
        private List<User> _users;
        private bool _isToDoStatusesTreeExpanded;
        private EventTypeToDoModel _selectedEventTypeTodDo;
        private bool _isDirty;

        #endregion

        #region Properties

        public List<EventOption> EventOptions { get; set; }

        public List<User> Users
        {
            get { return _users; }
            set
            {
                if (_users == value) return;
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public EventTypeModel SelectedEventType
        {
            get { return _selectedEventType; }
            set
            {
                if (_selectedEventType == value) return;
                _selectedEventType = value;
                RaisePropertyChanged(() => SelectedEventType);

                DeleteEventPropertyCommand.RaiseCanExecuteChanged();

                if (SelectedEventType != null)
                {
                    SelectedEventType.PropertyChanged += OnPropertyChanged;
                    SelectedEventType.EventType.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        public EventStatusModel SelectedEventStatus
        {
            get { return _selectedEventStatus; }
            set
            {
                if (_selectedEventStatus == value) return;
                _selectedEventStatus = value;
                RaisePropertyChanged(() => SelectedEventStatus);

                DeleteEventPropertyCommand.RaiseCanExecuteChanged();
                if (SelectedEventStatus!= null)
                {
                    SelectedEventStatus.PropertyChanged += OnPropertyChanged;
                    SelectedEventStatus.EventStatus.PropertyChanged += OnPropertyChanged;
                }

            }
        }

        public FollowUpStatus SelectedTODOStatus
        {
            get { return _selectedTODOStatus; }
            set
            {
                if (_selectedTODOStatus == value) return;
                _selectedTODOStatus = value;
                RaisePropertyChanged(() => SelectedTODOStatus);

                DeleteEventPropertyCommand.RaiseCanExecuteChanged();

                if (SelectedTODOStatus!= null)
                {
                    SelectedTODOStatus.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public bool IsEventPropertySelected
        {
            get { return _isEventPropertySelected; }
            set
            {
                _isEventPropertySelected = value;
                RaisePropertyChanged(() => IsEventPropertySelected);
            }
        }

        public object SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (_selectedObject == value) return;
                if (IsDirty)
                {

                    if (_selectedObject is EventTypeModel)
                    {
                        _adminDataUnit.EventTypesRepository.Refresh((_selectedObject as EventTypeModel).EventType);
                    }
                    else if (_selectedObject is EventStatusModel)
                    {
                        _adminDataUnit.EventStatusesRepository.Refresh((_selectedObject as EventStatusModel).EventStatus);
                    }
                    else if (_selectedObject is FollowUpStatus)
                    {
                        _adminDataUnit.FollowUpStatusesRepository.Refresh((_selectedObject as FollowUpStatus));
                    }
                }
                IsDirty = false;
                _selectedObject = value;
                RaisePropertyChanged(() => SelectedObject);

                SelectedEventType = value as EventTypeModel;
                SelectedEventStatus = value as EventStatusModel;
                SelectedTODOStatus = value as FollowUpStatus;

                IsEventPropertySelected = value is EventTypeModel || value is EventStatusModel || value is FollowUpStatus;
            }
        }


        public ObservableCollection<EventTypeModel> EventTypes
        {
            get { return _eventTypes; }
            set
            {
                if (_eventTypes == value) return;
                _eventTypes = value;
                RaisePropertyChanged(() => EventTypes);
            }
        }

        public ObservableCollection<EventStatusModel> EventStatuses
        {
            get { return _eventStatuses; }
            set
            {
                if (_eventStatuses == value) return;
                _eventStatuses = value;
                RaisePropertyChanged(() => EventStatuses);
            }
        }

        public ObservableCollection<FollowUpStatus> TODOStatuses
        {
            get { return _todoStatuses; }
            set
            {
                if (_todoStatuses == value) return;
                _todoStatuses = value;
                RaisePropertyChanged(() => TODOStatuses);
            }
        }

        public bool IsToDoStatusesTreeExpanded
        {
            get { return _isToDoStatusesTreeExpanded; }
            set
            {
                if (_isToDoStatusesTreeExpanded == value) return;
                _isToDoStatusesTreeExpanded = value;
                RaisePropertyChanged(() => IsToDoStatusesTreeExpanded);
            }
        }

        public EventTypeToDoModel SelectedEventTypeToDo
        {
            get { return _selectedEventTypeTodDo; }
            set
            {
                if (_selectedEventTypeTodDo == value) return;
                _selectedEventTypeTodDo = value;
                RaisePropertyChanged(() => SelectedEventTypeToDo);
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

        public RelayCommand AddEventTypeCommand { get; private set; }

        public RelayCommand AddEventStatusCommand { get; private set; }

        public RelayCommand AddTODOStatusCommand { get; private set; }

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand DeleteEventPropertyCommand { get; set; }

        public RelayCommand AddDefaultReminderCommand { get; private set; }

        public RelayCommand<EventTypeToDoModel> EditEventTypeToDoCommand { get; private set; }

        public RelayCommand<EventTypeToDoModel> DeleteEventTypeToDoCommand { get; private set; }

        #endregion

        #region Constructor

        public EventsViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            AddEventTypeCommand = new RelayCommand(AddEventTypeCommandExecuted, () => true);
            AddEventStatusCommand = new RelayCommand(AddEventStatusCommandExecuted, () => true);
            AddTODOStatusCommand = new RelayCommand(AddTODOStatusCommandExecuted, () => true);
            SaveChangesCommand = new RelayCommand(SaveChangesCommandExecuted, SaveChangesCommandCanExecute);
            AddDefaultReminderCommand = new RelayCommand(AddDefaultReminderCommandExecuted);
            DeleteEventPropertyCommand = new RelayCommand(DeleteEventPropertyCommandExecuted, DeleteEventPropertyCommandCanExecute);

            EditEventTypeToDoCommand = new RelayCommand<EventTypeToDoModel>(EditEventTypeToDoCommandExecuted);
            DeleteEventTypeToDoCommand = new RelayCommand<EventTypeToDoModel>(DeleteEventTypeToDoCommandExecuted);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            var users = await _adminDataUnit.UsersRepository.GetUsersAsync();
            Users = new List<User>(users.OrderBy(x => x.FirstName));

            var options = await _adminDataUnit.EventOptionsRepository.GetAllAsync();
            EventOptions = new List<EventOption>(options);

            _adminDataUnit.EventTypesRepository.Refresh();

            var eventTypes = await _adminDataUnit.EventTypesRepository.GetAllAsyncWithDefaultToDos();
            EventTypes = new ObservableCollection<EventTypeModel>(eventTypes.Select(x => new EventTypeModel(x)));

            foreach (var eventTypeModel in EventTypes)
            {
                LoadEventTypeOptions(eventTypeModel);
            }

            _adminDataUnit.EventStatusesRepository.Refresh();

            var eventStatuses = await _adminDataUnit.EventStatusesRepository.GetAllAsync();
            EventStatuses = new ObservableCollection<EventStatusModel>(eventStatuses.Select(x => new EventStatusModel(x)));

            foreach (var eventStatusModel in EventStatuses)
            {
                LoadEventStatusOptions(eventStatusModel);
            }
            var statusType = Convert.ToInt32(StatusType.ToDosStatus);
            var todoStatuses = await _adminDataUnit.FollowUpStatusesRepository.GetAllAsync(todoStatus => todoStatus.StatusType == statusType);
            TODOStatuses = new ObservableCollection<FollowUpStatus>(todoStatuses.OrderBy(x => x.NumberOfDays));

            IsBusy = false;
        }

        private void LoadEventTypeOptions(EventTypeModel eventTypeModel)
        {
            eventTypeModel.Options = new ObservableCollection<EventOptionModel>();
            var eventTypeOptions = eventTypeModel.EventType.EventTypeOptions.Select(x => x.EventOption);

            foreach (EventOption eventOption in EventOptions)
            {
                var eventOptionModel = new EventOptionModel(eventOption)
                {
                    IsChecked = eventTypeOptions.Contains(eventOption)
                };

                eventOptionModel.PropertyChanged += EventTypeOptionOnPropertyChanged;
                eventTypeModel.Options.Add(eventOptionModel);
            }
        }

        private void LoadEventStatusOptions(EventStatusModel eventStatusModel)
        {
            eventStatusModel.Options = new ObservableCollection<EventOptionModel>();
            var eventStatusOptions = eventStatusModel.EventStatus.EventStatusOptions.Select(x => x.EventOption);

            foreach (EventOption eventOption in EventOptions)
            {
                var eventOptionModel = new EventOptionModel(eventOption)
                {
                    IsChecked = eventStatusOptions.Contains(eventOption)
                };

                eventOptionModel.PropertyChanged += EventStatusOptionOnPropertyChanged;
                eventStatusModel.Options.Add(eventOptionModel);
            }
        }

        private void EventTypeOptionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;

            var option = sender as EventOptionModel;

            if (e.PropertyName == "IsChecked")
            {
                if (option.IsChecked)
                {
                    var eventTypeOption = new EventTypeOption()
                    {
                        ID = Guid.NewGuid(),
                        EventTypeID = SelectedEventType.EventType.ID,
                        EventOptionID = option.EventOption.ID
                    };

                    _adminDataUnit.EventTypeOptionsRepository.Add(eventTypeOption);
                }
                else
                {
                    var eventTypeOption = SelectedEventType.EventType.EventTypeOptions.FirstOrDefault(x => x.EventOptionID == option.EventOption.ID);
                    _adminDataUnit.EventTypeOptionsRepository.Delete(eventTypeOption);
                }

                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        private void EventStatusOptionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;

            var option = sender as EventOptionModel;

            if (e.PropertyName == "IsChecked")
            {
                if (option.IsChecked)
                {
                    var eventStatusOption = new EventStatusOption()
                        {
                            ID = Guid.NewGuid(),
                            EventStatusID = SelectedEventStatus.EventStatus.ID,
                            EventOptionID = option.EventOption.ID
                        };

                    _adminDataUnit.EventStatusOptionsRepository.Add(eventStatusOption);
                }
                else
                {
                    var eventStatusOption = SelectedEventStatus.EventStatus.EventStatusOptions.FirstOrDefault(x => x.EventOptionID == option.EventOption.ID);
                    _adminDataUnit.EventStatusOptionsRepository.Delete(eventStatusOption);
                }

                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        #endregion

        #region Commands

        private void AddEventTypeCommandExecuted()
        {
            var type = new EventType
                {
                    ID = Guid.NewGuid(),
                    Name = "New Event Type",
                    Colour = "#808080",
                    PreferredName = "",
                    Abbreviation = ""
                };

            _adminDataUnit.EventTypesRepository.Add(type);
            _adminDataUnit.SaveChanges();

            var typeModel = new EventTypeModel(type);
            LoadEventTypeOptions(typeModel);

            EventTypes.Add(typeModel);

            SelectedObject = typeModel;
        }

        private async void AddEventStatusCommandExecuted()
        {
            var status = new EventStatus
                {
                    ID = Guid.NewGuid(),
                    Name = "New Event Status",
                    Colour = "#808080",
                    PreferredName = ""
                };

            _adminDataUnit.EventStatusesRepository.Add(status);
            await _adminDataUnit.SaveChanges();

            var statusModel = new EventStatusModel(status);
            LoadEventStatusOptions(statusModel);

            EventStatuses.Add(statusModel);

            SelectedObject = statusModel;
        }

        private async void AddTODOStatusCommandExecuted()
        {
            var status = new FollowUpStatus
            {
                Color = "#808080",
                ID = Guid.NewGuid(),
                NumberOfDays = 0,
                Priority = 1,
                Status = "New TODO Status",
                StatusType = Convert.ToInt32(StatusType.ToDosStatus)
            };

            _adminDataUnit.FollowUpStatusesRepository.Add(status);
            await _adminDataUnit.SaveChanges();
            TODOStatuses.Add(status);
            SelectedObject = status;
            IsToDoStatusesTreeExpanded = true;

        }

        private void AddDefaultReminderCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");
            var addDefaultReminder = new AddDefaultEventTypeTODOView(SelectedObject as EventTypeModel);
            addDefaultReminder.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (addDefaultReminder.DialogResult != null && addDefaultReminder.DialogResult == true)
            {
                var viewmodel = addDefaultReminder.ViewModel as AddDefaultEventTypeTODOViewModel;
                SelectedEventType.EventTypeToDos.Add(viewmodel.EventTypeToDo);
            }
        }

        private async void SaveChangesCommandExecuted()
        {
            await _adminDataUnit.SaveChanges();
            IsDirty = false;
        }

        private bool SaveChangesCommandCanExecute()
        {
            return IsEventPropertySelected && IsDirty;
        }

        private void DeleteEventPropertyCommandExecuted()
        {
            if (SelectedObject == null) return;

            bool? dialogResult = null;
            /*string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM + Environment.NewLine
                + "Another elements of the system may depend on this item. " + Environment.NewLine
                +"The system automatically deletes all data that depend on this item. Are you sure?";*/

            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            if (SelectedObject is EventTypeModel)
            {
                // check if any product depend on this event type
                if (SelectedEventType.EventType.ProductEventTypes.Any())
                {
                    // remove all ProductEventTypes that depend on this EventType
                    _adminDataUnit.ProductEventTypesRepository.Delete(SelectedEventType.EventType.ProductEventTypes.ToList());
                }

                // delete event type options
                _adminDataUnit.EventTypeOptionsRepository.Delete(SelectedEventType.EventType.EventTypeOptions.ToList());

                // delete event type
                _adminDataUnit.EventTypesRepository.Delete(SelectedEventType.EventType);
                _adminDataUnit.SaveChanges();

                EventTypes.Remove(SelectedEventType);
                SelectedEventType = null;
            }
            else if (SelectedObject is EventStatusModel)
            {
                // delete event status options
                _adminDataUnit.EventStatusOptionsRepository.Delete(SelectedEventStatus.EventStatus.EventStatusOptions.ToList());

                // delete event status
                _adminDataUnit.EventStatusesRepository.Delete(SelectedEventStatus.EventStatus);
                _adminDataUnit.SaveChanges();

                EventStatuses.Remove(SelectedEventStatus);
                SelectedEventStatus = null;
            }
            else if (SelectedObject is FollowUpStatus)
            {
                _adminDataUnit.FollowUpStatusesRepository.Delete(SelectedTODOStatus);
                _adminDataUnit.SaveChanges();
                TODOStatuses.Remove(SelectedTODOStatus);
                SelectedTODOStatus = null;
            }
        }

        private bool DeleteEventPropertyCommandCanExecute()
        {
            return (SelectedObject is EventTypeModel || SelectedObject is EventStatusModel || SelectedObject is FollowUpStatus);
        }

        private void DeleteEventTypeToDoCommandExecuted(EventTypeToDoModel eventTypeToDoModel)
        {
            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;


            // Delete Event Type TO DO
            _adminDataUnit.EventTypeTODOsRepository.Delete(eventTypeToDoModel.EventTypeTODO);

            _adminDataUnit.SaveChanges();

            SelectedEventType.EventTypeToDos.Remove(eventTypeToDoModel);
        }

        private void EditEventTypeToDoCommandExecuted(EventTypeToDoModel eventTypeToDoModel)
        {
            RaisePropertyChanged("DisableParentWindow");

            var addDefaultEventTypeToDoview = new AddDefaultEventTypeTODOView(SelectedObject as EventTypeModel, eventTypeToDoModel);
            addDefaultEventTypeToDoview.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (addDefaultEventTypeToDoview.DialogResult != null && addDefaultEventTypeToDoview.DialogResult == true)
            {
            }
        }

        #endregion
    }
}