using System.Collections.Generic;

namespace EventManagementSystem.Reporting.CommonObjects
{
    public class FunctionSheetReportBag
    {
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string MainContact { get; set; }
        public string Telephone { get; set; }
        public string EventType { get; set; }
        public string CorrespondenceAddress { get; set; }
        public int NumberOfPeople { get; set; }
        public string Time { get; set; }

        public List<FunctionSheetEventNote> Notes { get; set; }

        public List<FunctionSheetEventItem> Items { get; set; }

        public object TotalItems { get; set; }

        public double TotalExceptVAT { get; set; }
        public double VAT { get; set; }
        public double TotalWithVAT { get; set; }
        public double LessDepositPaid { get; set; }
        public double BalanceToPay { get; set; }
    }
}
