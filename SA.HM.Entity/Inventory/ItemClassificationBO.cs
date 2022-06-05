using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemClassificationBO
    {
        public int ClassificationId { get; set; }
        public string ClassificationName { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool ActiveStat { get; set; }
        public int BillId { get; set; }
        public int DiscountId { get; set; }
    }
}
