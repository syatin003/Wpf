using System;
using System.Windows;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Core.Booking.EventBookingTabs.Items;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;
using EventManagementSystem.Core.Serialization;
using System.Linq;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EventManagementSystem.Views.ContactManager.ContactManagerTabs;
using EventManagementSystem.Services;
using EventManagementSystem.Views.Core.Booking;

namespace EventManagementSystem.ViewModels.Events
{
    public class EventsBookedViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<EventModel> _boookedEvents;
        private readonly IEventDataUnit _eventsDataUnit;

        #endregion

        #region Properties

        public ObservableCollection<EventModel> Events
        {
            get { return _boookedEvents; }
            set
            {
                if (_boookedEvents == value) return;
                _boookedEvents = value;
                RaisePropertyChanged(() => Events);
            }
        }

        public RelayCommand<EventModel> EditEventCommand { get; private set; }
        public RelayCommand<EventModel> DetailsItemCommand { get; set; }

        #endregion

        #region Constructor

        public EventsBookedViewModel(ObservableCollection<EventModel> model)
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _eventsDataUnit = dataUnitLocator.ResolveDataUnit<IEventDataUnit>();

            EditEventCommand = new RelayCommand<EventModel>(EditEventCommandExecuted, EditEventCommandCanExecute);
            DetailsItemCommand = new RelayCommand<EventModel>(DetailsItemCommandExecuted);
            Events = model;

        }

        #endregion


        #region Commands

        private bool EditEventCommandCanExecute(EventModel arg)
        {
            return AccessService.Current.UserHasPermissions(Properties.Resources.PERMISSION_EDIT_EVENT_ALLOWED);
        }

        private void DetailsItemCommandExecuted(EventModel item)
        {
            RaisePropertyChanged("DisableParentWindow");
            var view = new EventDetailsView(item);
            view.ShowDialog();
            RaisePropertyChanged("EnableParentWindow");

        }

        private void EditEventCommandExecuted(EventModel item)
        {
            RaisePropertyChanged("DisableParentWindow");

            var bookingView = new BookingView(EventManagementSystem.ViewModels.Core.Booking.BookingViews.Event, item);

            bookingView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");

            if (bookingView.DialogResult != null && bookingView.DialogResult == true)
            {
                item.Refresh();
                item.RefreshItems();
            }
            else
            {
                item.Refresh();
            }

        }

        #endregion
    }
}