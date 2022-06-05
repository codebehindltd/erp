using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InventoryItemUsageAnalysisBO
    {
        public string ItemCode { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal? AverageCost { get; set; }
        public decimal? ActualUsage { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? ActualValue { get; set; }
        public decimal? AvgDailyUsage { get; set; }
        public decimal? AverageDailyUsage { get; set; }
        public decimal Turn { get; set; }
        public decimal AverageOfDaysHand { get; set; }
        public decimal? PriceFluctuation { get; set; }
        public decimal? UsagePerGuest { get; set; }
    }
}
