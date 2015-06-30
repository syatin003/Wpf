using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Models;
using Telerik.Windows.Controls;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Reminders;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using EventManagementSystem.Core.Serialization;

namespace EventManagementSystem.ViewModels.Events
{
    public class RemindersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventDataUnit _eventDataUnit;
        private bool _isBusy;
        private ObservableCollection<EventReminderModel> _eventReminders;
        private DateTime _selectedDate;
        public List<EventReminderModel> _allEventReminders;
        private bool _isDataLoadedOnce;



        #endregion Fields

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

        public ObservableCollection<EventReminderModel> EventReminders
        {
            get
            { return _eventReminders; }
            set
            {
                if (_eventReminders == value) return;
                _eventReminders = value;
                RaisePropertyChanged(() => EventReminders);
            }
        }

        public bool IsDataLoadedOnce
        {
            get { return _isDataLoadedOnce; }
            set
            {
                if (_isDataLoadedOnce == value) return;
                _isDataLoadedOnce = value;
                RaisePropertyChanged(() => IsDataLoadedOnce);
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                RaisePropertyChanged(() => SelectedDate);
                if (IsDataLoadedOnce)
                    OnSelectedDateChanged();
            }
        }

        public RelayCommand<EventReminderModel> DeleteReminderCommand { get; private set; }
        public RelayCommand<EventReminderModel> EditReminderCommand { get; private set; }

        #endregion Properties

        #region Constructor

        public RemindersViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();
            DeleteReminderCommand = new RelayCommand<EventReminderModel>(DeleteReminderCommandExecuted);
            EditReminderCommand = new RelayCommand<EventReminderModel>(EditReminderCommandExecuted);
        }

        #endregion Constructor

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;

            _eventDataUnit.EventRemindersRepository.Refresh();

            var reminders = await _eventDataUnit.EventRemindersRepository.GetAllAsync();
            _allEventReminders = new List<EventReminderModel>(reminders.Select(x => new EventReminderModel(x)).OrderBy(x => x.EventReminder.Status).ThenByDescending(x => x.DateDue));
            EventReminders = new ObservableCollection<EventReminderModel>(_allEventReminders);

            IsBusy = false;

            IsDataLoadedOnce = true;
        }

        private void OnSelectedDateChanged()
        {
            IsBusy = true;

            EventReminders = new ObservableCollection<EventReminderModel>(_allEventReminders.Where(x => x.DateDue.Date == SelectedDate.Date));

            IsBusy = false;

        }

        #endregion Methods

        #region Commands

        private void EditReminderCommandExecuted(EventReminderModel eventReminderModel)
        {
            RaisePropertyChanged("DisableParentWindow");
            var eventReminderView = new AddEventReminderView(eventReminderModel);

            eventReminderView.ShowDialog();

            if (eventReminderView.DialogResult != null && eventReminderView.DialogResult == true)
                _eventDataUnit.SaveChanges();
            RaisePropertyChanged("EnableParentWindow");

        }
        private void DeleteReminderCommandExecuted(EventReminderModel eventReminderModel)
        {
            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            _eventDataUnit.EventRemindersRepository.Delete(eventReminderModel.EventReminder);
            _eventDataUnit.SaveChanges();
            EventReminders.Remove(eventReminderModel);
        }
        #endregion Commands
    }
}
