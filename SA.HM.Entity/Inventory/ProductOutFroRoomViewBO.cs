using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ProductOutFroRoomViewBO
    {
        public HotelRoomInventoryBO ProductOut { get; set; }
        public List<HotelRoomInventoryDetailsBO> ProductOutDetails { get; set; }
    }
}
