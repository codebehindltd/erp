using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InventoryStockVarianceViewBO
    {
        //public int CostCenterId { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal? AverageCost { get; set; }
        public decimal? ExpectedQty { get; set; }
        public decimal? ActualQty { get; set; }
        public decimal? ActualValue { get; set; }
        public decimal? CVQty { get; set; }
        public decimal? CVValue { get; set; }
        public string CountedBy { get; set; }
    }
}
