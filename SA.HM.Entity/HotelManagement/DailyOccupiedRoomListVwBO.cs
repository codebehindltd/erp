using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class DailyOccupiedRoomListVwBO
    {
        public DateTime ServiceDate { get; set; }
        public string RoomType { get; set; }       
        public int RoomCount { get; set; }
    }
}
