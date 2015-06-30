using EventManagementSystem.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class TillDivisionModel : ModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly TillDivision _tillDivision;
        private Till _masterTill;
        private ObservableCollection<TillModel> _tills;
        private bool _isExpanded;

        #endregion Fields

        #region Properties
        [DataMember]
        public TillDivision TillDivision
        {
            get { return _tillDivision; }
        }


        [DataMember]
        public String Name
        {
            get { return _tillDivision.Name; }
            set
            {
                if (_tillDivision.Name == value) return;
                _tillDivision.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public Till Mastertill
        {
            get { return _masterTill; }
            set
            {
                if (_masterTill == value) return;
                _masterTill = value;
                _tillDivision.MasterTillID = _masterTill.ID;
                RaisePropertyChanged(() => Mastertill);
            }
        }

        public ObservableCollection<TillModel> Tills
        {
            get { return _tills; }
            set
            {
                if (_tills == value) return;
                _tills = value;
                RaisePropertyChanged(() => Tills);
            }
        }

        public List<TillDivisionGroupDepartmentModel> GroupDepList
        {
            get
            {
                return new List<TillDivisionGroupDepartmentModel>()
                {
                    new TillDivisionGroupDepartmentModel()
                    {
                        TillDivision=TillDivision,
                        Type=EventManagementSystem.Enums.Admin.GroupDepartmentEnum.Departments
                    },
                    new TillDivisionGroupDepartmentModel()
                    {
                        TillDivision=TillDivision,
                        Type=EventManagementSystem.Enums.Admin.GroupDepartmentEnum.Groups
                    }
                };
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                RaisePropertyChanged(() => IsExpanded);
            }
        }


        #endregion Properties


        #region Constructor

        public TillDivisionModel(TillDivision tillDivision)
        {
            _tillDivision = tillDivision;
            if (_tillDivision.Till != null)
                Mastertill = _tillDivision.Till;
            if (_tillDivision.Tills != null)
                Tills = new ObservableCollection<TillModel>(_tillDivision.Tills.Select(till => new TillModel(till)));
        }
        #endregion Constructor


        #region ErrorInfo
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }
        #endregion ErrorInfo
    }
}
