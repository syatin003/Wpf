using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Enums.Admin;

namespace EventManagementSystem.Models
{
    public class TillDivisionGroupDepartmentModel : ModelBase
    {
        private TillDivision _tillDivision;

        public TillDivision TillDivision
        {
            get { return _tillDivision; }
            set
            {
                if (_tillDivision == value) return;
                _tillDivision = value;
                RaisePropertyChanged(() => TillDivision);
            }
        }
        private GroupDepartmentEnum _type;

        public GroupDepartmentEnum Type
        {
            get { return _type; }
            set
            {
                if (_type == value) return;
                _type = value;
                RaisePropertyChanged(() => Type);
            }
        }

    }
}
