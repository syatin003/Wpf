using EventManagementSystem.Core.ViewModels;
using System;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class LeaverModel : ModelBase
    {
        public DateTime? ResignDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string MembershipNumber { get; set; }
        public string MemberName { get; set; }
        public string CategoryName { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public int LinkedMembers { get; set; }
        public DateTime? MembershipStart { get; set; }
        public DateTime? MembershipEnd { get; set; }
        public int ContractPeriod { get; set; }
        public string LastDDMonth { get; set; }
    }
}
