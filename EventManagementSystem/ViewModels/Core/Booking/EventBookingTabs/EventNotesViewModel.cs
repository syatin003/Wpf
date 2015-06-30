using System;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Notes;
using EventManagementSystem.Views.CRM;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs
{
    public class EventNotesViewModel : ViewModelBase
    {
        #region Fields

        private EventModel _event;
        private readonly IEventDataUnit _eventsDataUnit;

        #endregion

        #region Properties

        public EventModel Event
        {
            get { return _event; }
            set
            {
                if (_event == value) return;
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }

        public RelayCommand AddNoteCommand { get; private set; }
        public RelayCommand<EventNoteModel> DeleteNoteCommand { get; private set; }
        public RelayCommand<EventNoteModel> EditNoteCommand { get; private set; }
        public RelayCommand OpenEnquiryCommand { get; private set; }

        #endregion

        #region Constructor

        public EventNotesViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            AddNoteCommand = new RelayCommand(AddNoteCommandExecuted);
            DeleteNoteCommand = new RelayCommand<EventNoteModel>(DeleteNoteCommandExecuted);
            EditNoteCommand = new RelayCommand<EventNoteModel>(EditNoteCommandExecuted);
            OpenEnquiryCommand = new RelayCommand(OpenEnquiryCommandExecute);
        }

        #endregion

        #region Methods

        #endregion

        #region Commands

        private void AddNoteCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddNoteView(Event);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.Event.LastEditDate = DateTime.Now;
                _event.EventNotes.Add(window.ViewModel.EventNote);
            }
        }

        private void EditNoteCommandExecuted(EventNoteModel item)
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new AddNoteView(Event, item);
            window.ShowDialog();

            if (window.DialogResult != null && window.DialogResult.Value)
            {
                _event.Event.LastEditDate = DateTime.Now;
            }

            RaisePropertyChanged("EnableParentWindow");
        }

        private void DeleteNoteCommandExecuted(EventNoteModel item)
        {
            _event.EventNotes.Remove(item);
            _eventsDataUnit.EventNotesRepository.Delete(item.EventNote);
            _event.Event.LastEditDate = DateTime.Now;
        }

        private void OpenEnquiryCommandExecute()
        {
            RaisePropertyChanged("DisableParentWindow");

            var window = new NewEnquiryView(Event.Enquiry);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        #endregion
    }
}
