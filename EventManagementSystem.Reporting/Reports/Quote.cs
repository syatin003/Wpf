using EventManagementSystem.Reporting.CommonObjects;
using Telerik.Reporting;

namespace EventManagementSystem.Reporting.Reports
{
    /// <summary>
    /// Summary description for Quote.
    /// </summary>
    public partial class Quote : Report
    {
        public Quote(InvoiceReportBag reportBag)
        {
            InitializeComponent();
            this.DataSource = reportBag;
        }
    }
}