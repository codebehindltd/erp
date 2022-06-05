using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemStockInformationBO
    {
        public int StockId { get; set; }
        public int LocationId { get; set; }
        public int ItemId { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal AverageCost { get; set; }
        public string HeadName { get; set; }
    }
}
