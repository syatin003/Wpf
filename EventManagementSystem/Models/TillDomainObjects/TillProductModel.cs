using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Models.TillDomainObjects
{
    public class TillProductModel
    {
        public int? Record { get; set; }
        public string Name { get; set; }
        public int GroupRecord { get; set; }
        public int DepartmentRecord { get; set; }
        public int ProductRateRecord { get; set; }
        public double? Quantity1 { get; set; }
        public double? Quantity2 { get; set; }
        public double? Quantity3 { get; set; }
        public decimal? Price1L1 { get; set; }
        public decimal? Price1L2 { get; set; }
        public decimal? Price1L3 { get; set; }
        public decimal? Price1L4 { get; set; }
        public decimal? Price2L1 { get; set; }
        public decimal? Price2L2 { get; set; }
        public decimal? Price2L3 { get; set; }
        public decimal? Price2L4 { get; set; }
        public decimal? Price3L1 { get; set; }
        public decimal? Price3L2 { get; set; }
        public decimal? Price3L3 { get; set; }
        public decimal? Price3L4 { get; set; }
    }
}
