using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemAdjustmentViewBO
    {
        public ItemStockAdjustmentBO AdjustmentItem { get; set; }
        public List<ItemStockAdjustmentDetailsBO> AdjustmentItemDetails { get; set; }
        public string AdjustmentItemDetailsGrid { get; set; }
        public string ItemName { get; set; }
        public int ItemId { get; set; }
    }
}
