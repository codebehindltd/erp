using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemVarianceViewBO
    {
        public InvItemStockVarianceBO StockVariance { get; set; }
        public List<InvItemStockVarianceDetailsBO> StockVarianceDetails { get; set; }
        public string AdjustmentItemDetailsGrid { get; set; }
        public string ItemName { get; set; }
        public int ItemId { get; set; }
    }
}
