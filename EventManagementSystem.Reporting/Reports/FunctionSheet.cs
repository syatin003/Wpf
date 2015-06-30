using EventManagementSystem.Reporting.CommonObjects;
using Telerik.Reporting;

namespace EventManagementSystem.Reporting.Reports
{
    /// <summary>
    /// Summary description for InvoiceReport.
    /// </summary>
    public partial class FunctionSheet : Report
    {
        public FunctionSheet(FunctionSheetReportBag bag)
        {
            InitializeComponent();
            this.DataSource = bag;
        }
    }
}