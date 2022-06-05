using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductOutSerialInfoBO
    {
        public Int64 OutSerialId { get; set; }
        public int OutId { get; set; }
        public int ItemId { get; set; }
        public string SerialNumber { get; set; }
        public Decimal Quantity { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
    }
}
