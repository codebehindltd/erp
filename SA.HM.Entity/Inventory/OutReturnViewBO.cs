using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class OutReturnViewBO
    {
        public PMProductReturnBO ProductReturn { get; set; }
        public List<PMProductReturnDetailsBO> ProductReturnDetails { get; set; }
        public List<PMProductReturnSerialBO> ProductReturnSerialInfo { get; set; }
    }
}
