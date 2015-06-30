using System;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Events
{
    public class AddCalendarNoteViewModel : ViewModelBase
    {
        #region Fields

        private CalendarNoteModel _calendarNote;
        private bool _isEdit;
        private readonly IEventDataUnit _eventsDataUnit;

        #endregion

        #region Properties

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public CalendarNoteModel CalendarNote
        {
            get { return _calendarNote; }
            set
            {
                if (_calendarNote == value) return;
                _calendarNote = value;
                RaisePropertyChanged(() => CalendarNote);
            }
        }

        #endregion

        #region Constructor

        public AddCalendarNoteViewModel(CalendarNoteModel note)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            SaveCommand = new RelayCommand(SaveCommandExecuted, SaveCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);

            ProcessNote(note);
        }

        #endregion

        #region Methods

        private void ProcessNote(CalendarNoteModel note)
        {
            _isEdit = note != null;

            CalendarNote = _isEdit ? note : GetCalendarNote();
            CalendarNote.PropertyChanged += (sender, args) => SaveCommand.RaiseCanExecuteChanged();
        }

        private CalendarNoteModel GetCalendarNote()
        {
            return new CalendarNoteModel(new CalendarNote()
            {
                ID = Guid.NewGuid(),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2) // Add 2 hours to end date by default
            });
        }

        #endregion

        #region Commands

        private async void SaveCommandExecuted()
        {
            if (!_isEdit)
            {
                _eventsDataUnit.CalendarNotesRepository.Add(CalendarNote.CalendarNote);
            }
            
            await _eventsDataUnit.SaveChanges();
        }

        private bool SaveCommandCanExecute()
        {
            return !_calendarNote.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            _eventsDataUnit.RevertChanges();
        }

        #endregion
    }
}
