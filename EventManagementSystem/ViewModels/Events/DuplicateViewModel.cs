using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.Views.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.ViewModels.Events
{
    public class DuplicateViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields

        private String _eventName;
        private DateTime? _eventDate;
        private bool _isBusy;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }
        public String EventName
        {
            get { return _eventName; }
            set
            {
                if (_eventName == value) return;
                _eventName = value;
                RaisePropertyChanged(() => EventName);
            }
        }

        public DateTime? EventDate
        {
            get { return _eventDate; }
            set
            {
                if (_eventDate == value) return;
                _eventDate = value;
                RaisePropertyChanged(() => EventDate);
            }
        }

        public RelayCommand OKCommand { get; private set; }

        #endregion

        #region Constructors

        public DuplicateViewModel()
        {
            OKCommand = new RelayCommand(OKCommandExecuted, OKCommandCanExecute);
            this.PropertyChanged += DuplicateViewModel_PropertyChanged;
        }

        private void DuplicateViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OKCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Commands

        private void OKCommandExecuted()
        {

        }

        private bool OKCommandCanExecute()
        {
            return !HasErrors;
        }


        #endregion


        #region IDataError Implementation

        public bool HasErrors
        {
            get { return typeof(DuplicateViewModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }


        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;


                if (columnName == "EventName")
                    if (string.IsNullOrWhiteSpace(EventName))
                        Error = "Event Name can't be empty";

                if (columnName == "EventDate")
                    if (EventDate == null)
                        Error = "Event Date can't be empty";
                return Error;
            }
        }
        public string Error { get; private set; }

        #endregion

    }
}
