using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class InvItemViewBO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal UnitPriceUsd { get; set; }
    }
}
