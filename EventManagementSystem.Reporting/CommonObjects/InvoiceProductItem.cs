using System;

namespace EventManagementSystem.Reporting.CommonObjects
{
    public class InvoiceProductItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
    }
}
