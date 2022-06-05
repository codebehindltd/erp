using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InventoryItemUsageViewBO
    {
        public int CostCenterId { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string HeadName { get; set; }
        public decimal? AverageCost { get; set; }
        public decimal? ActualUsage { get; set; }
        //public decimal? ActualQty { get; set; }
        public decimal? ActualValue { get; set; }
        //public decimal? CVQty { get; set; }
        //public decimal? CVValue { get; set; }
       // public string CountedBy { get; set; }



        public decimal NoofTurns { get; set; }
        public decimal AvgDailyUsage { get; set; }
        public decimal AvgNoofDaysOnHand { get; set; }
        public decimal PriceFluctuation { get; set; }
        public decimal UsagePerGuest { get; set; }
    }
}
