using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomStatusInfoSummaryBO
    {
        public string RoomType { get; set; }
        public int TotalRoom { get; set; }
        public int Reserved { get; set; }
        public int Booked { get; set; }
        public int RoomAvailable { get; set; }
    }
}
