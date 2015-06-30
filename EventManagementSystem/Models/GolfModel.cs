using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class GolfModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly Golf _golf;
        private GolfModel _turnDefaultGolf;
        private ObservableCollection<GolfModel> _availableGolfs;
        private Boolean _isSelectedAsFollowResource;
        #endregion

        #region Properties

        public Golf Golf
        {
            get { return _golf; }
        }



        public string Name
        {
            get { return _golf.Name; }
            set
            {
                if (_golf.Name == value) return;
                _golf.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string Color
        {
            get { return _golf.Color; }
            set
            {
                if (_golf.Color == value) return;
                _golf.Color = value;
                RaisePropertyChanged(() => Color);
            }
        }

        public GolfModel TurnDefaultGolf
        {
            get { return _turnDefaultGolf; }
            set
            {
                if (_turnDefaultGolf == value) return;
                _turnDefaultGolf = value;
                _golf.TurnDefault = (_turnDefaultGolf != null) ? _turnDefaultGolf.Golf.ID : (Guid?)null;

                RaisePropertyChanged(() => TurnDefaultGolf);
            }
        }

        public ObservableCollection<GolfModel> AvailableGolfs
        {
            get { return _availableGolfs; }
            set
            {
                if (_availableGolfs == value) return;
                _availableGolfs = value;
                RaisePropertyChanged(() => AvailableGolfs);
            }
        }

        #endregion

        #region Constructors

        public GolfModel(Golf golf)
        {
            _golf = golf;
        }

        #endregion

        #region IDataError Implementation

        public bool HasErrors
        {
            get { return typeof(GolfModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "Name")
                    if (string.IsNullOrWhiteSpace(Name))
                        Error = "Name can't be empty";

                if (columnName == "Color")
                    if (string.IsNullOrWhiteSpace(Color))
                        Error = "Color can't be empty";

                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion

        internal void Refresh()
        {
            RaisePropertyChanged(() => AvailableGolfs);
            RaisePropertyChanged(() => TurnDefaultGolf);
        }
    }
}
