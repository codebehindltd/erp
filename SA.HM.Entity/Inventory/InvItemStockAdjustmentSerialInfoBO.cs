using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemStockAdjustmentSerialInfoBO
    {
        public int Id { get; set; }
        public int StockAdjustmentId { get; set; }
        public int? LocationId { get; set; }
        public int? ItemId { get; set; }
        public string SerialNumber { get; set; }
    }
}
