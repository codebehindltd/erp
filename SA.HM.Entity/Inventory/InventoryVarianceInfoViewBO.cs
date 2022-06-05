using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InventoryVarianceInfoViewBO
    {
        public int CostCenterId { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal? WastageQuantity { get; set; }
        public decimal? WastageCost { get; set; }
        public string WastageAllowance { get; set; }
        public DateTime? StockVarianceDate { get; set; }
        public string EnteredBy { get; set; }
        public string WastageReason { get; set; }        
    }
}
