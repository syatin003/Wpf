using EventManagementSystem.Reporting.CommonObjects;
using Telerik.Reporting;

namespace EventManagementSystem.Reporting.Reports
{
    /// <summary>
    /// Summary description for Confirmation.
    /// </summary>
    public partial class Confirmation : Report
    {
        public Confirmation(InvoiceReportBag reportBag)
        {
            InitializeComponent();
            this.DataSource = reportBag;
        }
    }
}