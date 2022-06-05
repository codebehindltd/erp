using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class AbcAnalysisBO
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal NumberOfItemsSold { get; set; }
        public decimal UsageValue { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalUsage { get; set; }
        public decimal AnnualSold { get; set; }
        public decimal AnnualUsage { get; set; }
        public decimal RunningTotal { get; set; }
        public string AbcAnalysis { get; set; }
    }
}