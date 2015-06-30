using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Views.Core.Booking;

namespace EventManagementSystem.ViewModels.Core.Booking
{
    public class BookingViewModel : ViewModelBase
    {
        #region Fields

        private ContentControl _content;
        private readonly ModelBase _model;

        #endregion

        #region Properties

        public ContentControl Content
        {
            get { return _content; }
            set
            {
                if (Equals(_content, value)) return;
                _content = value;
                RaisePropertyChanged(() => Content);
            }
        }

        public RelayCommand SwitchToTeeBookingCommand { get; private set; }
        public RelayCommand SwitchToGroupBookingCommand { get; private set; }
        public RelayCommand SwitchToEventBookingCommand { get; private set; }
        public RelayCommand SwitchToCompetitionBookingCommand { get; private set; }
        public RelayCommand SwitchToMatchBookingCommand { get; private set; }

        #endregion

        #region Constructor

        public BookingViewModel(BookingViews type, ModelBase model, bool IsDuplicate)
        {
            _model = model;

            // TODO: Remove "() => false" when tab will be completed.
            SwitchToTeeBookingCommand = new RelayCommand(SwitchToTeeBookingCommandExecuted, () => false);
            SwitchToGroupBookingCommand = new RelayCommand(SwitchToGroupBookingCommandExecuted, () => false);
            SwitchToEventBookingCommand = new RelayCommand(SwitchToEventBookingCommandExecuted);
            SwitchToCompetitionBookingCommand = new RelayCommand(SwitchToCompetitionBookingCommandExecuted, () => false);
            SwitchToMatchBookingCommand = new RelayCommand(SwitchToMatchBookingCommandExecuted, () => false);
            if (IsDuplicate)
            {
                Content = new EventBookingView(_model as EventModel, IsDuplicate);
            }
            else
            {
                SwitchBookingView(type);

            }
        }

        #endregion

        #region Methods

        private void SwitchBookingView(BookingViews type)
        {
            switch (type)
            {
                case BookingViews.Tee:
                    {
                        SwitchToTeeBookingCommandExecuted();
                        break;
                    }
                case BookingViews.Group:
                    {
                        SwitchToGroupBookingCommandExecuted();
                        break;
                    }
                case BookingViews.Event:
                    {
                        SwitchToEventBookingCommandExecuted();
                        break;
                    }
                case BookingViews.Competition:
                    {
                        SwitchToCompetitionBookingCommandExecuted();
                        break;
                    }
                case BookingViews.Match:
                    {
                        SwitchToMatchBookingCommandExecuted();
                        break;
                    }
            }
        }

        #endregion

        #region Commands

        private void SwitchToMatchBookingCommandExecuted()
        {
            Content = new MatchBookingView();
        }

        private void SwitchToCompetitionBookingCommandExecuted()
        {
            Content = new CompetitionBookingView();
        }

        private void SwitchToEventBookingCommandExecuted()
        {
            Content = new EventBookingView(_model as EventModel);
        }

        private void SwitchToGroupBookingCommandExecuted()
        {
            Content = new GroupBookingView();
        }

        private void SwitchToTeeBookingCommandExecuted()
        {
            Content = new TeeBookingView();
        }

        #endregion
    }

    public enum BookingViews
    {
        Tee,
        Group,
        Event,
        Competition,
        Match
    }
}