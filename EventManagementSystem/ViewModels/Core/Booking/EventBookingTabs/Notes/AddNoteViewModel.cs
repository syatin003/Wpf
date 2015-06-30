using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Notes
{
    public class AddNoteViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventsDataUnit;
        private readonly EventModel _event;
        private bool _isBusy;
        private ObservableCollection<EventNoteType> _eventNoteTypes;
        private EventNoteModel _eventNote;
        private bool _isEditMode;

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

        public ObservableCollection<EventNoteType> EventNoteTypes
        {
            get { return _eventNoteTypes; }
            set
            {
                if (_eventNoteTypes == value) return;
                _eventNoteTypes = value;
                RaisePropertyChanged(() => EventNoteTypes);
            }
        }

        public EventNoteModel EventNote
        {
            get { return _eventNote; }
            set
            {
                if (_eventNote == value) return;
                _eventNote = value;
                RaisePropertyChanged(() => EventNote);
            }
        }

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public AddNoteViewModel(EventModel eventModel, EventNoteModel noteModel)
        {
            _event = eventModel;

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessNote(noteModel);
        }

        #endregion

        #region Methods

        private void ProcessNote(EventNoteModel noteModel)
        {
            _isEditMode = (noteModel != null);

            EventNote = (_isEditMode) ? noteModel : GetNote();
            EventNote.PropertyChanged += EventNoteOnPropertyChanged;
        }

        private EventNoteModel GetNote()
        {
            var noteModel = new EventNoteModel(new EventNote()
            {
                ID = Guid.NewGuid(),
                EventID = _event.Event.ID,
                Date = DateTime.Now,
                UserID = AccessService.Current.User.ID
            });

            return noteModel;
        }

        private void EventNoteOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SubmitCommand.RaiseCanExecuteChanged();
        }

        public async void LoadData()
        {
            IsBusy = true;

            var types = await _eventsDataUnit.EventNoteTypesRepository.GetAllAsync();
            EventNoteTypes = new ObservableCollection<EventNoteType>(types);

            IsBusy = false;
        }

        #endregion

        #region Commands

        private void SubmitCommandExecuted()
        {
            if (!_isEditMode)
            {
                _eventsDataUnit.EventNotesRepository.Add(EventNote.EventNote);
            }
        }

        private bool SubmitCommandCanExecute()
        {
            return !EventNote.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            _eventsDataUnit.RevertChanges();

            if (_isEditMode)
            {
                EventNote.Refresh();
            }
        }

        #endregion
    }
}