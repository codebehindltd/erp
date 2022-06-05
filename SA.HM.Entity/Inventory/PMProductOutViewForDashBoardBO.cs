using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductOutViewForDashBoardBO
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string ProductType { get; set; }
        public string SerialNumber { get; set; }
        public string UnitHead { get; set; }
    }
}
