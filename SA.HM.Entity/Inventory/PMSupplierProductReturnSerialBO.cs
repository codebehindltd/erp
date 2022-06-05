using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMSupplierProductReturnSerialBO
    {
        public Int64 ReturnSerialId { get; set; }
        public Int64 ReturnId { get; set; }
        public int ItemId { get; set; }
        public string SerialNumber { get; set; }
        public Decimal Quantity { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
    }
}
