using EventManagementSystem.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class JoinerActivityModel : ModelBase
    {
        public DateTime? DateSale { get; set; }
        public string MemberName { get; set; }
        public string MembershipNumber { get; set; }
        public string SalesPerson { get; set; }
        public string CategoryName { get; set; }
        public int Members { get; set; }
        public string ChargeType { get; set; }
        public string MethodOfPayment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public DateTime? IstPaymentMonth { get; set; }
        public double IstMonthPayment { get; set; }
        public double OutGoingMonthPayment { get; set; }
        public DateTime? LastPaymentMonth { get; set; }
        public int ContractPeriod { get; set; }
        public double AnnualFeePaid { get; set; }
        public double JoiningFeePaid { get; set; }
        public string PromoSource { get; set; }
    }
}
