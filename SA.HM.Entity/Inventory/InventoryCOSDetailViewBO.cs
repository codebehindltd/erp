using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InventoryCOSDetailViewBO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string StockBy { get; set; }
        public decimal? AverageCost { get; set; }
        public decimal? BeginingStock { get; set; }
        public decimal? PurchaseQuantity { get; set; }
        public decimal? ActualUsageQuantity { get; set; }
        public decimal? ActualUsageCost { get; set; }
        public decimal? EndingQuantiy { get; set; }
        public decimal? SalesQuantity { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? Costs { get; set; }
        public decimal? CostsOfSales { get; set; }
    }
}
