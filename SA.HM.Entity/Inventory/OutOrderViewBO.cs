using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class OutOrderViewBO
    {
        public PMProductOutBO ProductOut { get; set; }
        public List<PMProductOutDetailsBO> ProductOutDetails { get; set; }
        public List<PMProductOutSerialInfoBO> ProductSerialInfo { get; set; }
    }
}
