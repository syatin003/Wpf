using EventManagementSystem.Core.ViewModels;
using System;
namespace EventManagementSystem.Models
{
    [Serializable]
    public class JoinerLeaverModel : ModelBase
    {
        public string MemberShipGroup { get; set; }
        public string CategoryName { get; set; }
        public int Opening { get; set; }
        public int Joiners { get; set; }
        public int Leavers { get; set; }
        public int Closing { get; set; }
        public int TransfersIn { get; set; }
        public int TransfersOut { get; set; }
    }
}
