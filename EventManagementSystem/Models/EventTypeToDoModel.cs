using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventTypeToDoModel : ModelBase, IDataErrorInfo
    {

        #region Fields

        private readonly EventTypeTODO _eventTypeTODO;


        #endregion Fields

        #region Properties

        [DataMember]
        public EventTypeTODO EventTypeTODO
        {
            get { return _eventTypeTODO; }
        }

        [DataMember]
        public string WhatToDo
        {
            get { return _eventTypeTODO.WhatToDo; }
            set
            {
                if (_eventTypeTODO.WhatToDo == value) return;
                _eventTypeTODO.WhatToDo = value;
                RaisePropertyChanged(() => WhatToDo);
            }
        }

        [DataMember]
        public Int32 NumberOfDays
        {
            get { return _eventTypeTODO.NumberOfDays; }
            set
            {
                if (_eventTypeTODO.NumberOfDays == value) return;
                _eventTypeTODO.NumberOfDays = value;
                RaisePropertyChanged(() => NumberOfDays);
            }
        }

        [DataMember]
        public User AssignedToUser
        {
            get { return _eventTypeTODO.User1; }
            set
            {
                if (_eventTypeTODO.User1 == value) return;
                _eventTypeTODO.User1 = value;
                RaisePropertyChanged(() => AssignedToUser);
            }
        }

       

        [DataMember]
        public Int32 RelatedDateType
        {
            get { return _eventTypeTODO.RelatedDateType; }
            set
            {
                if (_eventTypeTODO.RelatedDateType == value) return;
                _eventTypeTODO.RelatedDateType = value;
                RaisePropertyChanged(() => RelatedDateType);
                RaisePropertyChanged(() => DateTypeName);
            }
        }

        public String DateTypeName
        {
            get
            {
                return Enum.GetName(typeof(EventManagementSystem.Enums.Admin.RelatedDateType), _eventTypeTODO.RelatedDateType);
            }
        }

       
        #endregion Properties

        #region Constructor

        public EventTypeToDoModel(EventTypeTODO eventTypeToDo)
        {
            _eventTypeTODO = eventTypeToDo;
        }

        #endregion

        #region IDataErrorInfo Implementation

        public bool HasErrors
        {
            get { return typeof(EventTypeToDoModel).GetProperties().Any(prop => !string.IsNullOrEmpty(this[prop.Name])); }
        }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;

                if (columnName == "WhatToDo")
                    if (string.IsNullOrWhiteSpace(WhatToDo))
                        Error = "WhatToDo text can't be empty.";
                if (columnName == "NumberOfDays")
                    if (string.IsNullOrWhiteSpace(Convert.ToString(NumberOfDays)))
                        Error = "Number Of Days can't be empty.";
                if (columnName == "AssignedToUser")
                    if (AssignedToUser == null)
                        Error = "Assigned to user can not be null";
                return Error;
            }
        }

        public string Error { get; private set; }

        #endregion
    }
}
