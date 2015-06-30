using System.Collections.Generic;

namespace EventManagementSystem.Reporting.CommonObjects
{
    public class InvoiceReportBag
    {
        // Invoice
        public int InvoiceNumber { get; set; }
        public string PreparedBy { get; set; }
        public string ContactAddress { get; set; }
        public string EventName { get; set; }
        public string QuoteDate { get; set; }
        public string Logo { get; set; }

        // Event
        public string EventDate { get; set; }

        public double TotalExceptVAT { get; set; }
        public double VAT { get; set; }
        public double TotalWithVAT { get; set; }
        public double LessDepositPaid { get; set; }
        public double BalanceToPay { get; set; }

        // Golf club info
        public string GolfClubHeaderInfo { get; set; }
        public string GolfClubLeftFooterInfo { get; set; }
        public string GolfClubRightFooterInfo { get; set; }
        public string CorrespondenceTokens { get; set; }

        public List<InvoiceProductItem> InvoiceProductItems { get; set; }
    }
}
