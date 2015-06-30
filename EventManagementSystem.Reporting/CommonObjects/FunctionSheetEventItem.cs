using System;
using System.Collections.Generic;

namespace EventManagementSystem.Reporting.CommonObjects
{
    public class FunctionSheetEventItem
    {
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public string DisplayTime { get; set; }

        public List<string> Products { get; set; }

        public FunctionSheetEventItem()
        {
            Products = new List<string>();
        }
    }
}
