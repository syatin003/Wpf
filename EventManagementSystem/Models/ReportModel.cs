using System;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class ReportModel : ModelBase
    {
        #region Fields

        private readonly Report _report;
        private String _reportColor;


        #endregion

        #region Properties

        public Report Report
        {
            get { return _report; }
        }

        public string Name
        {
            get { return _report.Name; }
        }

        public String ReportColor
        {
            get {
                if (_reportColor == null || _reportColor == String.Empty)
                    return "LightGray";
                return _reportColor; }
            set
            {
                if (_reportColor == value) return;
                _reportColor = value;
                RaisePropertyChanged(() => ReportColor);
            }
        }

        #endregion

        #region Constructor

        public ReportModel(Report report)
        {
            _report = report;
        }

        #endregion

    }
}
