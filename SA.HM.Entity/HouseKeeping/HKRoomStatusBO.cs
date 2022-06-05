using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HKRoomStatusBO
    {
        public long HKRoomStatusId { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
    }
}
