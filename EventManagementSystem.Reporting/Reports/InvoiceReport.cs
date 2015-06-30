using EventManagementSystem.Reporting.CommonObjects;
using Telerik.Reporting;

namespace EventManagementSystem.Reporting.Reports
{
    /// <summary>
    /// Summary description for InvoiceReport.
    /// </summary>
    public partial class InvoiceReport : Report
    {
        public InvoiceReport(InvoiceReportBag reportBag)
        {
            InitializeComponent();
            this.DataSource = reportBag;
        }
    }
}