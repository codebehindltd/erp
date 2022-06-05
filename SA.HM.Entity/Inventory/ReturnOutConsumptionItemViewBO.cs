using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ReturnOutConsumptionItemViewBO
    {
        public int OutId { get; set; }
        public int OutDetailsId { get; set; }
        public int ItemId { get; set; }
        public int StockById { get; set; }
        public string ItemName { get; set; }
        public string HeadName { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal Quantity { get; set; }
        public string ProductType { get; set; }
        public int FromCostCenterId { get; set; }
        public int FromLocationId { get; set; }
    }
}
