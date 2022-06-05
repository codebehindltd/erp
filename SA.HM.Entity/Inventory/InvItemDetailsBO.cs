using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemDetailsBO
    {
        public int DetailId { get; set; }
        public int ItemId { get; set; }
        public int CostCenterId { get; set; }
        public int ItemDetailId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
    }
}
